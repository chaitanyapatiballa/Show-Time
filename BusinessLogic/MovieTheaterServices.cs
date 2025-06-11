using DBModels.Db;
using TheaterService.DTOs;
using TheaterService.Repositories;

namespace TheaterService.Services
{
    public class MovieTheaterServices
    {
        private readonly MovieTheaterRepository _repository;

        public MovieTheaterServices(MovieTheaterRepository repository)
        {
            _repository = repository;
        }

        public async Task<MovieTheater> AssignMovieToTheaterAsync(MovieTheaterDto dto)
        {
            var assignment = new MovieTheater
            {
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId
            };
            return await _repository.AssignMovieToTheaterAsync(assignment);
        }

        public async Task<List<MovieTheater>> GetAllAssignmentsAsync()
        {
            return await _repository.GetAllAssignmentsAsync();
        }
    }
}
