using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace TheaterService.Controllers
{
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
        public async Task<ActionResult<List<Theater>>> GetAllTheaters() => Ok(await _logic.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Theater>> GetTheaterById(int id)
        {
            var theater = await _logic.GetByIdAsync(id);
            return theater == null ? NotFound() : Ok(theater);
        }

        [HttpPost("Add-Theater")]
        public async Task<ActionResult> CreateTheater([FromBody] TheaterDto dto)
        {
            var theater = new Theater { Name = dto.Name, Location = dto.Location, Capacity = dto.Capacity };
            await _logic.AddAsync(theater, dto.MovieIds);
            return CreatedAtAction(nameof(GetTheaterById), new { id = theater.Theaterid }, theater);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTheater(int id, [FromBody] TheaterDto dto)
        {
            var existing = await _logic.GetByIdAsync(id);
            if (existing == null) return NotFound();
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
            if (existing == null) return NotFound();
            await _logic.DeleteAsync(existing);
            return NoContent();
        }

        [HttpGet("showtemplates")]
        public async Task<ActionResult<List<Showtemplate>>> GetAllShowtemplates() => Ok(await _logic.GetAllShowtemplatesAsync());

        [HttpGet("showtemplates/{id}")]
        public async Task<ActionResult<Showtemplate>> GetShowtemplateById(int id)
        {
            var template = await _logic.GetShowtemplateByIdAsync(id);
            return template == null ? NotFound() : Ok(template);
        }

        [HttpPost("showtemplates")]
        public async Task<ActionResult> CreateShowtemplate(ShowtemplateDto dto)
        {
            var template = await _logic.AddShowtemplateAsync(dto);
            return CreatedAtAction(nameof(GetShowtemplateById), new { id = template.Showtemplateid }, template);
        }

        [HttpPut("showtemplates/{id}")]
        public async Task<ActionResult> UpdateShowtemplate(int id, ShowtemplateDto dto)
        {
            var existing = await _logic.GetShowtemplateByIdAsync(id);
            if (existing == null) return NotFound();
            await _logic.UpdateShowtemplateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("showtemplates/{id}")]
        public async Task<ActionResult> DeleteShowtemplate(int id)
        {
            var template = await _logic.GetShowtemplateByIdAsync(id);
            if (template == null) return NotFound();
            await _logic.DeleteShowtemplateAsync(id);
            return NoContent();
        }

        [HttpGet("showinstances")]
        public async Task<ActionResult<List<Showinstance>>> GetAllShowinstances() => Ok(await _logic.GetAllShowinstancesAsync());

        [HttpGet("showinstances/{id}")]
        public async Task<ActionResult<Showinstance>> GetShowinstanceById(int id)
        {
            var instance = await _logic.GetShowinstanceByIdAsync(id);
            return instance == null ? NotFound() : Ok(instance);
        }

        [HttpPost("showinstances")]
        public async Task<ActionResult> CreateShowinstance([FromBody] ShowinstanceDto dto)
        {
            try
            {
                var instance = await _logic.AddShowinstanceAsync(dto);
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
            var updated = await _logic.UpdateShowinstanceAsync(id, dto);
            return !updated ? NotFound() : NoContent();
        }

        [HttpDelete("showinstances/{id}")]
        public async Task<ActionResult> DeleteShowinstance(int id)
        {
            var deleted = await _logic.DeleteShowinstanceAsync(id);
            return !deleted ? NotFound() : NoContent();
        }

        [HttpGet("showinstances/by-movie/{movieId}")]
        public async Task<ActionResult<List<Showinstance>>> GetShowinstancesByMovie(int movieId)
        {
            var result = await _logic.GetShowinstancesByMovieAsync(movieId);
            return Ok(result);
        }

        [HttpGet("showinstances/{showInstanceId}/seats")]
        public async Task<ActionResult<List<Showseatstatus>>> GetSeatStatusesByShowinstanceId(int showInstanceId)
        {
            var result = await _logic.GetSeatStatusesByShowInstanceIdAsync(showInstanceId);
            return Ok(result);
        }
    }
}
