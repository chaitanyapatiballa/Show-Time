using DBModels.Db;
using Microsoft.EntityFrameworkCore;

namespace TheaterService.Repositories
{
    public class TheaterRepository
    {
        private readonly AppDbContext _context;

        public TheaterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Theater>> GetTheatersAsync()
        {
            return await _context.Theaters.AsNoTracking().ToListAsync();
        }

        public async Task<Theater?> GetTheaterByIdAsync(int id)
        {
            return await _context.Theaters.FindAsync(id);
        }

        public async Task<Theater> AddTheaterAsync(Theater theater)
        {
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();
            return theater;
        }

        public async Task<Theater?> UpdateTheaterAsync(Theater updated)
        {
            var existing = await _context.Theaters.FindAsync(updated.Id);
            if (existing == null) return null;

            existing.Name = updated.Name;
            existing.Location = updated.Location;
            existing.Capacity = updated.Capacity;
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteTheaterAsync(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null) return false;

            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
