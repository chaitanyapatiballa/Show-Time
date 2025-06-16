using DataAccessLayer.Repositories;
using DBModels.Models;

namespace BusinessLogic
{
    public class TheaterLogic(TheaterRepository repository)
    {   
        private readonly TheaterRepository _repository = repository;

        public async Task<List<Theater>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Theater?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task AddAsync(Theater theater, List<int>? movieIds) => await _repository.AddAsync(theater, movieIds);

        public async Task UpdateAsync(Theater theater, List<int>? movieIds) => await _repository.UpdateAsync(theater, movieIds);

        public async Task DeleteAsync(Theater theater) => await _repository.DeleteAsync(theater);
    }
}
