using Booking_Service.Data;
using Booking_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_Service.Repositories
{
    public class BookingRepository
    {
        private readonly BookingDbContext _Context;
        public BookingRepository(BookingDbContext context)
        {
            _Context = context;
        }

        public async Task<Booking?> GetBookingsId(int id)
        {
            var booking = await _Context.Bookings.FindAsync(id);
            return booking; 
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            _Context.Bookings.Add(booking);
            await _Context.SaveChangesAsync();
            return booking;
        }

        public async Task CancelBookingAsync(int id)
        {
            var booking = await _Context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.IsCancelled = true;
                await _Context.SaveChangesAsync();
            }
        }
    }
}
