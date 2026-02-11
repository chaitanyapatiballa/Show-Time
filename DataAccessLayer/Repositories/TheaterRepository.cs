using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TheaterRepository : ITheaterRepository
{
    private readonly AppDbContext _context;

    public TheaterRepository(AppDbContext context)
    {
        _context = context;
    }

    // --- Theater Operations ---
    public async Task<List<Theater>> GetAllAsync() => 
        await _context.Theaters
            .Include(t => t.Movietheater)
            .ThenInclude(mt => mt.Movie)
            .ToListAsync();

    public async Task<Theater?> GetByIdAsync(int id) => 
        await _context.Theaters
            .Include(t => t.Movietheater)
            .ThenInclude(mt => mt.Movie)
            .FirstOrDefaultAsync(t => t.Theaterid == id);

    public async Task AddAsync(Theater theater)
    {
        _context.Theaters.Add(theater);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theater theater)
    {
        _context.Theaters.Update(theater);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Theater theater)
    {
        _context.Theaters.Remove(theater);
        await _context.SaveChangesAsync();
    }

    // --- Movie-Theater Links ---
    public async Task UpdateMovieLinksAsync(int theaterId, List<int> movieIds)
    {
        var existingLinks = _context.Movietheater.Where(mt => mt.Theaterid == theaterId);
        _context.Movietheater.RemoveRange(existingLinks);

        if (movieIds != null && movieIds.Any())
        {
            var movies = await _context.Movies.Where(m => movieIds.Contains(m.Movieid)).ToListAsync();
            var newLinks = movies.Select(m => new Movietheater { Movieid = m.Movieid, Theaterid = theaterId });
            _context.Movietheater.AddRange(newLinks);
        }
        
        await _context.SaveChangesAsync();
    }

    // --- Seat Operations ---
    public async Task AddSeatsAsync(IEnumerable<Seat> seats)
    {
        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Seat>> GetSeatsByTheaterIdAsync(int theaterId)
    {
        return await _context.Seats.Where(s => s.Theaterid == theaterId).ToListAsync();
    }

    // --- ShowTemplate Operations ---
    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() => 
        await _context.Showtemplates.ToListAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) => 
        await _context.Showtemplates.FindAsync(id);

    public async Task<Showtemplate?> GetShowtemplateWithTheaterAsync(int id)
    {
         return await _context.Showtemplates
            .Include(st => st.Theater)
            .FirstOrDefaultAsync(st => st.Showtemplateid == id);
    }

    public async Task<Showtemplate> AddShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Remove(template);
        await _context.SaveChangesAsync();
    }

    // --- ShowInstance Operations ---
    public async Task<List<Showinstance>> GetAllShowinstancesAsync() => 
        await _context.Showinstances.ToListAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) => 
        await _context.Showinstances.FindAsync(id);

    public async Task<List<Showinstance>> GetShowinstancesByMovieAsync(int movieId)
    {
        return await _context.Showinstances
            .Include(si => si.Showtemplate)
            .ThenInclude(showtemplate => showtemplate!.Theater) 
            .Where(si => si.Showtemplate != null && si.Showtemplate.Movieid == movieId && si.Showdate >= DateOnly.FromDateTime(DateTime.Today))
            .ToListAsync();
    }

    public async Task AddShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Add(instance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Update(instance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Remove(instance);
        await _context.SaveChangesAsync();
    }

    // --- Seat Status Operations ---
    public async Task<List<Showseatstatus>> GetSeatStatusesByShowInstanceIdAsync(int showInstanceId)
    {
        return await _context.Showseatstatuses
            .Include(s => s.Seat)
            .Where(s => s.Showinstanceid == showInstanceId)
            .ToListAsync();
    }

    public async Task AddShowseatstatusesAsync(IEnumerable<Showseatstatus> statuses)
    {
        _context.Showseatstatuses.AddRange(statuses);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveShowseatstatusesAsync(IEnumerable<Showseatstatus> statuses)
    {
        _context.Showseatstatuses.RemoveRange(statuses);
        await _context.SaveChangesAsync();
    }

    // --- Helpers ---
    public async Task<bool> MovieTheaterLinkExistsAsync(int movieId, int theaterId)
    {
        return await _context.Movietheater.AnyAsync(mt => mt.Movieid == movieId && mt.Theaterid == theaterId);
    }

    public async Task AddMovieTheaterLinkAsync(int movieId, int theaterId)
    {
        _context.Movietheater.Add(new Movietheater { Movieid = movieId, Theaterid = theaterId });
        await _context.SaveChangesAsync();
    }
}
