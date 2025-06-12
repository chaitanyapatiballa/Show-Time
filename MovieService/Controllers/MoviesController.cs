using DBModels.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var movies = await _movieService.GetMovies();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving movies: {ex.Message}");
            }
        }

        [HttpGet("GetMovie/{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = await _movieService.GetMovieById(id);
                if (movie == null)
                    return NotFound($"Movie with ID {id} not found.");

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving movie: {ex.Message}");
            }
        }

        [HttpPost("AddMovie")]
        public async Task<ActionResult<Movie>> AddMovie(MovieDto movieDto)
        {
            try
            {
                if (movieDto == null || string.IsNullOrEmpty(movieDto.Title) ||
                    string.IsNullOrEmpty(movieDto.Genre) || string.IsNullOrEmpty(movieDto.Duration))
                {
                    return BadRequest("Invalid movie data.");
                }

                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    Duration = movieDto.Duration,
                    TheaterId = movieDto.TheaterId
                };

                var created = await _movieService.AddMovie(movie);
                return CreatedAtAction(nameof(GetMovie), new { id = created.MovieId }, created);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding movie: {ex.Message}");
            }
        }

        [HttpPut("UpdateMovie/{id}")]
        public async Task<ActionResult<Movie>> UpdateMovie(int id, MovieDto movieDto)
        {
            try
            {
                if (movieDto == null || string.IsNullOrEmpty(movieDto.Title) ||
                    string.IsNullOrEmpty(movieDto.Genre) || string.IsNullOrEmpty(movieDto.Duration))
                {
                    return BadRequest("Invalid movie data.");
                }

                var movie = new Movie
                {
                    MovieId = id,
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    Duration = movieDto.Duration,
                    TheaterId = movieDto.TheaterId
                };

                var updated = await _movieService.UpdateMovie(movie);
                if (updated == null)
                    return NotFound($"Movie with ID {id} not found.");

                return Ok(updated);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating movie: {ex.Message}");
            }
        }

        [HttpDelete("DeleteMovie/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                var deleted = await _movieService.DeleteMovie(id);
                if (!deleted)
                    return NotFound($"Movie with ID {id} not found.");

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting movie: {ex.Message}");
            }
        }
    }
}




