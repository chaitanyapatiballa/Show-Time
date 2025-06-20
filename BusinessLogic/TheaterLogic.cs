using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class TheaterLogic
{
    private readonly AppDbContext _context;
    private readonly TheaterRepository _repository;

    public TheaterLogic(AppDbContext context)
    {
        _context = context;
        _repository = new TheaterRepository(context);
    }

    public async Task<List<Theater>> GetAllAsync() => await _context.Theaters.Include(t => t.Movietheater).ThenInclude(mt => mt.Movie).ToListAsync();

    public async Task<Theater?> GetByIdAsync(int id) => await _context.Theaters.Include(t => t.Movietheater).ThenInclude(mt => mt.Movie).FirstOrDefaultAsync(t => t.Theaterid == id);

    public async Task AddAsync(Theater theater, List<int>? movieIds)
    {
        if ((movieIds?.Any()) == true)
        {
            var movies = await _context.Movies.Where(m => movieIds.Contains(m.Movieid)).ToListAsync();
            theater.Movietheater = movies.Select(m => new Movietheater { Movieid = m.Movieid, Theater = theater }).ToList();
        }

        await _repository.AddAsync(theater);

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

        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theater theater, List<int>? movieIds)
    {
        var existingLinks = _context.Movietheater.Where(mt => mt.Theaterid == theater.Theaterid);
        _context.Movietheater.RemoveRange(existingLinks);

        if ((movieIds?.Any()) != true)
        {

        }
        else
        {
            var movies = await _context.Movies.Where(m => movieIds.Contains(m.Movieid)).ToListAsync();
            theater.Movietheater = movies.Select(m => new Movietheater { Movieid = m.Movieid, Theaterid = theater.Theaterid }).ToList();
        }

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

        bool linkExists = await _context.Movietheater.AnyAsync(mt => mt.Movieid == dto.Movieid && mt.Theaterid == dto.Theaterid);
        if (!linkExists)
            _context.Movietheater.Add(new Movietheater { Movieid = dto.Movieid, Theaterid = dto.Theaterid });

        _context.Showtemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateShowtemplateAsync(int id, ShowtemplateDto dto)
    {
        var existing = await _context.Showtemplates.FindAsync(id);
        if (existing == null) return;

        existing.Baseprice = dto.Baseprice;
        existing.Format = dto.Format;
        existing.Language = dto.Language;
        existing.Movieid = dto.Movieid;
        existing.Theaterid = dto.Theaterid;

        _context.Showtemplates.Update(existing);
        await _context.SaveChangesAsync();
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
        return await _context.Showinstances
            .Include(si => si.Showtemplate)
            .ThenInclude(showtemplate => showtemplate!.Theater) 
            .Where(si => si.Showtemplate != null && si.Showtemplate.Movieid == movieId && si.Showdate >= DateOnly.FromDateTime(DateTime.Today))
            .ToListAsync();
    }

    public async Task<List<Showseatstatus>> GetSeatStatusesByShowInstanceIdAsync(int showInstanceId)
    {
        return await _context.Showseatstatuses
            .Include(s => s.Seat)
            .Where(s => s.Showinstanceid == showInstanceId)
            .ToListAsync();
    }

    public async Task<Showinstance> AddShowinstanceAsync(ShowinstanceDto dto)
    {
        var showtemplate = await _context.Showtemplates.Include(st => st.Theater).FirstOrDefaultAsync(st => st.Showtemplateid == dto.Showtemplateid)
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

        _context.Showinstances.Add(showinstance);
        await _context.SaveChangesAsync();

        var seats = await _context.Seats.Where(s => s.Theaterid == showtemplate.Theaterid).ToListAsync();

        var seatStatuses = seats.Select(seat => new Showseatstatus
        {
            Seatid = seat.Seatid,
            Showinstanceid = showinstance.Showinstanceid,
            Isbooked = false
        }).ToList();

        _context.Showseatstatuses.AddRange(seatStatuses);
        await _context.SaveChangesAsync();

        return showinstance;
    }

    public async Task<bool> UpdateShowinstanceAsync(int id, ShowinstanceDto dto)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        var showtemplate = await _context.Showtemplates.Include(st => st.Theater).FirstOrDefaultAsync(st => st.Showtemplateid == dto.Showtemplateid);
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

        var seatStatuses = await _context.Showseatstatuses.Where(s => s.Showinstanceid == id).ToListAsync();
        _context.Showseatstatuses.RemoveRange(seatStatuses);

        await _repository.DeleteShowinstanceAsync(instance);
        await _context.SaveChangesAsync();
        return true;
    }
}
