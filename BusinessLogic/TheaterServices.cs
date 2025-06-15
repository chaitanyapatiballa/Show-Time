using DBModels.Models;
using TheaterService.DTOs;
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
        public async Task<List<Theater>> GetTheaters()  
        {
            return await _theaterRepository.GetTheaters();
        }   
        public async Task<Theater?> GetTheaterById(int id)  
        {
            return await _theaterRepository.GetTheaterById(id); 
        }
        public async Task<Theater> AddTheater(Theater theater)  
        {
            return await _theaterRepository.AddTheater(theater);    
        }
        public async Task<Theater?> UpdateTheater(TheaterDto dto)
        {
            var existing = await _theaterRepository.GetTheaterById(dto.TheaterId);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Location = dto.Location;
            existing.Capacity = dto.Capacity;

            return await _theaterRepository.UpdateTheater(existing);
        }

        public async Task<bool> DeleteTheater(int id)   
        {
            return await _theaterRepository.DeleteTheater(id);  
        }
    }
}
