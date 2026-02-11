using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic;

public class VenueLogic
{
    private readonly IVenueRepository _repository;

    public VenueLogic(IVenueRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Venue>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Venue?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task AddAsync(VenueDto dto)
    {
        var venue = new Venue
        {
            Name = dto.Name,
            Location = dto.Location,
            Address = dto.Address,
            TotalCapacity = dto.TotalCapacity
        };

        await _repository.AddAsync(venue);
    }

    public async Task AddSectionAsync(int venueId, VenueSectionDto dto)
    {
         var venue = await _repository.GetByIdAsync(venueId);
         if (venue == null) throw new Exception("Venue not found");

         var section = new VenueSection
         {
             VenueId = venueId,
             Name = dto.Name,
             Capacity = dto.Capacity,
             BasePriceMultiplier = dto.BasePriceMultiplier,
             RowCount = dto.RowCount,
             SeatsPerRow = dto.SeatsPerRow
         };

         await _repository.AddSectionAsync(section);
    }

    public async Task UpdateAsync(int id, VenueDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return;

        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.Address = dto.Address;
        existing.TotalCapacity = dto.TotalCapacity;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
