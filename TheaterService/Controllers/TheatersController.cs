using DBModels.Db;
using Microsoft.AspNetCore.Mvc;
using TheaterService.Services;

namespace TheaterService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatersController : ControllerBase
    {
        private readonly TheaterServices _service; 

        public TheatersController(TheaterServices service) 
        {
            _service = service;
        }

        [HttpGet("GetTheaters")]
        public async Task<ActionResult<List<Theater>>> GetTheaters()
        {
            try
            {
                var theaters = await _service.GetTheatersAsync(); 
                return Ok(theaters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving theaters: {ex.Message}"); 
            }
        }

        [HttpGet("GetTheater/{id}")] 
        public async Task<ActionResult<Theater>> GetTheater(int id) 
        {
            try
            {
                var theater = await _service.GetTheaterByIdAsync(id); 
                if (theater == null)
                {
                    return NotFound($"Theater with ID {id} not found.");
                }
                return Ok(theater);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving theater: {ex.Message}"); 
            }
        }

        [HttpPost("AddTheater")]
        public async Task<ActionResult<Theater>> AddTheater(Theater theater)
        {
            try
            {
                if (theater == null || string.IsNullOrEmpty(theater.Name) || string.IsNullOrEmpty(theater.Location) || theater.Capacity <= 0)
                {
                    return BadRequest("Invalid theater data.");
                }
                var addedTheater = await _service.AddTheaterAsync(theater);
                return CreatedAtAction(nameof(GetTheater), new { id = addedTheater.Id }, addedTheater);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding theater: {ex.Message}");
            }
        }

        [HttpPut("UpdateTheater")]
        public async Task<ActionResult<Theater>> UpdateTheater(Theater theater)
        {
            try
            {
                if (theater == null || theater.Id <= 0 || string.IsNullOrEmpty(theater.Name) || string.IsNullOrEmpty(theater.Location) || theater.Capacity <= 0)
                {
                    return BadRequest("Invalid theater data.");
                }
                var updatedTheater = await _service.UpdateTheaterAsync(theater);
                if (updatedTheater == null)
                {
                    return NotFound($"Theater with ID {theater.Id} not found.");
                }
                return Ok(updatedTheater);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating theater: {ex.Message}");
            }
        }
        [HttpDelete("DeleteTheater/{id}")]
        public async Task<ActionResult> DeleteTheater(int id)
        {
            try
            {
                var deleted = await _service.DeleteTheaterAsync(id);
                if (!deleted)
                {
                    return NotFound($"Theater with ID {id} not found.");
                }
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting theater: {ex.Message}");
            }
        }   
    }
}
