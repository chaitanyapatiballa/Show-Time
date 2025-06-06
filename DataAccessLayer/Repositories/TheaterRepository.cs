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
            return await _context.Theaters.ToListAsync();
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
        public async Task<Theater?> UpdateTheaterAsync(Theater theater)
        {
            var existingTheater = await _context.Theaters.FindAsync(theater.Id);
            if (existingTheater == null)
            {
                return null;
            }
            existingTheater.Name = theater.Name;
            existingTheater.Location = theater.Location;
            existingTheater.Capacity = theater.Capacity;
            await _context.SaveChangesAsync();
            return existingTheater;
        }
        public async Task<bool> DeleteTheaterAsync(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
            {
                return false;
            }
            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
