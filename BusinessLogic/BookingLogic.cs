using Booking_Service.Controllers;
using DataAccessLayer.Repositories;
using DBModels.Dto;
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

        public async Task<(bool Success, decimal Amount, string? ErrorMessage)> BookSeatAsync(BookingDto dto, int userId)
        {
            try
            {
                var seatStatus = await _repository.GetShowseatstatusAsync(dto.Showinstanceid, dto.Seatid);
                if (seatStatus == null || seatStatus.Isbooked)
                    return (false, 0, "Seat already booked or not found");

                seatStatus.Isbooked = true;
                var showinstance = await _repository.GetShowinstanceByIdAsync(dto.Showinstanceid);
                if (showinstance == null || showinstance.Availableseats <= 0)
                    return (false, 0, "No seats available");

                showinstance.Availableseats -= 1;

                if (!showinstance.Showdate.HasValue || !showinstance.Showtime.HasValue)
                    return (false, 0, "Invalid showtime");

                var combinedShowtime = new DateTime(
                    showinstance.Showdate.Value.Year,
                    showinstance.Showdate.Value.Month,
                    showinstance.Showdate.Value.Day,
                    showinstance.Showtime.Value.Hour,
                    showinstance.Showtime.Value.Minute,
                    showinstance.Showtime.Value.Second
                );
                combinedShowtime = DateTime.SpecifyKind(combinedShowtime, DateTimeKind.Unspecified);

                decimal seatPrice = await _repository.GetSeatPriceAsync(dto.Seatid);

                await _repository.SaveBookingAsync(dto.Showinstanceid, dto.Seatid, userId, combinedShowtime, seatPrice);
                await _repository.SaveChangesAsync();

                return (true, seatPrice, null);
            }
            catch (Exception ex)
            {
                
                return (false, 0, $"Error occurred: {ex.Message}");
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            try
            {
                return await _repository.CancelBookingAsync(bookingId);
            }
            catch (Exception)
            {
                
                return false;
            }
        }

        public async Task<List<ShowinstanceDto>> GetShowsAsync(int movieId, int theaterId, DateOnly date)
        {
            try
            {
                return await _repository.GetShowsByMovieTheaterAndDateAsync(movieId, theaterId, date);
            }
            catch (Exception)
            {
                
                return new List<ShowinstanceDto>();
            }
        }

        public async Task<List<SeatStatusDto>> GetAvailableSeatsAsync(int showinstanceId)
        {
            try
            {
                return await _repository.GetAvailableSeatsForShowinstanceAsync(showinstanceId);
            }
            catch (Exception)
            {
                
                return new List<SeatStatusDto>();
            }
        }

        public async Task GenerateNextDayShowinstancesAsync()
        {
            try
            {
                await _repository.DuplicateTodayShowinstancesForTomorrow();
            }
            catch (Exception)
            {
                
            }
        }
    }
}
