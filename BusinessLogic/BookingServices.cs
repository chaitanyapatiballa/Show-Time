using Booking_Service.DTOs;
using Booking_Service.HttpClients;
using DBModels.Db;
using Microsoft.EntityFrameworkCore;

namespace Booking_Service.Services
{
    public class BookingServices
    {
        private readonly AppDbContext _context;
        private readonly IMovieServiceClient _movieService;
        private readonly ITheaterServiceClient _theaterService;
        private readonly IPaymentServiceClient _paymentService;

        public BookingServices(
            AppDbContext context,
            IMovieServiceClient movieService,
            ITheaterServiceClient theaterService,
            IPaymentServiceClient paymentService)
        {
            _context = context;
            _movieService = movieService;
            _theaterService = theaterService;
            _paymentService = paymentService;
        }

        public async Task<Booking> AddBooking(BookingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Validate Movie and Theater exist via HTTP call
            var movie = await _movieService.GetMovieAsync(dto.MovieId);
            var theater = await _theaterService.GetTheaterAsync(dto.TheaterId);

            if (movie == null)
                throw new Exception("Invalid Movie ID.");
            if (theater == null)
                throw new Exception("Invalid Theater ID.");

            // Step 1: Save booking without paymentId
            var booking = new Booking
            {
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId,
                SeatNumber = dto.SeatNumber,
                UserId = dto.UserId,
                BookingTime = DateTime.UtcNow,
                IsCancelled = false
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(); // Booking ID is generated

            // Step 2: Create Payment and update Booking
            var paymentId = await _paymentService.CreatePaymentAsync(booking.Id);
            booking.PaymentId = paymentId;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Movie)
                .Include(b => b.Theater)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null && !booking.IsCancelled)
            {
                booking.IsCancelled = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
