using DBModels.Models;
using PaymentService.Repositories;

namespace PaymentService.Services
{
    public class PaymentServices
    {
        private readonly PaymentRepository _paymentRepository;
        public PaymentServices(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> ProcessPaymentAsync(Payment payment)
        {

            payment.PaymentTime = DateTime.UtcNow;
            payment.IsSuccessful = true; 

            await _paymentRepository.AddPaymentAsync(payment);

            return payment.IsSuccessful;
        }

    }
}
