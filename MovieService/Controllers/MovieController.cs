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

    [HttpGet("showtemplates")]
    public async Task<ActionResult<List<Showtemplate>>> GetAllShowtemplates()
        => await _service.GetAllShowtemplatesAsync();

    [HttpGet("showtemplates/{id}")]
    public async Task<ActionResult<Showtemplate>> GetShowtemplateById(int id)
    {
        var template = await _service.GetShowtemplateByIdAsync(id);
        if (template == null) return NotFound();
        return template;
    }

    [HttpPost("showtemplates")]
    public async Task<ActionResult> CreateShowtemplate(ShowtemplateDto dto)
    {
        var template = await _service.AddShowtemplateAsync(dto);
        return CreatedAtAction(nameof(GetShowtemplateById), new { id = template.Showtemplateid }, template);
    }
    [HttpPut("showtemplates/{id}")]
    public async Task<ActionResult> UpdateShowtemplate(int id, ShowtemplateDto dto)
    {
        var existing = await _service.GetShowtemplateByIdAsync(id);
        if (existing == null) return NotFound();

        await _service.UpdateShowtemplateAsync(id, dto);
        return NoContent();
    }


    [HttpDelete("showtemplates/{id}")]
    public async Task<ActionResult> DeleteShowtemplate(int id)
    {
        var template = await _service.GetShowtemplateByIdAsync(id);
        if (template == null) return NotFound();
        await _service.DeleteShowtemplateAsync(id);
        return NoContent();
    }

    [HttpGet("showinstances")]
    public async Task<ActionResult<List<Showinstance>>> GetAllShowinstances() =>
     await _service.GetAllShowinstancesAsync();

    [HttpGet("showinstances/{id}")]
    public async Task<ActionResult<Showinstance>> GetShowinstanceById(int id)
    {
        var instance = await _service.GetShowinstanceByIdAsync(id);
        if (instance == null) return NotFound();
        return instance;
    }

    [HttpPost("showinstances")]
    public async Task<ActionResult> CreateShowinstance([FromBody] ShowinstanceDto dto)
    {
        try
        {
            var instance = await _service.AddShowinstanceAsync(dto);
            return CreatedAtAction(nameof(GetShowinstanceById), new { id = instance.Showinstanceid }, instance);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("showinstances/{id}")]
    public async Task<ActionResult> UpdateShowinstance(int id, [FromBody] ShowinstanceDto dto)
    {
        var updated = await _service.UpdateShowinstanceAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("showinstances/{id}")]
    public async Task<ActionResult> DeleteShowinstance(int id)
    {
        var deleted = await _service.DeleteShowinstanceAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}