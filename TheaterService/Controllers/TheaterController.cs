using BusinessLogic;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using TheaterService.DTOs;

namespace TheaterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheaterController(TheaterLogic service) : ControllerBase
{
    private readonly TheaterLogic _service = service;

    [HttpGet]
    public async Task<ActionResult<List<TheaterDto>>> GetAll()
    {
        var theaters = await _service.GetAllAsync();

        var dtoList = theaters.Select(t => new TheaterDto
        {
            Theaterid = t.Theaterid,
            Name = t.Name,
            Location = t.Location,
            MovieIds = t.MovieTheaters.Select(mt => mt.movieid).ToList()
        }).ToList();

        return dtoList;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<TheaterDto>> GetById(int id)
    {
        var theater = await _service.GetByIdAsync(id);
        if (theater == null) return NotFound();

        var dto = new TheaterDto
        {
            Theaterid = theater.Theaterid,
            Name = theater.Name,
            Location = theater.Location,
            MovieIds = theater.MovieTheaters.Select(mt => mt.movieid).ToList()
        };

        return dto;
    }


    [HttpPost]
    public async Task<ActionResult> Create(TheaterDto dto)
    {
        var theater = new Theater
        {
            Name = dto.Name,
            Location = dto.Location
        };

        await _service.AddAsync(theater, dto.MovieIds);
        return CreatedAtAction(nameof(GetById), new { id = theater.Theaterid }, theater);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TheaterDto dto)
    {
        var theater = new Theater
        {
            Theaterid = id,
            Name = dto.Name,
            Location = dto.Location
        };

        await _service.UpdateAsync(theater); 
        return NoContent();
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var theater = await _service.GetByIdAsync(id);
        if (theater == null) return NotFound();
        await _service.DeleteAsync(theater);
        return NoContent();
    }
}
