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

        public async Task<List<object>> GetAllAssignments()
        {
            var assignments = await _repository.GetAllAssignmentsAsync();
            var result = assignments.Select(mt => new
            {
                MovieId = mt.MovieId,
                MovieTitle = mt.Movie?.Title,
                TheaterId = mt.TheaterId,
                TheaterName = mt.Theater?.Name,
                TheaterLocation = mt.Theater?.Location
            }).ToList<object>();

            return result;
        }

        public async Task<MovieTheater> AssignMovieToTheater(MovieTheaterDto dto)
        {
            var assignment = new MovieTheater
            {
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId
            };

            await _repository.AddAssignmentAsync(assignment);
            return assignment;
        }
    }
}