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

    public async Task<List<Theater>> GetAllAsync()
    {
        return await _context.Theaters
            .Include(t => t.MovieTheaters)
            .ThenInclude(mt => mt.Movie)
            .ToListAsync();
    }

    public async Task<Theater?> GetByIdAsync(int id)
    {
        return await _context.Theaters
            .Include(t => t.MovieTheaters)
            .ThenInclude(mt => mt.Movie)
            .FirstOrDefaultAsync(t => t.Theaterid == id);
    }

    public async Task AddAsync(Theater theater, List<int>? movieIds)
    {
        if (movieIds != null && movieIds.Any())
        {
            var movies = await _context.Movies
                .Where(m => movieIds.Contains(m.Movieid))
                .ToListAsync();

            theater.MovieTheaters = movies.Select(m => new MovieTheater
            {
                Movieid = m.Movieid,
                Theater = theater
            }).ToList();
        }

        await _repository.AddAsync(theater);
    }

    public async Task UpdateAsync(Theater theater, List<int>? movieIds)
    {
        // Remove existing MovieTheater links
        var existingLinks = _context.MovieTheaters.Where(mt => mt.Theaterid == theater.Theaterid);
        _context.MovieTheaters.RemoveRange(existingLinks);

        if (movieIds != null && movieIds.Any())
        {
            var movies = await _context.Movies
                .Where(m => movieIds.Contains(m.Movieid))
                .ToListAsync();

            theater.MovieTheaters = movies.Select(m => new MovieTheater
            {
                Movieid = m.Movieid,
                Theaterid = theater.Theaterid
            }).ToList();
        }

        await _repository.UpdateAsync(theater);
    }

    public async Task DeleteAsync(Theater theater)
    {
        await _repository.DeleteAsync(theater);
    }

    // Showtemplate
    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() =>
        await _repository.GetAllShowtemplatesAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) =>
        await _repository.GetShowtemplateByIdAsync(id);

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

        bool linkExists = await _context.MovieTheaters.AnyAsync(mt =>
            mt.Movieid == template.Movieid && mt.Theaterid == template.Theaterid);

        if (!linkExists)
        {
            _context.MovieTheaters.Add(new MovieTheater
            {
                Movieid = template.Movieid.Value,
                Theaterid = template.Theaterid.Value
            });
        }

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
        {
            await _repository.DeleteShowtemplateAsync(existing);
        }
    }

    // Showinstance

    
    public async Task<List<Showinstance>> GetAllShowinstancesAsync() =>
        await _repository.GetAllShowinstancesAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) =>
        await _repository.GetShowinstanceByIdAsync(id);

    public async Task<Showinstance> AddShowinstanceAsync(ShowinstanceDto dto)
    {
        var showtemplate = await _context.Showtemplates
            .Include(st => st.Theater)
            .FirstOrDefaultAsync(st => st.Showtemplateid == dto.Showtemplateid);

        if (showtemplate == null)
            throw new Exception("Invalid Showtemplate ID");

        var showinstance = new Showinstance
        {
            Showtemplateid = dto.Showtemplateid,
            Showdate = dto.Showdate,
            Showtime = dto.Showtime,
            Availableseats = showtemplate.Theater.Capacity
        };

        _context.Showinstances.Add(showinstance);
        await _context.SaveChangesAsync();

        var theaterSeats = await _context.Seats
            .Where(s => s.Theaterid == showtemplate.Theaterid)
            .ToListAsync();

        var seatStatuses = theaterSeats.Select(seat => new Showseatstatus
        {
            Seatid = seat.Seatid,
            Isbooked = false,
            Showinstanceid = showinstance.Showinstanceid
        }).ToList();

        _context.Showseatstatuses.AddRange(seatStatuses);
        await _context.SaveChangesAsync();

        return showinstance;
    }
    public async Task<bool> UpdateShowinstanceAsync(int id, ShowinstanceDto dto)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        var showtemplate = await _context.Showtemplates
            .Include(st => st.Theater)
            .FirstOrDefaultAsync(st => st.Showtemplateid == dto.Showtemplateid);

        if (showtemplate == null)
            throw new Exception("Invalid Showtemplate ID");

        instance.Showtemplateid = dto.Showtemplateid;
        instance.Showdate = dto.Showdate;
        instance.Showtime = dto.Showtime;

        //  AvailableSeats from theater capacity
        instance.Availableseats = showtemplate.Theater.Capacity;

        await _repository.UpdateShowinstanceAsync(instance);
        return true;
    }


    public async Task<bool> DeleteShowinstanceAsync(int id)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        // Delete related seat statuses first
        var seatStatuses = await _context.Showseatstatuses
            .Where(s => s.Showinstanceid == id)
            .ToListAsync();

        _context.Showseatstatuses.RemoveRange(seatStatuses);

        await _repository.DeleteShowinstanceAsync(instance);
        await _context.SaveChangesAsync();

        return true;
    }

}
