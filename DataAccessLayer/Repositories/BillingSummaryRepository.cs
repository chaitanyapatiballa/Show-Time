using DBModels.Db;
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

        public async Task<BillingSummary> AddAsync(BillingSummary summary)
        {
            _context.BillingSummaries.Add(summary);
            await _context.SaveChangesAsync();
            return summary;
        }

        public async Task<BillingSummary> GetByBookingIdAsync(int bookingId)
        {
            return await _context.BillingSummaries.FirstOrDefaultAsync(pb => pb.BookingId == bookingId);
        }
    }
}
