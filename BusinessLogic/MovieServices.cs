using DBModels.Models;
using MovieService.Repositories;
namespace MovieService.Services
{
    public class MovieServices
    {
        private readonly MovieRepository _movieRepository;

        public MovieServices(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _movieRepository.GetMoviesAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _movieRepository.GetMovieByIdAsync(id);
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            return await _movieRepository.AddMovieAsync(movie);
        }

        public async Task<Movie?> UpdateMovieAsync(Movie movie)
        {
            
            return await _movieRepository.UpdateMovieAsync(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            return await _movieRepository.DeleteMovieAsync(id);
        }
    }
}
