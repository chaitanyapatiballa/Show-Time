using BookingService.DTOs;
using BookingService.Repositories;
using DBModels.Dto;
using DBModels.Models;
using MovieService.Models;
using PaymentService.DTOs;
using System.Net.Http.Json;
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

        public async Task<object?> GetBookingDetails(int bookingId)
        {
            var booking = await _repository.GetBookingById(bookingId);
            if (booking == null) return null;

            var movie = await GetFromService<MovieDto>("MovieService", $"/api/Movies/GetMovie/{booking.Movieid}");
            var theater = await GetFromService<TheaterDto>("TheaterService", $"/api/Theaters/GetTheater/{booking.Theaterid}");
            var payment = await GetFromService<PaymentDto>("PaymentService", $"/api/Payments/GetPaymentByBooking/{booking.Bookingid}");

            return new
            {
                Booking = booking,
                Movie = movie,
                Theater = theater,
                Payment = payment
            };
        }

        private async Task<T?> GetFromService<T>(string serviceName, string path)
        {
            var client = _httpClientFactory.CreateClient(serviceName);
            var response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<IEnumerable<string>> GetAvailableSeats(int movieId, int theaterId, DateTime showTime)
        {
            showTime = DateTime.SpecifyKind(showTime, DateTimeKind.Utc);
            var bookedSeats = await _repository.GetBookedSeats(movieId, theaterId, showTime);

            var theater = await GetFromService<TheaterDto>("TheaterService", $"/api/Theaters/GetTheater/{theaterId}");
            if (theater == null) throw new Exception("Theater not found");

            var allSeats = GenerateSeatLabels(theater.Capacity);
            return allSeats.Except(bookedSeats).ToList();
        }

        private List<string> GenerateSeatLabels(int totalSeats)
        {
            var seatLabels = new List<string>();
            int rows = (int)Math.Ceiling(totalSeats / 20.0);
            int seatsPerRow = 20;
            char rowChar = 'A';

            for (int r = 0; r < rows && rowChar <= 'Z'; r++, rowChar++)
            {
                for (int s = 1; s <= seatsPerRow && seatLabels.Count < totalSeats; s++)
                    seatLabels.Add($"{rowChar}{s}");
            }

            return seatLabels;
        }

        public async Task<List<object>> GetBookingHistoryByUserId(int userId, string? status)
        {
            var allBookings = await _repository.GetBookingsByUserId(userId);

            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower();
                allBookings = allBookings.Where(b => b.Status?.ToLower() == status).ToList();
            }

            var result = new List<object>();

            foreach (var booking in allBookings)
            {
                var movie = await GetFromService<MovieDto>("MovieService", $"/api/Movies/GetMovie/{booking.Movieid}");
                var theater = await GetFromService<TheaterDto>("TheaterService", $"/api/Theaters/GetTheater/{booking.Theaterid}");
                var payment = await GetFromService<PaymentDto>("PaymentService", $"/api/Payments/GetPaymentByBooking/{booking.Bookingid}");

                result.Add(new { Booking = booking, Movie = movie, Theater = theater, Payment = payment });
            }

            return result;
        }

        public HttpClient CreatePaymentServiceClient() => _httpClientFactory.CreateClient("PaymentService");

        public async Task UpdateBooking(Booking booking)
        {
            await _repository.UpdateBooking(booking);
        }

        public async Task<Booking?> CreateBookingWithBilling(BookingDto dto)
        {
            var booking = new Booking
            {
                Userid = dto.UserId,
                Movieid = dto.MovieId,
                Theaterid = dto.TheaterId,
                Seatnumber = dto.SeatNumber,
                Showtime = dto.ShowTime,
                Bookingtime = DateTime.UtcNow,
                IsCancelled = false,
                Status = "Confirmed"
            };

            var savedBooking = await _repository.AddBooking(booking);

            var showId = await GetShowInstanceId(dto.MovieId, dto.TheaterId, dto.ShowTime);
            if (showId == null) throw new Exception("Unable to fetch show instance.");

            var paymentClient = _httpClientFactory.CreateClient("PaymentService");

            var billingResponse = await paymentClient.PostAsync(
                $"/api/BillingSummary/generate?bookingId={savedBooking.Bookingid}&showId={showId}&paymentMethod={dto.PaymentMethod ?? "cash"}",
                null);

            if (!billingResponse.IsSuccessStatusCode)
                throw new Exception("Billing generation failed.");

            var billingData = await billingResponse.Content.ReadFromJsonAsync<Billingsummary>();
            if (billingData != null)
                await _repository.SaveBillingSummary(billingData);

            var paymentUrl = $"/api/Payment/make-payment?bookingId={savedBooking.Bookingid}&showId={showId}&paymentMethod={dto.PaymentMethod ?? "cash"}";
            var paymentResponse = await paymentClient.PostAsync(paymentUrl, null);

            if (!paymentResponse.IsSuccessStatusCode)
                throw new Exception("Payment failed: " + await paymentResponse.Content.ReadAsStringAsync());

            var payment = await paymentResponse.Content.ReadFromJsonAsync<PaymentDto>();
            if (payment == null || payment.PaymentId <= 0)
                throw new Exception("Invalid payment response");

            savedBooking.Paymentid = payment.PaymentId;
            await _repository.UpdateBooking(savedBooking);

            return savedBooking;
        }

        private async Task<int?> GetShowInstanceId(int movieId, int theaterId, DateTime showTime)
        {
            var showClient = _httpClientFactory.CreateClient("ShowService");
            var url = $"/api/Shows/instance?movieId={movieId}&theaterId={theaterId}&showTime={showTime.ToUniversalTime():O}";

            var response = await showClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var showInstance = System.Text.Json.JsonSerializer.Deserialize<ShowInstanceDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return showInstance?.ShowInstanceid;
        }
    }
}
