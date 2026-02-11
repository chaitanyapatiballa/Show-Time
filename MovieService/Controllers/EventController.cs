using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly EventLogic _logic;

    public EventController(EventLogic logic)
    {
        _logic = logic;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _logic.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evt = await _logic.GetByIdAsync(id);
        if (evt == null) return NotFound();
        return Ok(evt);
    }

    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(EventType type)
    {
        var events = await _logic.GetByTypeAsync(type);
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EventDto dto)
    {
        await _logic.AddAsync(dto);
        return Ok("Event created successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EventDto dto)
    {
        await _logic.UpdateAsync(id, dto);
        return Ok("Event updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _logic.DeleteAsync(id);
        return Ok("Event deleted successfully");
    }
}
