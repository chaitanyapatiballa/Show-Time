using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Showseatstatus?> GetShowseatstatusAsync(int showinstanceId, int seatId)
        {
            return await _context.Showseatstatuses
                .FirstOrDefaultAsync(s => s.Showinstanceid == showinstanceId && s.Seatid == seatId);
        }

        public async Task<Showinstance?> GetShowinstanceByIdAsync(int id)
        {
            return await _context.Showinstances.FindAsync(id);
        }

        public async Task<decimal> GetSeatPriceAsync(int seatId)
        {
            var seat = await _context.Seats.FindAsync(seatId);
            return seat?.Baseprice ?? 100;
        }

        public async Task SaveBookingAsync(int showinstanceid, int seatid, int userid, DateTime showtime, decimal price)
        {
            var seat = await _context.Seats.FindAsync(seatid);
            var showinstance = await _context.Showinstances.FindAsync(showinstanceid);
            var showtemplate = await _context.Showtemplates.FindAsync(showinstance?.Showtemplateid);

            var booking = new Booking
            {
                Showinstanceid = showinstanceid,
                Seatid = seatid,
                Userid = userid,
                Showtime = DateTime.SpecifyKind(showtime, DateTimeKind.Unspecified),
                Bookingtime = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                Seatnumber = seat?.Number.ToString(),
                Movieid = showtemplate?.Movieid,
                Theaterid = showtemplate?.Theaterid,
                Status = "Booked",
                Baseprice = price
            };

            _context.Bookings.Add(booking);
        }


        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Status == "Cancelled") return false;

            booking.Status = "Cancelled";

            var seatStatus = await _context.Showseatstatuses.FirstOrDefaultAsync(s =>
                s.Showinstanceid == booking.Showinstanceid && s.Seatid == booking.Seatid);

            if (seatStatus != null) seatStatus.Isbooked = false;

            var instance = await _context.Showinstances.FindAsync(booking.Showinstanceid);
            if (instance != null) instance.Availableseats += 1;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShowinstanceDto>> GetShowsByMovieTheaterAndDateAsync(int movieId, int theaterId, DateOnly date)
        {
            return await _context.Showinstances
                .Where(s =>
                    s.Showtemplate.Movieid == movieId &&
                    s.Showtemplate.Theaterid == theaterId &&
                    s.Showdate == date)
                .Select(s => new ShowinstanceDto
                {
                    Showinstanceid = s.Showinstanceid,
                    Showdate = (DateOnly)s.Showdate,
                    Showtime = (TimeOnly)s.Showtime,
                    Availableseats = (int)s.Availableseats
                })
                .OrderBy(s => s.Showtime)
                .ToListAsync();
        }

        public async Task<List<SeatStatusDto>> GetAvailableSeatsForShowinstanceAsync(int showinstanceId)
        {
            return await _context.Showseatstatuses
                .Where(s => s.Showinstanceid == showinstanceId)
                .Include(s => s.Seat)
                .Select(s => new SeatStatusDto
                {
                    Seatid = s.Seatid,
                    Row = s.Seat.Row,
                    Number = s.Seat.Number,
                    Isbooked = s.Isbooked,
                    Price = s.Seat.Baseprice
                })
                .OrderBy(s => s.Row).ThenBy(s => s.Number)
                .ToListAsync();
        }

        public async Task DuplicateTodayShowinstancesForTomorrow()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var tomorrow = today.AddDays(1);

            var todayInstances = await _context.Showinstances
                .Where(si => si.Showdate == today)
                .ToListAsync();

            foreach (var instance in todayInstances)
            {
                var newInstance = new Showinstance
                {
                    Showtemplateid = instance.Showtemplateid,
                    Showdate = tomorrow,
                    Showtime = instance.Showtime,
                    Availableseats = instance.Availableseats
                };

                _context.Showinstances.Add(newInstance);
                await _context.SaveChangesAsync();

                var theaterId = _context.Showtemplates.First(st => st.Showtemplateid == newInstance.Showtemplateid).Theaterid;
                var seats = await _context.Seats.Where(s => s.Theaterid == theaterId).ToListAsync();

                var statuses = seats.Select(seat => new Showseatstatus
                {
                    Showinstanceid = newInstance.Showinstanceid,
                    Seatid = seat.Seatid,
                    Isbooked = false
                });

                _context.Showseatstatuses.AddRange(statuses);
                await _context.SaveChangesAsync();
            }
        }
    }
}