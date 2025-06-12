using DBModels.Db;
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

        public async Task<bool> ProcessPayment(Payment payment) 
        {

            payment.PaymentTime = DateTime.UtcNow;
            payment.IsSuccessful = true; 

            await _paymentRepository.AddPayment(payment);   

            return payment.IsSuccessful;
        }

    }
}
