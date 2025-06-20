using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using System;

namespace BusinessLogic
{
    public class MovieLogic
    {
        private readonly MovieRepository _repository;

        public MovieLogic(MovieRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                return new List<Movie>();
            }
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            try
            {
                return await _repository.AddMovieAsync(movie);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(int id, MovieDto dto)
        {
            try
            {
                var movie = await _repository.GetByIdAsync(id);
                if (movie == null) return;

                movie.Title = dto.Title;
                movie.Duration = dto.Duration;
                movie.Genre = dto.Genre;
                movie.Releasedate = dto.Releasedate;

                await _repository.UpdateAsync(movie);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var movie = await _repository.GetByIdAsync(id);
                if (movie != null)
                    await _repository.DeleteAsync(movie);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Showinstance>> GetShowinstancesForMovieAsync(int movieId)
        {
            try
            {
                return await _repository.GetShowinstancesForMovieAsync(movieId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetShowinstancesForMovieAsync: {ex.Message}");
                return new List<Showinstance>();
            }
        }

        public async Task<List<Theater>> GetTheatersForMovieAsync(int movieId)
        {
            try
            {
                return await _repository.GetTheatersForMovieAsync(movieId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTheatersForMovieAsync: {ex.Message}");
                return new List<Theater>();
            }
        }
    }
}
