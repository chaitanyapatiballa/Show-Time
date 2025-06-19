using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class MovieRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<List<Movie>> GetAllAsync() =>
        await _context.Movies.ToListAsync();

    public async Task<Movie?> GetByIdAsync(int id) =>
        await _context.Movies.FindAsync(id);

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

    public async Task<List<Showinstance>> GetShowinstancesForMovieAsync(int movieId)
    {
        return await _context.Showinstances
            .Include(si => si.Showtemplate)
            .ThenInclude(st => st.Theater)
            .Where(si => si.Showtemplate.Movieid == movieId && si.Showdate >= DateOnly.FromDateTime(DateTime.Now))
            .ToListAsync();
    }

    public async Task<List<Theater>> GetTheatersForMovieAsync(int movieId)
    {
        return await _context.Movietheater
            .Where(mt => mt.Movieid == movieId)
            .Select(mt => mt.Theater)
            .Distinct()
            .ToListAsync();
    }
}
