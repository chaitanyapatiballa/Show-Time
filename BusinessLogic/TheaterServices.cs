using DBModels.Db;
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
        public async Task<Theater?> UpdateTheater(Theater theater)  
        {
            return await _theaterRepository.UpdateTheater(theater); 
        }
        public async Task<bool> DeleteTheater(int id)   
        {
            return await _theaterRepository.DeleteTheater(id);  
        }
    }
}
