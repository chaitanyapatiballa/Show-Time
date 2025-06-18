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

        public Task SaveBookingAsync(int showinstanceid, int seatid, int userid)
        {
            var booking = new Booking
            {
                Showinstanceid = showinstanceid,
                Seatid = seatid,
                Userid = userid,
                Bookingtime = DateTime.UtcNow
            };
            _context.Bookings.Add(booking);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
