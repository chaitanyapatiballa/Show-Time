using BookingService.Repositories;
using DBModels.Db;
using MovieService.Models;
using PaymentService.DTOs;
using System.Text.Json;
using TheaterService.DTOs;

namespace BookingService.Services
{
    public class BookingServices
    {
        private readonly BookingRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingServices(BookingRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Booking?> CreateBooking(Booking booking)
        {
            return await _repository.AddBooking(booking);
        }

        public async Task<object?>  GetBookingDetails(int bookingId)
        {
            var booking = await _repository.GetBookingById(bookingId);
            if (booking == null) return null;

            var movieClient = _httpClientFactory.CreateClient("MovieService");
            var theaterClient = _httpClientFactory.CreateClient("TheaterService");
            var paymentClient = _httpClientFactory.CreateClient("PaymentService");

            var movie = await GetFromService<MovieDto>(movieClient, $"/api/Movies/GetMovie/{booking.MovieId}");
            var theater = await GetFromService<TheaterDto>(theaterClient, $"/api/Theaters/GetTheater/{booking.TheaterId}");
            var payment = await GetFromService<PaymentDto>(paymentClient, $"/api/Payments/GetPaymentByBooking/{booking.Id}");

            return new
            {
                Booking = booking,
                Movie = movie,
                Theater = theater,
                Payment = payment
            };
        }

        private async Task<T?> GetFromService <T>(HttpClient client, string path)
        {
            var response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode) return default;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
