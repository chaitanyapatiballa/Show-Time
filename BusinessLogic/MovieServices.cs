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

        public async Task<List<Movie>> GetMovies ()
        {
            return await _movieRepository.GetMovies();
        }

        public async Task<Movie?> GetMovieById (int id)
        {
            return await _movieRepository.GetMovieById (id);
        }
            
        public async Task<Movie> AddMovie (Movie movie)
        {
            return await _movieRepository.AddMovie(movie);
        }

        public async Task<Movie?> UpdateMovie (Movie movie)
        {
            return await _movieRepository.UpdateMovie (movie);
        }

        public async Task<bool> DeleteMovie (int id)
        {
            return await _movieRepository.DeleteMovie (id);
        }
    }
}

