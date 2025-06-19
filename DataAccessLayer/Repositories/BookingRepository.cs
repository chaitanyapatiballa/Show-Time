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

        public async Task SaveBookingAsync(int showinstanceid, int seatid, int userid, DateTime showtime)
        {
            var seat = await _context.Seats.FindAsync(seatid);
            var showinstance = await _context.Showinstances.FindAsync(showinstanceid);
            var showtemplate = await _context.Showtemplates.FindAsync(showinstance?.Showtemplateid);

            var booking = new Booking
            {
                Showinstanceid = showinstanceid,
                Seatid = seatid,
                Userid = userid,
                Showtime = showtime,
                Bookingtime = DateTime.UtcNow,
                Seatnumber = seat?.Number.ToString(),
                Movieid = showtemplate?.Movieid,
                Theaterid = showtemplate?.Theaterid,
                Status = "Booked"
            };

            _context.Bookings.Add(booking);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
