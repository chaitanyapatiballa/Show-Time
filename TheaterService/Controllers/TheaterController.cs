using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using TheaterService.DTOs;

namespace MovieBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TheaterController : ControllerBase
{
    private readonly TheaterLogic _logic;

    public TheaterController(TheaterLogic logic)
    {
        _logic = logic;
    }

   
    [HttpGet]
    public async Task<ActionResult<List<Theater>>> GetAllTheaters()
    {
        var theaters = await _logic.GetAllAsync();
        return Ok(theaters);
    }

   
    [HttpGet("{id}")]
    public async Task<ActionResult<Theater>> GetTheaterById(int id)
    {
        var theater = await _logic.GetByIdAsync(id);
        if (theater == null)
            return NotFound();

        return Ok(theater);
    }

    
    [HttpPost]
    public async Task<ActionResult> CreateTheater([FromBody] TheaterDto dto)
    {
        var theater = new Theater
        {
            Name = dto.Name,
            Location = dto.Location,
            Capacity = dto.Capacity
        };

        await _logic.AddAsync(theater, dto.MovieIds);

        return CreatedAtAction(nameof(GetTheaterById), new { id = theater.Theaterid }, theater);
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTheater(int id, [FromBody] TheaterDto dto)
    {
        var existing = await _logic.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.Capacity = dto.Capacity;

        await _logic.UpdateAsync(existing, dto.MovieIds);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTheater(int id)
    {
        var existing = await _logic.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _logic.DeleteAsync(existing);
        return NoContent();
    }
}
