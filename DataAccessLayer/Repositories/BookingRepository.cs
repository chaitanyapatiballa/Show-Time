using DBModels.Db;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();  
            return booking;
        }
    }
}
