
using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic
{
    public class PaymentLogic
    {
        private readonly PaymentRepository _repository;
        private readonly AppDbContext _context;

        public PaymentLogic(PaymentRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
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
}
