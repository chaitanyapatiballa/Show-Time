using DBModels.Db;
using DBModels.Models;

namespace Booking_Service.Repositories
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetBookingsId(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                // Optionally log or throw here
                return;
            }

            if (!booking.IsCancelled)
            {
                booking.IsCancelled = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
