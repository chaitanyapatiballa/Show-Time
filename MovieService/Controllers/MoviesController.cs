using DBModels.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieService.DTOs;
using MovieService.Models;
using MovieService.Services;


namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieServices _movieService;

        public MoviesController(MovieServices movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("GetMovies")]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            try
            {
                var movies = await _movieService.GetMoviesAsync();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving movies: {ex.Message}");
            }
        }
        [HttpGet("GetMovie/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieByIdAsync(id);
                if (movie == null)
                {
                    return NotFound($"Movie with ID {id} not found.");
                }
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving movie: {ex.Message}");
            }
        }
        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movie>> AddMovie(MovieDto movieDto)
        {
            try
            {
                if (movieDto == null ||
                    string.IsNullOrEmpty(movieDto.Title) ||
                    string.IsNullOrEmpty(movieDto.Genre) ||
                    string.IsNullOrEmpty(movieDto.Duration))
                {
                    return BadRequest("Invalid movie data.");
                }

                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    Duration = movieDto.Duration
                };

                var createdMovie = await _movieService.AddMovieAsync(movie);
                return CreatedAtAction(nameof(GetMovies), new { id = createdMovie.Id }, createdMovie);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error while adding movie: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding movie: {ex.Message}");
            }
        }


        [HttpPut("UpdateMovie/{id}")]
        public async Task<ActionResult<Movie>> UpdateMovie(int id, MovieDto movieDto)
        {
            try
            {
                if (movieDto == null || string.IsNullOrEmpty(movieDto.Title) || string.IsNullOrEmpty(movieDto.Genre) || string.IsNullOrEmpty(movieDto.Duration))
                {
                    return BadRequest("Invalid movie data.");
                }

                var movie = new Movie
                {
                    Id = id,
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    Duration = movieDto.Duration
                };

                var updatedMovie = await _movieService.UpdateMovieAsync(movie);
                if (updatedMovie == null)
                {
                    return NotFound($"Movie with ID {id} not found.");
                }

                return Ok(updatedMovie);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error while updating movie: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating movie: {ex.Message}");
            }
        }

        [HttpDelete("DeleteMovie/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var deleted = await _movieService.DeleteMovieAsync(id);
                if (!deleted)
                {
                    return NotFound($"Movie with ID {id} not found.");
                }

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error while deleting movie: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting movie: {ex.Message}");
            }
        }
    }
}




