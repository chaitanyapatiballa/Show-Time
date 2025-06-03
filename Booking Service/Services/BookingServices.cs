using Booking_Service.Models;
using Booking_Service.Repositories;

namespace Booking_Service.Services
{
    public class BookingServices
    {
        private readonly BookingRepository _bookingRepository;
        public BookingServices(BookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingsId(id);
        }
        public async Task<Booking> AddBooking (Booking booking)
        {
            return await _bookingRepository.AddBooking(booking);
        }
        public async Task CancelBookingAsync(int id)
        {
            await _bookingRepository.CancelBookingAsync(id);
        }
    }
}
