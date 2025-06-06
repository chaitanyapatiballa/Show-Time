using DBModels.Models;
using TheaterService.Repositories;

namespace TheaterService.Services
{
    public class TheaterServices
    {
        private readonly TheaterRepository _theaterRepository;
        public TheaterServices(TheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }
        public async Task<List<Theater>> GetTheatersAsync()
        {
            return await _theaterRepository.GetTheatersAsync();
        }
        public async Task<Theater?> GetTheaterByIdAsync(int id)
        {
            return await _theaterRepository.GetTheaterByIdAsync(id);
        }
        public async Task<Theater> AddTheaterAsync(Theater theater)
        {
            return await _theaterRepository.AddTheaterAsync(theater);
        }
        public async Task<Theater?> UpdateTheaterAsync(Theater theater)
        {
            return await _theaterRepository.UpdateTheaterAsync(theater);
        }
        public async Task<bool> DeleteTheaterAsync(int id)
        {
            return await _theaterRepository.DeleteTheaterAsync(id);
        }
    }
}
