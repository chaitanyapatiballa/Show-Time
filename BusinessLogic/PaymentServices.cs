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

        public async Task<Payment> ProcessPayment(Payment payment)
        {
            // Set necessary fields before saving
            payment.PaymentTime = DateTime.UtcNow;
            payment.IsSuccessful = true;

            // Save to DB
            await _paymentRepository.AddPayment(payment);

            // Return the full payment object (with auto-generated PaymentId)
            return payment;
        }
    }
}
