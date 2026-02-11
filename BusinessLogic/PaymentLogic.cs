using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using MessagingLibrary;
using Microsoft.EntityFrameworkCore;
using PaymentService.DTOs;
using PaymentService.Repositories;

namespace PaymentService.Logic;

public class PaymentLogic
{
    private readonly BillingsummaryRepository _summaryRepo;
    private readonly PaymentRepository _paymentRepo;
    private readonly BookingRepository _bookingRepo;
    private readonly AppDbContext _context;
    private readonly IRabbitMQPublisher _publisher;

    public PaymentLogic(
        BillingsummaryRepository summaryRepo,
        PaymentRepository paymentRepo,
        BookingRepository bookingRepo,
        AppDbContext context, 
        IRabbitMQPublisher publisher)
    {
        _summaryRepo = summaryRepo;
        _paymentRepo = paymentRepo;
        _bookingRepo = bookingRepo;
        _context = context;
        _publisher = publisher;
    }

    public async Task<Billingsummary?> CreateSummaryAsync(BillingsummaryDto dto)
    {
        try
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
        catch (Exception ex)
        {
            // Log exception (optional)
            Console.WriteLine($"CreateSummaryAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<Payment?> MakePaymentAsync(PaymentDto dto, int userId)
    {
        try
        {
            var booking = await _context.Bookings.FindAsync(dto.Bookingid);
            if (booking == null || booking.Status == "Cancelled")
                return null;

            var paymentDate = DateTime.SpecifyKind(dto.Paymentdate, DateTimeKind.Unspecified);

            var payment = new Payment
            {
                Bookingid = dto.Bookingid,
                Amountpaid = dto.Amountpaid,
                Paymentmethod = dto.Paymentmethod,
                Userid = userId,
                Paymentdate = paymentDate,
                Status = "Paid"
            };

            booking.Status = "Confirmed";

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

           
            string message = $"Payment confirmed for Booking ID: {dto.Bookingid}, User: {userId}, Amount: ₹{dto.Amountpaid}";
            _publisher.Publish("payment-queue", message);

            return payment;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MakePaymentAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Payment>> GetPaymentsByUserAsync(int userId)
    {
        try
        {
            return await _paymentRepo.GetPaymentsByUserAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetPaymentsByUserAsync error: {ex.Message}");
            return new List<Payment>();
        }
    }

    public async Task<bool> RefundAsync(int bookingId)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"RefundAsync error: {ex.Message}");
            return false;
        }
    }
}
