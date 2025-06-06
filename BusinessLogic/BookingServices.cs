using Booking_Service.Repositories;
using DBModels.Models;
using PaymentService.DTOs;
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
                throw new InvalidOperationException("Movie not found");

            // Validate with TheaterService
            var theater = await GetTheaterAsync(booking.TheaterId);
            if (theater == null)
                throw new InvalidOperationException("Theater not found");

            // Save booking to DB
            var BookingSuccessful = await _bookingRepository.AddBooking(booking);

            // Attempt payment via PaymentService
            var paymentSuccess = await MakePaymentAsync(booking.UserId.ToString(), BookingSuccessful.Id, 550); // Fixed amount

            if (!paymentSuccess)
                throw new InvalidOperationException("Payment failed");

            return BookingSuccessful;
        }

        public async Task CancelBookingAsync(int id)
        {
            await _bookingRepository.CancelBookingAsync(id);
        }

     

        private async Task<object?> GetMovieAsync(int movieId)
        {
            var url = $"http://localhost:5000/api/movies/{movieId}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch (Exception )
            {
               
            }
            return null;
        }

        private async Task<object?> GetTheaterAsync(int theaterId)
        {
            var url = $"http://localhost:5101/api/Theaters/GetTheater/{theaterId}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch (Exception )
            {
                
            }
            return null;
        }

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

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5102/api/Payment", payment);
                return response.IsSuccessStatusCode;
            }
            catch (Exception )
            {
                
                return false;
            }
        }
    }
}
