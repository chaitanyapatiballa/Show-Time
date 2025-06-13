using DBModels.Db;
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

        public async Task<List<Movie>> GetMovies()
        {
            return await _movieRepository.GetAllMoviesAsync();
        }

        public async Task<Movie?> GetMovieById(int id)
        {
            return await _movieRepository.GetMovieByIdAsync(id);
        }

        public async Task<Movie> AddMovie(Movie movie)
        {
            return await _movieRepository.AddMovieAsync(movie);
        }

        public async Task<Movie?> UpdateMovie(Movie movie)
        {
            return await _movieRepository.UpdateMovieAsync(movie);
        }

        public async Task<bool> DeleteMovie(int id)
        {
            return await _movieRepository.DeleteMovieAsync(id);
        }
    }
}