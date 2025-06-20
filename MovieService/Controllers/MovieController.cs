using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieLogic _logic;

    public MovieController(MovieLogic logic)
    {
        _logic = logic;
    }

    [HttpGet("movies")]
    public async Task<ActionResult<List<Movie>>> GetAllMovies()
    {
        try
        {
            return await _logic.GetAllAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("movies/{id}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        try
        {
            var movie = await _logic.GetByIdAsync(id);
            if (movie == null) return NotFound();
            return movie;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("movies")]
    public async Task<IActionResult> CreateMovie(MovieDto dto)
    {
        try
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Duration = dto.Duration,
                Releasedate = dto.Releasedate
            };
            var created = await _logic.AddAsync(movie);
            return CreatedAtAction(nameof(GetMovieById), new { id = created.Movieid }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("movies/{id}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieDto dto)
    {
        try
        {
            var existing = await _logic.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _logic.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("movies/{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        try
        {
            var existing = await _logic.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _logic.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("movies/{id}/shows")]
    public async Task<IActionResult> GetShowinstancesForMovie(int id)
    {
        try
        {
            var shows = await _logic.GetShowinstancesForMovieAsync(id);
            return Ok(shows);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("movies/{id}/theaters")]
    public async Task<IActionResult> GetTheatersForMovie(int id)
    {
        try
        {
            var theaters = await _logic.GetTheatersForMovieAsync(id);
            return Ok(theaters);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
