using DataAccessLayer.Repositories;
using DBModels.Db;
using DBModels.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class BillingSummaryService
{
    private readonly BillingSummaryRepository _repository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public BillingSummaryService(BillingSummaryRepository repository, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<BillingSummary> CreateAsync(int bookingId, int showId, string paymentMethod)
    {
        var client = _httpClientFactory.CreateClient("ShowService");
        var res = await client.GetAsync($"/api/Shows/{showId}");
        res.EnsureSuccessStatusCode();

        var show = JsonConvert.DeserializeObject<ShowDto>(await res.Content.ReadAsStringAsync());

        decimal basePrice = show.TicketPrice;
        decimal discount = GetDiscountByPaymentMethod(paymentMethod);
        decimal gst = _config.GetValue<decimal>("Billing:GstPercent");
        decimal serviceFee = _config.GetValue<decimal>("Billing:ServiceFee");

        decimal discounted = basePrice - discount;
        decimal gstAmount = (discounted * gst) / 100;
        decimal finalAmount = discounted + gstAmount + serviceFee;

        var summary = new BillingSummary
        {
            BookingId = bookingId,
            BasePrice = basePrice,
            Discount = discount,
            GST = gstAmount,
            ServiceFee = serviceFee,
            FinalAmount = finalAmount
        };

        return await _repository.AddAsync(summary);
    }

    public async Task<BillingSummaryDto> GetByBookingIdAsync(int bookingId)
    {
        var billing = await _repository.GetByBookingIdAsync(bookingId);
        if (billing == null) return null;

        return new BillingSummaryDto
        {
            BookingId = billing.BookingId,
            BasePrice = billing.BasePrice,
            Discount = billing.Discount,
            GST = billing.GST,
            ServiceFee = billing.ServiceFee,
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