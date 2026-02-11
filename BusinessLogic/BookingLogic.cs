using Booking_Service.Controllers;
using DataAccessLayer.Repositories;
using DBModels.Dto;
using PaymentService.Repositories;
using MessagingLibrary;

namespace BusinessLogic
{
    public class BookingLogic
    {
        private readonly BookingRepository _repository;
        private readonly BillingsummaryRepository _summaryRepo;
        private readonly PaymentRepository _paymentRepo;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRabbitMQPublisher _publisher;

        public BookingLogic(
            BookingRepository bookingRepo,
            BillingsummaryRepository summaryRepo,
            PaymentRepository paymentRepo,
            IHttpClientFactory httpClientFactory,
            IRabbitMQPublisher publisher
        )
        {
            _repository = bookingRepo;
            _summaryRepo = summaryRepo;
            _paymentRepo = paymentRepo;
            _httpClientFactory = httpClientFactory;
            _publisher = publisher;
        }

        public async Task<(bool Success, decimal Amount, string? ErrorMessage)> BookSeatAsync(BookingDto dto, int userId)
        {
            try
            {
                var seatStatus = await _repository.GetShowseatstatusAsync(dto.Showinstanceid, dto.Seatid);
                if (seatStatus == null || seatStatus.Isbooked)
                    return (false, 0, "Seat already booked or not found");

                // Check lock
                if (seatStatus.LockedAt.HasValue && seatStatus.LockedAt.Value.AddMinutes(10) > DateTime.UtcNow)
                {
                     // If locked by someone else
                     if (seatStatus.LockedBy != userId.ToString())
                         return (false, 0, "Seat is locked by another user");
                }

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

                // Publish Event
                string message = $"Booking Created: Show {dto.Showinstanceid}, Seat {dto.Seatid}, User {userId}, Amount {seatPrice}";
                _publisher.Publish("booking-queue", message);

                return (true, seatPrice, null);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                return (false, 0, "The seat was modified by another user. Please try again.");
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
