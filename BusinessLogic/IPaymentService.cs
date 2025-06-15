using BookingService.DTOs;
using BookingService.Repositories;
using DBModels.Models;


namespace PaymentService.Services;

public class IPaymentService
{
    private readonly PaymentRepository _repository;

    public IPaymentService(PaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Payment> ProcessPaymentAsync(PaymentDto dto)   
    {
        var payment = new Payment
        {
            Bookingid = dto.Bookingid,
            Amountpaid = dto.Amountpaid,
            Paymentmethod = dto.Paymentmethod,
            Userid = dto.Userid,
            Paymentdate = DateTime.UtcNow
        };

        return await _repository.AddPaymentAsync(payment);
    }

    public async Task<List<Payment>> GetUserPaymentsAsync(int userId)
    {
        return await _repository.GetPaymentsByUserAsync(userId);
    }
}