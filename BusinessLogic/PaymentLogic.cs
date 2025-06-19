using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using PaymentService.DTOs;
using PaymentService.Repositories;

namespace PaymentService.Logic;

public class PaymentLogic(
    BillingsummaryRepository summaryRepo,
    PaymentRepository paymentRepo,
    BookingRepository bookingRepo,
    AppDbContext context)
{
    private readonly BillingsummaryRepository _summaryRepo = summaryRepo;
    private readonly PaymentRepository _paymentRepo = paymentRepo;
    private readonly BookingRepository _bookingRepo = bookingRepo;
    private readonly AppDbContext _context = context;

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
        var booking = await _context.Bookings.FindAsync(dto.Bookingid);
        if (booking == null || booking.Status == "Cancelled") return null;

        var payment = new Payment
        {
            Bookingid = dto.Bookingid,
            Amountpaid = dto.Amountpaid,
            Paymentmethod = dto.Paymentmethod,
            Userid = dto.Userid,
            Paymentdate = dto.Paymentdate,
            Status = "Paid"
        };

        booking.Status = "Confirmed";

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<List<Payment>> GetPaymentsByUserAsync(int userId)
    {
        return await _paymentRepo.GetPaymentsByUserAsync(userId);
    }

    public async Task<bool> RefundAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null || booking.Status == "Cancelled") return false;

        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Bookingid == bookingId);
        if (payment == null) return false;

        booking.Status = "Cancelled";
        payment.Status = "Refunded";
        payment.Refunddate = DateTime.UtcNow;

        // Free the seat again
        var seatStatus = await _context.Showseatstatuses
            .FirstOrDefaultAsync(s => s.Seatid == booking.Seatid && s.Showinstanceid == booking.Showinstanceid);
        if (seatStatus != null)
            seatStatus.Isbooked = false;

        var showInstance = await _context.Showinstances.FindAsync(booking.Showinstanceid);
        if (showInstance != null)
            showInstance.Availableseats++;

        await _context.SaveChangesAsync();
        return true;
    }
}