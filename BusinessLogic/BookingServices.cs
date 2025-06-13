using BookingService.DTOs;
using BookingService.Repositories;
using DBModels.Db;
using DBModels.Dto;
using MovieService.Models;
using Newtonsoft.Json;
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

            var movieClient = _httpClientFactory.CreateClient("MovieService");
            var theaterClient = _httpClientFactory.CreateClient("TheaterService");
            var paymentClient = _httpClientFactory.CreateClient("PaymentService");
            var showClient = _httpClientFactory.CreateClient("ShowService");

            var movie = await GetFromService<MovieDto>(movieClient, $"/api/Movies/GetMovie/{booking.MovieId}");
            var theater = await GetFromService<TheaterDto>(theaterClient, $"/api/Theaters/GetTheater/{booking.TheaterId}");
            var payment = await GetFromService<PaymentDto>(paymentClient, $"/api/Payments/GetPaymentByBooking/{booking.MovieId}");

            return new
            {
                Booking = booking,
                Movie = movie,
                Theater = theater,
                Payment = payment
            };
        }

        private async Task<T?> GetFromService<T>(HttpClient client, string path)
        {
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

            var theaterClient = _httpClientFactory.CreateClient("TheaterService");
            var theaterResponse = await theaterClient.GetAsync($"/api/Theaters/GetTheater/{theaterId}");


            if (!theaterResponse.IsSuccessStatusCode)
                throw new Exception("Failed to fetch theater info from TheaterService.");

            var theaterContent = await theaterResponse.Content.ReadAsStringAsync();
            var theater = JsonConvert.DeserializeObject<TheaterDto>(theaterContent);
            int capacity = theater.Capacity;

            var allSeats = GenerateSeatLabels(capacity);
            var availableSeats = allSeats.Except(bookedSeats).ToList();

            return availableSeats;
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
                {
                    seatLabels.Add($"{rowChar}{s}");
                }
            }

            return seatLabels;
        }

        public async Task<List<object>> GetBookingHistoryByUserId(int userId, string? status)
        {
            var allBookings = await _repository.GetBookingsByUserId(userId);

            //if (startDate.HasValue && endDate.HasValue)
            //{
            //    allBookings = allBookings
            //        .Where(b => b.BookingTime.Date >= startDate.Value.Date && b.BookingTime.Date <= endDate.Value.Date)
            //        .ToList();
            //}

            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower();
                if (status == "Confirmed")
                {
                    allBookings = allBookings.Where(b => b.Status.ToLower() == "Confirmed").ToList();
                }
                else if (status == "cancelled")
                {
                    allBookings = allBookings.Where(b => b.Status.ToLower() == "cancelled").ToList();
                }
            }

            var result = new List<object>();

            foreach (var booking in allBookings)
            {
                var movieClient = _httpClientFactory.CreateClient("MovieService");
                var theaterClient = _httpClientFactory.CreateClient("TheaterService");
                var paymentClient = _httpClientFactory.CreateClient("PaymentService");
                var showClient = _httpClientFactory.CreateClient("ShowService");

                var movie = await GetFromService<MovieDto>(movieClient, $"/api/Movies/GetMovie/{booking.MovieId}");
                var theater = await GetFromService<TheaterDto>(theaterClient, $"/api/Theaters/GetTheater/{booking.TheaterId}");
                var payment = await GetFromService<PaymentDto>(paymentClient, $"/api/Payments/GetPaymentByBooking/{booking.MovieId}");

                result.Add(new
                {
                    Booking = booking,
                    Movie = movie,
                    Theater = theater,
                    Payment = payment
                });
            }

            return result;
        }

        public HttpClient CreatePaymentServiceClient()
        {
            return _httpClientFactory.CreateClient("PaymentService");
        }

        public async Task UpdateBooking(Booking booking)
        {
            await _repository.UpdateBooking(booking);
        }

        public async Task<Booking?> CreateBookingWithBilling(BookingDto dto)
        {
            var booking = new Booking
            {
                UserId = dto.UserId,
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId,
                SeatNumber = dto.SeatNumber,
                ShowTime = dto.ShowTime,
                BookingTime = DateTime.UtcNow,
                IsCancelled = false,
                Status = "Confirmed"
            };

            var savedBooking = await _repository.AddBooking(booking);

            //  Get ShowInstanceId dynamically
            var showId = await GetShowInstanceId(dto.MovieId, dto.TheaterId, dto.ShowTime);
            if (showId == null) throw new Exception("Unable to fetch show instance.");

            var paymentClient = _httpClientFactory.CreateClient("PaymentService");

            //  Generate Billing
            var billingResponse = await paymentClient.PostAsync(
                $"/api/BillingSummary/generate?bookingId={savedBooking.BookingId}&showId={showId}&paymentMethod={dto.PaymentMethod ?? "cash"}",
                null);

            if (!billingResponse.IsSuccessStatusCode)
                throw new Exception("Billing generation failed.");

            var billingData = await billingResponse.Content.ReadFromJsonAsync<BillingSummary>();
            if (billingData != null)
            {
                await _repository.SaveBillingSummary(billingData);
            }

            //  Trigger Payment
            var paymentMethod = dto.PaymentMethod ?? "cash";
            var paymentUrl = $"/api/Payment/make-payment?bookingId={savedBooking.BookingId}&showId={showId}&paymentMethod={paymentMethod}";

            var paymentResponse = await paymentClient.PostAsync(paymentUrl, null);

            if (!paymentResponse.IsSuccessStatusCode)
            {
                var errorContent = await paymentResponse.Content.ReadAsStringAsync();
                throw new Exception($"Payment failed. StatusCode: {paymentResponse.StatusCode}, Response: {errorContent}");
            }

            var payment = await paymentResponse.Content.ReadFromJsonAsync<PaymentDto>();
            if (payment == null || payment.PaymentId <= 0)
                throw new Exception("Invalid payment response received.");

            savedBooking.PaymentId = payment.PaymentId;
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

            return showInstance?.ShowInstanceId;
        }
    }
}



