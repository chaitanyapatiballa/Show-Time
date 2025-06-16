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
    public async Task<ActionResult<List<Theater>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Theater>> GetById(int id)
    {
        var theater = await _service.GetByIdAsync(id);
        if (theater == null) return NotFound();
        return theater;
    }

    [HttpPost]
    public async Task<ActionResult> Create(TheaterDto dto)
    {
        var theater = new Theater { Name = dto.Name, Location = dto.Location };
        await _service.AddAsync(theater, dto.MovieIds);
        return CreatedAtAction(nameof(GetById), new { id = theater.Theaterid }, theater);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TheaterDto dto)
    {
        if (id != dto.Theaterid) return BadRequest();

        var theater = new Theater
        {
            Theaterid = dto.Theaterid,
            Name = dto.Name,
            Location = dto.Location
        };

        await _service.UpdateAsync(theater, dto.MovieIds);
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
