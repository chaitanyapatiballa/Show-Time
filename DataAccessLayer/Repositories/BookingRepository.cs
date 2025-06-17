// File: DataAccessLayer/Repositories/BookingRepository.cs
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BookingRepository(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<Showseatstatus?> GetShowseatstatusAsync(int showinstanceId, int seatId)
        {
            return await _context.Showseatstatuses
                .FirstOrDefaultAsync(s => s.Showinstanceid == showinstanceId && s.Seatid == seatId);
        }

        public async Task<Showinstance?> GetShowinstanceByIdAsync(int id)
        {
            return await _context.Showinstances.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
