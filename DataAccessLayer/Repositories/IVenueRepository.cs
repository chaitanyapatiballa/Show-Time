using DBModels.Models;

namespace DataAccessLayer.Repositories;

public interface IVenueRepository
{
    Task<List<Venue>> GetAllAsync();
    Task<Venue?> GetByIdAsync(int id);
    Task AddAsync(Venue venue);
    Task UpdateAsync(Venue venue);
    Task DeleteAsync(int id);
    
    Task AddSectionAsync(VenueSection section);
    Task<List<VenueSection>> GetSectionsByVenueIdAsync(int venueId);
}
