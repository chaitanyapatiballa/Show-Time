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

        [HttpPost("Add-Theater")]
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

        //[HttpPost("seed-seats")]
        //public async Task<IActionResult> SeedSeats()
        //{
        //    await _logic.AddSeatsForExistingTheatersAsync();
        //    return Ok("Seats added for existing theaters.");
        //}
        //[HttpPost("fix-showseatstatus")]
        //public async Task<IActionResult> FixShowSeatStatuses()
        //{
        //    await _logic.AddMissingShowseatStatusesAsync();
        //    return Ok("✅ Showseatstatuses fixed for all showinstances.");
        //}


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

        [HttpGet("showtemplates")]
        public async Task<ActionResult<List<Showtemplate>>> GetAllShowtemplates()
            => await _logic.GetAllShowtemplatesAsync();

        [HttpGet("showtemplates/{id}")]
        public async Task<ActionResult<Showtemplate>> GetShowtemplateById(int id)
        {
            var template = await _logic.GetShowtemplateByIdAsync(id);
            if (template == null) return NotFound();
            return template;
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
        public async Task<ActionResult<List<Showinstance>>> GetAllShowinstances() =>
            await _logic.GetAllShowinstancesAsync();

        [HttpGet("showinstances/{id}")]
        public async Task<ActionResult<Showinstance>> GetShowinstanceById(int id)
        {
            var instance = await _logic.GetShowinstanceByIdAsync(id);
            if (instance == null) return NotFound();
            return instance;
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
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("showinstances/{id}")]
        public async Task<ActionResult> DeleteShowinstance(int id)
        {
            var deleted = await _logic.DeleteShowinstanceAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
