using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly AppDbContext _context;

    public VenueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Venue>> GetAllAsync() => await _context.Venues.ToListAsync();

    public async Task<Venue?> GetByIdAsync(int id) => 
        await _context.Venues.Include(v => v.Sections).FirstOrDefaultAsync(v => v.VenueId == id);

    public async Task AddAsync(Venue venue)
    {
        _context.Venues.Add(venue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venue venue)
    {
        _context.Venues.Update(venue);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddSectionAsync(VenueSection section)
    {
        _context.VenueSections.Add(section);
        await _context.SaveChangesAsync();
    }

    public async Task<List<VenueSection>> GetSectionsByVenueIdAsync(int venueId) => 
        await _context.VenueSections.Where(vs => vs.VenueId == venueId).ToListAsync();
}
