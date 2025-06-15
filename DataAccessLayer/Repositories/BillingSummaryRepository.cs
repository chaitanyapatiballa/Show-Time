using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BillingSummaryRepository
    {
        private readonly AppDbContext _context;

        public BillingSummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Billingsummary> AddAsync(Billingsummary summary)
        {
            _context.BillingSummaries.Add(summary);
            await _context.SaveChangesAsync();
            return summary;
        }

        public async Task<Billingsummary> GetByBookingIdAsync(int bookingId)
        {
            return await _context.BillingSummaries.FirstOrDefaultAsync(pb => pb.Bookingid == bookingId);
        }
    }
}
