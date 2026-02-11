using DBModels.Models;

namespace DataAccessLayer.Repositories;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(int id);
    Task<List<Event>> GetByTypeAsync(EventType type);
    Task AddAsync(Event evt);
    Task UpdateAsync(Event evt);
    Task DeleteAsync(int id);
}
