using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using TheaterService.DTOs;
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
                var theaters = await _service.GetTheaters(); 
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
                var theater = await _service.GetTheaterById(id); 
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
        public async Task<ActionResult<TheaterDto>> AddTheater([FromBody] TheaterDto theaterDto)
        {
            try
            {
                if (theaterDto == null || string.IsNullOrEmpty(theaterDto.Name) || string.IsNullOrEmpty(theaterDto.Location) || theaterDto.Capacity <= 0)
                {
                    return BadRequest("Invalid theater data.");
                }

                var theater = new Theater
                {
                    Name = theaterDto.Name,
                    Location = theaterDto.Location,
                    Capacity = theaterDto.Capacity
                };

                var addedTheater = await _service.AddTheater(theater);

                var resultDto = new TheaterDto
                {
                    TheaterId = addedTheater.Theaterid,
                    Name = addedTheater.Name,
                    Location = addedTheater.Location,
                    Capacity = addedTheater.Capacity
                };

                return CreatedAtAction(nameof(GetTheater), new { id = resultDto.TheaterId }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding theater: {ex.Message}");
            }
        }


        [HttpPut("UpdateTheater")]
        public async Task<ActionResult<Theater>> UpdateTheater([FromBody] TheaterDto dto)
        {
            try
            {
                if (dto == null || dto.TheaterId <= 0 || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Location) || dto.Capacity <= 0)
                {
                    return BadRequest("Invalid theater data.");
                }

                var updated = await _service.UpdateTheater(dto);
                if (updated == null)
                {
                    return NotFound($"Theater with ID {dto.TheaterId} not found.");
                }

                return Ok(updated);
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
                var deleted = await _service.DeleteTheater(id);
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
