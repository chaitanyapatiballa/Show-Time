using DBModels.Db;
using Microsoft.EntityFrameworkCore;

namespace MovieService.Repositories
{
    public class MovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetMovies()  
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieById(int id)  
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> AddMovie(Movie movie)  
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie?> UpdateMovie(Movie movie)  
        {
            var existing = await _context.Movies.FindAsync(movie.Id);
            if (existing == null) return null;

            existing.Title = movie.Title;
            existing.Genre = movie.Genre;
            existing.Duration = movie.Duration;
            existing.TheaterId = movie.TheaterId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteMovie(int id) 
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
