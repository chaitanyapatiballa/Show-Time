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
        return await _logic.GetAllAsync();
    }

    [HttpGet("movies/{id}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _logic.GetByIdAsync(id);
        if (movie == null) return NotFound();
        return movie;
    }

    [HttpPost("movies")]
    public async Task<IActionResult> CreateMovie(MovieDto dto)
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

    [HttpPut("movies/{id}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieDto dto)
    {
        var existing = await _logic.GetByIdAsync(id);
        if (existing == null) return NotFound();
        await _logic.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("movies/{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var existing = await _logic.GetByIdAsync(id);
        if (existing == null) return NotFound();
        await _logic.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("movies/{id}/shows")]
    public async Task<IActionResult> GetShowinstancesForMovie(int id)
    {
        var shows = await _logic.GetShowinstancesForMovieAsync(id);
        return Ok(shows);
    }

    [HttpGet("movies/{id}/theaters")]
    public async Task<IActionResult> GetTheatersForMovie(int id)
    {
        var theaters = await _logic.GetTheatersForMovieAsync(id);
        return Ok(theaters);
    }
}
