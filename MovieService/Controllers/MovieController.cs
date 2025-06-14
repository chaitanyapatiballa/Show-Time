﻿using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using MovieService.Services;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _service;

    public MovieController(IMovieService service)
    {
        _service = service;
    }

    [HttpGet("movies")]
    public async Task<ActionResult<List<Movie>>> GetAllMovies() => await _service.GetAllAsync();

    [HttpGet("movies/{id}")]
    public async Task<ActionResult<Movie>> GetMovieById(int id)
    {
        var movie = await _service.GetByIdAsync(id);
        if (movie == null) return NotFound();
        return movie;
    }

    [HttpPost("movies")]
    public async Task<ActionResult> CreateMovie(Movie movie)
    {
        await _service.AddAsync(movie);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Movieid }, movie);
    }

    [HttpPut("movies/{id}")]
    public async Task<ActionResult> UpdateMovie(int id, Movie updatedMovie)
    {
        if (id != updatedMovie.Movieid) return BadRequest();
        await _service.UpdateAsync(updatedMovie);
        return NoContent();
    }

    [HttpDelete("movies/{id}")]
    public async Task<ActionResult> DeleteMovie(int id)
    {
        var movie = await _service.GetByIdAsync(id);
        if (movie == null) return NotFound();
        await _service.DeleteAsync(movie);
        return NoContent();
    }

    [HttpGet("showtemplates")]
    public async Task<ActionResult<List<Showtemplate>>> GetAllShowtemplates() => await _service.GetAllShowtemplatesAsync();

    [HttpGet("showtemplates/{id}")]
    public async Task<ActionResult<Showtemplate>> GetShowtemplateById(int id)
    {
        var template = await _service.GetShowtemplateByIdAsync(id);
        if (template == null) return NotFound();
        return template;
    }

    [HttpPost("showtemplates")]
    public async Task<ActionResult> CreateShowtemplate(Showtemplate template)
    {
        await _service.AddShowtemplateAsync(template);
        return CreatedAtAction(nameof(GetShowtemplateById), new { id = template.Showtemplateid }, template);
    }

    [HttpPut("showtemplates/{id}")]
    public async Task<ActionResult> UpdateShowtemplate(int id, Showtemplate template)
    {
        if (id != template.Showtemplateid) return BadRequest();
        await _service.UpdateShowtemplateAsync(template);
        return NoContent();
    }

    [HttpDelete("showtemplates/{id}")]
    public async Task<ActionResult> DeleteShowtemplate(int id)
    {
        var template = await _service.GetShowtemplateByIdAsync(id);
        if (template == null) return NotFound();
        await _service.DeleteShowtemplateAsync(template);
        return NoContent();
    }

    [HttpGet("showinstances")]
    public async Task<ActionResult<List<Showinstance>>> GetAllShowinstances() => await _service.GetAllShowinstancesAsync();

    [HttpGet("showinstances/{id}")]
    public async Task<ActionResult<Showinstance>> GetShowinstanceById(int id)
    {
        var instance = await _service.GetShowinstanceByIdAsync(id);
        if (instance == null) return NotFound();
        return instance;
    }

    [HttpPost("showinstances")]
    public async Task<ActionResult> CreateShowinstance(Showinstance instance)
    {
        await _service.AddShowinstanceAsync(instance);
        return CreatedAtAction(nameof(GetShowinstanceById), new { id = instance.Showinstanceid }, instance);
    }

    [HttpPut("showinstances/{id}")]
    public async Task<ActionResult> UpdateShowinstance(int id, Showinstance instance)
    {
        if (id != instance.Showinstanceid) return BadRequest();
        await _service.UpdateShowinstanceAsync(instance);
        return NoContent();
    }

    [HttpDelete("showinstances/{id}")]
    public async Task<ActionResult> DeleteShowinstance(int id)
    {
        var instance = await _service.GetShowinstanceByIdAsync(id);
        if (instance == null) return NotFound();
        await _service.DeleteShowinstanceAsync(instance);
        return NoContent();
    }
}

