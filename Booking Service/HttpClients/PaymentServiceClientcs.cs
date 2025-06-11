using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Booking_Service.HttpClients
{
    public class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreatePaymentAsync(int bookingId)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/payment", new { BookingId = bookingId });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            return result.PaymentId;
        }

        public async Task<string?> ProcessPaymentAsync(int paymentId)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/payment/process", new { PaymentId = paymentId });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }

    public class PaymentResponse
    {
        public int PaymentId { get; set; }
    }
}
