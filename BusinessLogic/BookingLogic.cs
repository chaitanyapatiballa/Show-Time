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
            var showInstance = await _repository.GetShowinstanceByIdAsync(dto.Showinstanceid);
            if (showInstance != null && showInstance.Availableseats > 0)
            {
                showInstance.Availableseats -= 1;
            }

           
            await _repository.SaveBookingAsync(dto.Showinstanceid, dto.Seatid, userId);

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
