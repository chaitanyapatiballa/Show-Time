using BookingService.DTOs;
using BookingService.Repositories;
using DBModels.Models;
using System.Net.Http.Json;

namespace BookingService.Services;

public class IBookingService
{
    private readonly BookingRepository _bookingRepo;
    private readonly BillingsummaryRepository _summaryRepo;
    private readonly PaymentRepository _paymentRepo;
    private readonly IHttpClientFactory _httpClientFactory;

    public IBookingService( 
        BookingRepository bookingRepo,
        BillingsummaryRepository summaryRepo,
        PaymentRepository paymentRepo,
        IHttpClientFactory httpClientFactory)
    {
        _bookingRepo = bookingRepo;
        _summaryRepo = summaryRepo;
        _paymentRepo = paymentRepo;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> ValidateMovieAsync(int movieId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("MovieService");
            var response = await client.GetAsync($"/api/movie/{movieId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> ValidateTheaterAsync(int theaterId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("TheaterService");
            var response = await client.GetAsync($"/api/theater/{theaterId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<Booking?> CreateBookingAsync(BookingDto dto)
    {
        if (!await ValidateMovieAsync(dto.Movieid) || !await ValidateTheaterAsync(dto.Theaterid))
            return null;

        var booking = new Booking
        {
            Movieid = dto.Movieid,
            Theaterid = dto.Theaterid,
            Userid = dto.Userid,
            Seatnumber = dto.Seatnumber,
            Showtime = dto.Showtime,
            Bookingtime = DateTime.UtcNow,
            Status = "Confirmed"
        };

        return await _bookingRepo.AddBookingAsync(booking);
    }

    public async Task<Billingsummary> CreateSummaryAsync(BillingsummaryDto dto)
    {
        var summary = new Billingsummary
        {
            Bookingid = dto.Bookingid,
            Baseamount = dto.Baseamount,
            Discount = dto.Discount,
            Gst = dto.Gst,
            Servicefee = dto.Servicefee,
            Totalamount = dto.Totalamount
        };

        return await _summaryRepo.AddSummaryAsync(summary);
    }

    public async Task<Payment?> MakePaymentAsync(PaymentDto dto)
    {
        var client = _httpClientFactory.CreateClient("PaymentService");
        var response = await client.PostAsJsonAsync("/api/payment/pay", dto);

        if (response.IsSuccessStatusCode)
        {
            var savedPayment = new Payment
            {
                Bookingid = dto.Bookingid,
                Amountpaid = dto.Amountpaid,
                Paymentmethod = dto.Paymentmethod,
                Userid = dto.Userid,
                Paymentdate = dto.Paymentdate
            };

            return await _paymentRepo.AddPaymentAsync(savedPayment);
        }

        return null;
    }
}

