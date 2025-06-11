using Booking_Service.Repositories;
using DBModels.Db;
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
            var movie = await GetMovieAsync(booking.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found");

            var theater = await GetTheaterAsync(booking.TheaterId);
            if (theater == null)
                throw new InvalidOperationException("Theater not found");

            var savedBooking = await _bookingRepository.AddBooking(booking);

            var payment = new PaymentDto
            {
                UserId = booking.UserId.ToString(),
                BookingId = savedBooking.Id,
                Amount = 550,
                PaymentTime = DateTime.UtcNow,
                IsSuccessful = true
            };

            var paymentResponse = await _httpClient.PostAsJsonAsync("http://localhost:5102/api/Payment", payment);

            if (!paymentResponse.IsSuccessStatusCode)
                throw new InvalidOperationException("Payment failed");

            var paymentData = await paymentResponse.Content.ReadFromJsonAsync<PaymentDto>();

            if (paymentData == null)
                throw new InvalidOperationException("Payment response invalid");

            savedBooking.PaymentId = paymentData.PaymentId;
            savedBooking.Status = "Confirmed";

            await _bookingRepository.UpdateBookingAsync(savedBooking);

            return savedBooking;
        }

        public async Task CancelBookingAsync(int id)
        {
            await _bookingRepository.CancelBookingAsync(id);
        }

        public async Task<object?> GetBookingDetailsAsync(int id)
        {
            var booking = await _bookingRepository.GetBookingsId(id);
            if (booking == null) return null;

            var movie = await GetMovieAsync(booking.MovieId);
            var theater = await GetTheaterAsync(booking.TheaterId);
            var payment = await GetPaymentAsync(booking.PaymentId);

            return new
            {
                BookingId = booking.Id,
                booking.UserId,
                booking.SeatNumber,
                booking.BookingTime,
                booking.IsCancelled,
                booking.Status,
                Movie = movie,
                Theater = theater,
                Payment = payment
            };
        }

        private async Task<object?> GetMovieAsync(int movieId)
        {
            var url = $"http://localhost:7105/api/Movies/GetMovie/{movieId}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch { }
            return null;
        }

        private async Task<object?> GetTheaterAsync(int theaterId)
        {
            var url = $"http://localhost:7106/api/Theaters/GetTheater/{theaterId}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch { }
            return null;
        }

        private async Task<object?> GetPaymentAsync(int paymentId)
        {
            var url = $"http://localhost:7107/api/Payment/{paymentId}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<object>();
            }
            catch { }
            return null;
        }
    }
}
