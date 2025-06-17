using DBModels.Dto;
using DBModels.Models;
using PaymentService.DTOs;
using PaymentService.Repositories;

namespace PaymentService.Logic;

public class PaymentLogic(
    BillingsummaryRepository summaryRepo,
    PaymentRepository paymentRepo)
{
    private readonly BillingsummaryRepository _summaryRepo = summaryRepo;
    private readonly PaymentRepository _paymentRepo = paymentRepo;

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
        var payment = new Payment
        {
            Bookingid = dto.Bookingid,
            Amountpaid = dto.Amountpaid,
            Paymentmethod = dto.Paymentmethod,
            Userid = dto.Userid,
            Paymentdate = dto.Paymentdate
        };

        return await _paymentRepo.AddPaymentAsync(payment);
    }

    public async Task<List<Payment>> GetPaymentsByUserAsync(int userId)
    {
        return await _paymentRepo.GetPaymentsByUserAsync(userId);
    }
}