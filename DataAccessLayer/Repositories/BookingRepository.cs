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

        public async Task<Booking?> AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<IEnumerable<string>> GetBookedSeats(int movieId, int theaterId, DateTime showTime)
        {
            // Ensure showTime is UTC to match PostgreSQL expectation
            var showTimeUtc = DateTime.SpecifyKind(showTime, DateTimeKind.Utc);

            return await _context.Bookings
                .Where(b => b.MovieId == movieId
                         && b.TheaterId == theaterId
                         && b.ShowTime == showTimeUtc
                         && !b.IsCancelled)
                .Select(b => b.SeatNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingHistoryByUserId(int userId, DateTime? fromDate, DateTime? toDate, string? status)
        {
            var query = _context.Bookings.AsQueryable();

            query = query.Where(b => b.UserId == userId);

            if (fromDate.HasValue)
                query = query.Where(b => b.BookingTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(b => b.BookingTime <= toDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.Status.ToLower() == status.ToLower());

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByMovieOrTheater(int? movieId, int? theaterId)
        {
            var query = _context.Bookings.AsQueryable();

            if (movieId.HasValue)
                query = query.Where(b => b.MovieId == movieId.Value);

            if (theaterId.HasValue)
                query = query.Where(b => b.TheaterId == theaterId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}