using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic;

public class TheaterLogic
{
    private readonly ITheaterRepository _repository;

    public TheaterLogic(ITheaterRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Theater>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Theater?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task AddAsync(Theater theater, List<int>? movieIds)
    {
        await _repository.AddAsync(theater);

        if (movieIds != null && movieIds.Any())
        {
            await _repository.UpdateMovieLinksAsync(theater.Theaterid, movieIds);
        }

        var seats = new List<Seat>();
        for (char row = 'A'; row <= 'D'; row++)
        {
            for (int num = 1; num <= 10; num++)
            {
                decimal price = row switch
                {
                    'A' => 150,
                    'B' => 200,
                    'C' => 250,
                    'D' => 300,
                    _ => 200
                };
                seats.Add(new Seat { Theaterid = theater.Theaterid, Row = row.ToString(), Number = num, Baseprice = price });
            }
        }

        await _repository.AddSeatsAsync(seats);
    }

    public async Task UpdateAsync(Theater theater, List<int>? movieIds)
    {
        await _repository.UpdateMovieLinksAsync(theater.Theaterid, movieIds ?? new List<int>());
        await _repository.UpdateAsync(theater);
    }

    public async Task DeleteAsync(Theater theater) => await _repository.DeleteAsync(theater);

    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() => await _repository.GetAllShowtemplatesAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) => await _repository.GetShowtemplateByIdAsync(id);

    public async Task<Showtemplate> AddShowtemplateAsync(ShowtemplateDto dto)
    {
        var template = new Showtemplate
        {
            Movieid = dto.Movieid,
            Theaterid = dto.Theaterid,
            Language = dto.Language,
            Format = dto.Format,
            Baseprice = dto.Baseprice
        };

        bool linkExists = await _repository.MovieTheaterLinkExistsAsync(dto.Movieid, dto.Theaterid);
        if (!linkExists)
            await _repository.AddMovieTheaterLinkAsync(dto.Movieid, dto.Theaterid);

        return await _repository.AddShowtemplateAsync(template);
    }

    public async Task UpdateShowtemplateAsync(int id, ShowtemplateDto dto)
    {
        var existing = await _repository.GetShowtemplateByIdAsync(id);
        if (existing == null) return;

        existing.Baseprice = dto.Baseprice;
        existing.Format = dto.Format;
        existing.Language = dto.Language;
        existing.Movieid = dto.Movieid;
        existing.Theaterid = dto.Theaterid;

        await _repository.UpdateShowtemplateAsync(existing);
    }

    public async Task DeleteShowtemplateAsync(int id)
    {
        var existing = await _repository.GetShowtemplateByIdAsync(id);
        if (existing != null)
            await _repository.DeleteShowtemplateAsync(existing);
    }

    public async Task<List<Showinstance>> GetAllShowinstancesAsync() => await _repository.GetAllShowinstancesAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) => await _repository.GetShowinstanceByIdAsync(id);

    public async Task<List<Showinstance>> GetShowinstancesByMovieAsync(int movieId, Showtemplate? st)
    {
        return await _repository.GetShowinstancesByMovieAsync(movieId);
    }
    
    public async Task<List<ShowinstanceDto>> GetShowsByFiltersAsync(int movieId, int theaterId, DateOnly date)
    {
        var instances = await _repository.GetShowinstancesByMovieTheaterAndDateAsync(movieId, theaterId, date);
        return instances.Select(s => new ShowinstanceDto
        {
            Showinstanceid = s.Showinstanceid,
            Showdate = s.Showdate ?? DateOnly.MinValue,
            Showtime = s.Showtime ?? TimeOnly.MinValue,
            Availableseats = s.Availableseats,
            Showtemplateid = s.Showtemplateid ?? 0
        }).OrderBy(s => s.Showtime).ToList();
    }

    public async Task<List<Showseatstatus>> GetSeatStatusesByShowInstanceIdAsync(int showInstanceId)
    {
        return await _repository.GetSeatStatusesByShowInstanceIdAsync(showInstanceId);
    }

    public async Task<List<SeatStatusDto>> GetSeatStatusDtosByShowInstanceIdAsync(int showInstanceId)
    {
         var statuses = await _repository.GetSeatStatusesByShowInstanceIdAsync(showInstanceId);
         return statuses.Select(s => new SeatStatusDto
         {
             Seatid = s.Seatid,
             Row = s.Seat?.Row ?? "",
             Number = s.Seat?.Number ?? 0,
             Isbooked = s.Isbooked,
             Price = s.Seat?.Baseprice ?? 0
         }).OrderBy(s => s.Row).ThenBy(s => s.Number).ToList();
    }

    public async Task<Showinstance> AddShowinstanceAsync(ShowinstanceDto dto)
    {
        var showtemplate = await _repository.GetShowtemplateWithTheaterAsync(dto.Showtemplateid)
            ?? throw new Exception("Invalid Showtemplate ID");

        if (showtemplate.Theater == null)
            throw new Exception("Theater information is missing for the specified Showtemplate.");

        var showinstance = new Showinstance
        {
            Showtemplateid = dto.Showtemplateid,
            Showdate = dto.Showdate,
            Showtime = dto.Showtime,
            Availableseats = showtemplate.Theater.Capacity
        };

        await _repository.AddShowinstanceAsync(showinstance);

        var seats = await _repository.GetSeatsByTheaterIdAsync(showtemplate.Theaterid ?? 0);

        var seatStatuses = seats.Select(seat => new Showseatstatus
        {
            Seatid = seat.Seatid,
            Showinstanceid = showinstance.Showinstanceid,
            Isbooked = false
        }).ToList();

        await _repository.AddShowseatstatusesAsync(seatStatuses);
        
        return showinstance;
    }

    public async Task<bool> UpdateShowinstanceAsync(int id, ShowinstanceDto dto)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        var showtemplate = await _repository.GetShowtemplateWithTheaterAsync(dto.Showtemplateid);
        if (showtemplate == null || showtemplate.Theater == null)
            throw new Exception("Invalid Showtemplate ID or missing Theater information.");

        instance.Showtemplateid = dto.Showtemplateid;
        instance.Showdate = dto.Showdate;
        instance.Showtime = dto.Showtime;
        instance.Availableseats = showtemplate.Theater.Capacity;

        await _repository.UpdateShowinstanceAsync(instance);
        return true;
    }

    public async Task<bool> DeleteShowinstanceAsync(int id)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        var seatStatuses = await _repository.GetSeatStatusesByShowInstanceIdAsync(id);
        await _repository.RemoveShowseatstatusesAsync(seatStatuses);

        await _repository.DeleteShowinstanceAsync(instance);
        return true;
    }
}
