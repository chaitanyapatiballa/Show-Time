using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace TheaterService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VenueController : ControllerBase
{
    private readonly VenueLogic _logic;

    public VenueController(VenueLogic logic)
    {
        _logic = logic;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var venues = await _logic.GetAllAsync();
        return Ok(venues);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var venue = await _logic.GetByIdAsync(id);
        if (venue == null) return NotFound();
        return Ok(venue);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VenueDto dto)
    {
        await _logic.AddAsync(dto);
        return Ok("Venue created successfully");
    }

    [HttpPost("{id}/sections")]
    public async Task<IActionResult> AddSection(int id, VenueSectionDto dto)
    {
        await _logic.AddSectionAsync(id, dto);
        return Ok("Section added successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VenueDto dto)
    {
        await _logic.UpdateAsync(id, dto);
        return Ok("Venue updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _logic.DeleteAsync(id);
        return Ok("Venue deleted successfully");
    }
}
