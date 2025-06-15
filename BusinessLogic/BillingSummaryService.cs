using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace BusinessLogic
{
    public class BillingSummaryService
    {
        private readonly BillingSummaryRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public BillingSummaryService(
            BillingSummaryRepository repository,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<Billingsummary> CreateAsync(int bookingId, int showId, string paymentMethod)
        {
            var client = _httpClientFactory.CreateClient("ShowService");

            var response = await client.GetAsync($"/api/Shows/{showId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var show = JsonConvert.DeserializeObject<ShowInstanceDto>(content);

            decimal basePrice = show.TicketPrice;
            decimal discount = GetDiscountByPaymentMethod(paymentMethod);
            decimal gstPercent = _config.GetValue<decimal>("Billing:GstPercent");
            decimal serviceFee = _config.GetValue<decimal>("Billing:ServiceFee");

            decimal discounted = basePrice - discount;
            decimal gstAmount = (discounted * gstPercent) / 100;
            decimal finalAmount = discounted + gstAmount + serviceFee;

            var summary = new Billingsummary
            {
                Bookingid = bookingId,
                BasePrice = basePrice,
                Discount = discount,
                Gst = gstAmount,
                Servicefee = serviceFee,
                FinalAmount = finalAmount
            };

            return await _repository.AddAsync(summary);
        }

        public async Task<BillingSummaryDto?> GetByBookingIdAsync(int bookingId)
        {
            var billing = await _repository.GetByBookingIdAsync(bookingId);
            if (billing == null) return null;

            return new BillingSummaryDto
            {
                BookingId = billing.Bookingid ?? 0,
                BasePrice = billing.BasePrice,
                Discount = billing.Discount ?? 0,
                GST = billing.Gst ?? 0,
                ServiceFee = billing.Servicefee ?? 0,
                FinalAmount = billing.FinalAmount
            };
        }


        private decimal GetDiscountByPaymentMethod(string method)
        {
            return method.ToLower() switch
            {
                "creditcard" => 30,
                "wallet" => 20,
                _ => 0
            };
        }
    }
}
