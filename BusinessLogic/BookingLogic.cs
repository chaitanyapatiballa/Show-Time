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

        public async Task<bool> BookSeatAsync(BookingDto dto)
        {
            var seatStatus = await _repository.GetShowseatstatusAsync(dto.Showinstanceid, dto.Seatid);
            if (seatStatus == null || seatStatus.Isbooked)
                return false;

            seatStatus.Isbooked = true;

            var showInstance = await _repository.GetShowinstanceByIdAsync(dto.Showinstanceid);
            if (showInstance != null && showInstance.Availableseats > 0)
            {
                showInstance.Availableseats -= 1;
            }

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
