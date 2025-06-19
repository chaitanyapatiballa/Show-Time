using Booking_Service.Controllers;
using DataAccessLayer.Repositories;
using PaymentService.Repositories;


namespace BusinessLogic
{
    public class BookingLogic
    (
        BookingRepository bookingRepo,
        BillingsummaryRepository summaryRepo,
        PaymentRepository paymentRepo,
        IHttpClientFactory httpClientFactory
    )
    {
        private readonly BookingRepository _repository = bookingRepo;
        private readonly BillingsummaryRepository _summaryRepo = summaryRepo;
        private readonly PaymentRepository _paymentRepo = paymentRepo;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;


        public async Task<bool> BookSeatAsync(BookingDto dto, int userId)
        {
            var seatStatus = await _repository.GetShowseatstatusAsync(dto.Showinstanceid, dto.Seatid);
            if (seatStatus == null || seatStatus.Isbooked)
                return false;

            // Mark seat as booked
            seatStatus.Isbooked = true;

            // Reduce available seats
            var showinstance = await _repository.GetShowinstanceByIdAsync(dto.Showinstanceid);
            if (showinstance == null || showinstance.Availableseats <= 0)
                return false;

            showinstance.Availableseats -= 1;

            // Combine Showdate (DateOnly?) + Showtime (TimeOnly?) → DateTime
            if (!showinstance.Showdate.HasValue || !showinstance.Showtime.HasValue)
                return false;

            var showdate = showinstance.Showdate.Value; //nullable DateOnly
            var showtime = showinstance.Showtime.Value; //nullable TimeOnly

            var combinedShowtime = new DateTime(
                showdate.Year,
                showdate.Month,
                showdate.Day,
                showtime.Hour,
                showtime.Minute,
                showtime.Second
            );

            await _repository.SaveBookingAsync(dto.Showinstanceid, dto.Seatid, userId, combinedShowtime);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
