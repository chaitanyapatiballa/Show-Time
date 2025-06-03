using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Repositories
{
    public class PaymentRepository
    {
        private readonly PaymentDbContext _context;
        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
    }
}
