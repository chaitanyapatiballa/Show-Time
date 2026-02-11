using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic;

public class EventLogic
{
    private readonly IEventRepository _repository;

    public EventLogic(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Event>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Event?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<List<Event>> GetByTypeAsync(EventType type) => await _repository.GetByTypeAsync(type);

    public async Task AddAsync(EventDto dto)
    {
        var evt = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            Language = dto.Language,
            DurationMinutes = dto.DurationMinutes,
            Type = dto.Type,
            Genre = dto.Genre,
            Cast = dto.Cast,
            Director = dto.Director,
            ImageUrl = dto.ImageUrl,
            ReleaseDate = dto.ReleaseDate
        };

        await _repository.AddAsync(evt);
    }

    public async Task UpdateAsync(int id, EventDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return;

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.Language = dto.Language;
        existing.DurationMinutes = dto.DurationMinutes;
        existing.Type = dto.Type;
        existing.Genre = dto.Genre;
        existing.Cast = dto.Cast;
        existing.Director = dto.Director;
        existing.ImageUrl = dto.ImageUrl;
        existing.ReleaseDate = dto.ReleaseDate;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
