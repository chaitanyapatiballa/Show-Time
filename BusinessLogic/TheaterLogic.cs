using DataAccessLayer.Repositories;
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
}
