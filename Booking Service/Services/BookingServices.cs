using Booking_Service.DTOs;
using Booking_Service.Models;
using Booking_Service.Repositories;
using System.Net.Http.Json;

namespace Booking_Service.Services
{
    public class BookingServices
    {
        private readonly BookingRepository _bookingRepository;
        private readonly HttpClient _httpClient;

        public BookingServices(BookingRepository bookingRepository, HttpClient httpClient)
        {
            _bookingRepository = bookingRepository;
            _httpClient = httpClient;
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingsId(id);
        }

        public async Task<Booking> AddBooking(Booking booking)
        {
            // Validate with MovieService
            var movie = await GetMovieAsync(booking.MovieId);
            if (movie == null)
                throw new Exception("Movie not found");

            // Validate with TheaterService
            var theater = await GetTheaterAsync(booking.TheaterId);
            if (theater == null)
                throw new Exception("Theater not found");

            // Save Booking to DB
            var createdBooking = await _bookingRepository.AddBooking(booking);

            // Send payment to PaymentService
            bool paymentSuccess = await MakePaymentAsync(booking.UserId.ToString(), createdBooking.Id, 550); 

            if (!paymentSuccess)
                throw new Exception("Payment failed");

            return createdBooking;
        }

        public async Task CancelBookingAsync(int id)
        {
            await _bookingRepository.CancelBookingAsync(id);
        }

        // Call to MovieService
        private async Task<object?> GetMovieAsync(int movieId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5000/api/movies/{movieId}");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<object>() : null;
        }

        // Call to TheaterService
        private async Task<object?> GetTheaterAsync(int theaterId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5101/api/Theaters/GetTheater/{theaterId}");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<object>() : null;
        }

        // Call to PaymentService
        private async Task<bool> MakePaymentAsync(string userId, int bookingId, decimal amount)
        {
            var payment = new PaymentDto
            {
                UserId = userId,
                BookingId = bookingId,
                Amount = amount,
                PaymentTime = DateTime.UtcNow,
                IsSuccessful = true
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5102/api/Payment", payment);
            return response.IsSuccessStatusCode;
        }
    }
}
