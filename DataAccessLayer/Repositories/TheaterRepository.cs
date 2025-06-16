using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TheaterRepository
{
    private readonly AppDbContext _context;

    public TheaterRepository(AppDbContext context)
    {
        _context = context;
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
}
