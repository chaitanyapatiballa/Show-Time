using BookingService.DTOs;
using DataAccessLayer.Repositories;
using DBModels.Models;


namespace BusinessLogic;

public class PaymentLogic(PaymentRepository repository)
{
    private readonly PaymentRepository _repository = repository;

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