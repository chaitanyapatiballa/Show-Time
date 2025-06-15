using DBModels.Models;
using TheaterService.Repositories;

namespace TheaterService.Services;

public class ITheaterService    
{   
    private readonly TheaterRepository _repository;

    public ITheaterService(TheaterRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Theater>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Theater?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task AddAsync(Theater theater, List<int>? movieIds) => await _repository.AddAsync(theater, movieIds);

    public async Task UpdateAsync(Theater theater, List<int>? movieIds) => await _repository.UpdateAsync(theater, movieIds);

    public async Task DeleteAsync(Theater theater) => await _repository.DeleteAsync(theater);
}
