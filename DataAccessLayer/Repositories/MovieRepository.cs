using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class MovieRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<List<Movie>> GetAllAsync() => await _context.Movies.ToListAsync();

    public async Task<Movie?> GetByIdAsync(int id) => await _context.Movies.FindAsync(id);

    public async Task<Movie> AddMovieAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }


    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync()
    {
        return await _context.Showtemplates.ToListAsync();
    }

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id)
    {
        return await _context.Showtemplates.FindAsync(id);
    }

    public async Task AddShowtemplateAsync(Showtemplate template)
    {

        bool exists = await _context.MovieTheaters.AnyAsync(mt =>
            mt.movieid == template.Movieid && mt.theaterid == template.Theaterid);

        if (!exists)
        {
            _context.MovieTheaters.Add(new movietheater
            {
                movieid = template.Movieid,
                theaterid = template.Theaterid
            });
        }

        _context.Showtemplates.Add(template);
        await _context.SaveChangesAsync();
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


    public async Task<List<Showinstance>> GetAllShowinstancesAsync() =>
     await _context.Showinstances.ToListAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) =>
        await _context.Showinstances.FindAsync(id);

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

}