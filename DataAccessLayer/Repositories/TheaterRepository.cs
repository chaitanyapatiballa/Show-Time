using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories; 

public class TheaterRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<List<Theater>> GetAllAsync()
    {
        return await _context.Theaters.Include(t => t.Movies).ToListAsync();
    }

    public async Task<Theater?> GetByIdAsync(int id)
    {
        return await _context.Theaters.Include(t => t.Movies)
            .FirstOrDefaultAsync(t => t.Theaterid == id);
    }

    public async Task AddAsync(Theater theater, List<int>? movieIds)
    {
        if (movieIds != null && movieIds.Count > 0)
        {
            var movies = await _context.Movies.Where(m => movieIds.Contains(m.Movieid)).ToListAsync();
            theater.Movies = movies;
        }
        _context.Theaters.Add(theater);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theater theater, List<int>? movieIds)
    {
        var existing = await _context.Theaters.Include(t => t.Movies)
            .FirstOrDefaultAsync(t => t.Theaterid == theater.Theaterid);

        if (existing == null) return;

        existing.Name = theater.Name;
        existing.Location = theater.Location;

        if (movieIds != null)
        {
            var movies = await _context.Movies.Where(m => movieIds.Contains(m.Movieid)).ToListAsync();
            existing.Movies = movies;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Theater theater)
    {
        _context.Theaters.Remove(theater);
        await _context.SaveChangesAsync();
    }
}