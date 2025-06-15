using DBModels.Models;
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
                MovieId = mt.Movieid,
                MovieTitle = mt.Movie?.Title,
                TheaterId = mt.Theaterid,
                TheaterName = mt.Theater?.Name,
                TheaterLocation = mt.Theater?.Location
            }).ToList<object>();

            return result;
        }

        public async Task<MovieTheater> AssignMovieToTheater(MovieTheaterDto dto)
        {
            var assignment = new MovieTheater
            {
                Movieid = dto.MovieId,
                Theaterid = dto.TheaterId
            };

            await _repository.AddAssignmentAsync(assignment);
            return assignment;
        }
    }
}