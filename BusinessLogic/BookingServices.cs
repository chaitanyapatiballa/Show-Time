using Booking_Service.Repositories;
using DBModels.Db;
using PaymentService.DTOs;
using System.Net.Http.Json;

namespace Booking_Service.Services
{
    public class BookingServices
    {
        private readonly BookingRepository _bookingRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingServices(BookingRepository bookingRepository, IHttpClientFactory httpClientFactory)
        {
            _bookingRepository = bookingRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingsId(id);
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            var movie = await GetMovieAsync(booking.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found");

            var theater = await GetTheaterAsync(booking.TheaterId);
            if (theater == null)
                throw new InvalidOperationException("Theater not found");

            // Save booking first with status Pending
            booking.Status = "Pending";
            var savedBooking = await _bookingRepository.AddBooking(booking);

            // Call payment service
            var paymentId = await MakePaymentAsync(booking.UserId.ToString(), savedBooking.Id, 550);
            if (paymentId == null)
                throw new InvalidOperationException("Payment failed");

            // Update Booking with PaymentId and status
            savedBooking.PaymentId = paymentId.Value;
            savedBooking.Status = "Confirmed";

            await _bookingRepository.UpdateBookingAsync(savedBooking);

            return savedBooking;
        }

        public async Task CancelBookingAsync(int id)
        {
            await _bookingRepository.CancelBookingAsync(id);
        }

        private async Task<object?> GetMovieAsync(int movieId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MovieService");
                var response = await client.GetAsync($"/api/Movies/{movieId}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling MovieService: {ex.Message}");
            }
            return null;
        }

        private async Task<object?> GetTheaterAsync(int theaterId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("TheaterService");
                var response = await client.GetAsync($"/api/Theaters/{theaterId}");
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling TheaterService: {ex.Message}");
            }
            return null;
        }

        private async Task<int?> MakePaymentAsync(string userId, int bookingId, decimal amount)
        {
            var client = _httpClientFactory.CreateClient("PaymentService");

            var payment = new PaymentDto
            {
                UserId = userId,
                BookingId = bookingId,
                Amount = amount,
                PaymentTime = DateTime.UtcNow,
                IsSuccessful = true
            };

            try
            {
                var response = await client.PostAsJsonAsync("/api/Payment", payment);
                if (response.IsSuccessStatusCode)
                {
                    var createdPayment = await response.Content.ReadFromJsonAsync<PaymentDto>();
                    return createdPayment?.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling PaymentService: {ex.Message}");
            }
            return null;
        }
    }
}
