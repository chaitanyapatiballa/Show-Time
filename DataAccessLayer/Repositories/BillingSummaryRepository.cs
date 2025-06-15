using DBModels.Models;

namespace BookingService.Repositories;

public class BillingsummaryRepository
{
    private readonly AppDbContext _context;
    public BillingsummaryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Billingsummary> AddSummaryAsync(Billingsummary summary)
    {
        _context.Billingsummaries.Add(summary);
        await _context.SaveChangesAsync();
        return summary;
    }
}