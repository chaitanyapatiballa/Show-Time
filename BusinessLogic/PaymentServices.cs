using DBModels.Db;
using PaymentService.Repositories;

namespace PaymentService.Services  
{
    public class PaymentService
    {
        private readonly PaymentRepository _repo;
        private readonly BillingSummaryService _billingService;

        public PaymentService(PaymentRepository repo, BillingSummaryService billingService)
        {
            _repo = repo;
            _billingService = billingService;
        }

        public async Task<Payment> MakePaymentAsync(int bookingId, int showId, string paymentMethod)
        {
            var billing = await _billingService.CreateAsync(bookingId, showId, paymentMethod);

            var payment = new Payment
            {
                BookingId = bookingId,
                AmountPaid = billing.FinalAmount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.UtcNow
            };

            return await _repo.AddAsync(payment);
        }

        public async Task<Payment?> GetPaymentByBookingIdAsync(int bookingId)
        {
            return await _repo.GetByBookingIdAsync(bookingId);
        }

    }
}