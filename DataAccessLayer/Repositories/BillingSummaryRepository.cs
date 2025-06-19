using DBModels.Models;

namespace DataAccessLayer.Repositories;
    
public class BillingsummaryRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<Billingsummary> AddSummaryAsync(Billingsummary summary)
    {
        _context.Billingsummaries.Add(summary);
        await _context.SaveChangesAsync();
        return summary;
    }
}