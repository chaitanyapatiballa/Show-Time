using DBModels.Models;
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
            var showTimeUtc = DateTime.SpecifyKind(showTime, DateTimeKind.Utc);

            return await _context.Bookings
                .Where(b => b.Movieid == movieId
                         && b.Theaterid == theaterId
                         && b.Showtime == showTimeUtc
                         && !b.IsCancelled)
                .Select(b => b.Seatnumber!)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingHistoryByUserId(int userId, DateTime? fromDate, DateTime? toDate, string? status)
        {
            var query = _context.Bookings.AsQueryable();

            query = query.Where(b => b.Userid == userId);

            if (fromDate.HasValue)
                query = query.Where(b => b.Bookingtime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(b => b.Bookingtime <= toDate.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(b => b.Status!.ToLower() == status.ToLower());

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByMovieOrTheater(int? movieId, int? theaterId)
        {
            var query = _context.Bookings.AsQueryable();

            if (movieId.HasValue)
                query = query.Where(b => b.Movieid == movieId.Value);

            if (theaterId.HasValue)
                query = query.Where(b => b.Theaterid == theaterId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _context.Bookings
                .Where(b => b.Userid == userId)
                .ToListAsync();
        }

        public async Task SaveBillingSummary(Billingsummary summary)
        {
            _context.BillingSummaries.Add(summary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
