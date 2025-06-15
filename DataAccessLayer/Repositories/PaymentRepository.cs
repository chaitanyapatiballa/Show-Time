using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class PaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> AddPaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<List<Payment>> GetPaymentsByUserAsync(int userId)
    {
        return await _context.Payments
            .Where(p => p.Userid == userId)
            .ToListAsync();
    }
}
