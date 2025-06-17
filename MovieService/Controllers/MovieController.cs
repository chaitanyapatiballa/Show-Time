using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieLogic _service;

    public MovieController(MovieLogic service)
    {
        _service = service;
    }

    [HttpGet("movies")]
    public async Task<ActionResult<List<Movie>>> GetAllMovies()
        => await _service.GetAllAsync();

    [HttpGet("movies/{id}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _service.GetByIdAsync(id);
        if (movie == null) return NotFound();
        return movie;
    }
    [HttpPost("movies")]
    public async Task<IActionResult> CreateMovie(MovieDto movieDto)
    {
        var movie = new Movie
        {
            Title = movieDto.Title,
            Genre = movieDto.Genre,
            Duration = movieDto.Duration,
            releasedate = movieDto.releasedate
        };

        var created = await _service.AddAsync(movie);
        return CreatedAtAction(nameof(GetMovieById), new { id = created.Movieid }, created);
    }



    [HttpPut("movies/{id}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieDto movieDto)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _service.UpdateAsync(id, movieDto);
        return NoContent();
    }

    [HttpDelete("movies/{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _service.DeleteAsync(id);
        return NoContent();
    }
}

    