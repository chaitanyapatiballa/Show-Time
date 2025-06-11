using Microsoft.AspNetCore.Mvc;
using TheaterService.DTOs;
using TheaterService.Services;

namespace TheaterService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieTheaterController : ControllerBase
    {
        private readonly MovieTheaterServices _service;

        public MovieTheaterController(MovieTheaterServices service)
        {
            _service = service;
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignMovieToTheater([FromBody] MovieTheaterDto dto)
        {
            if (dto.MovieId <= 0 || dto.TheaterId <= 0)
                return BadRequest("Invalid MovieId or TheaterId.");

            var result = await _service.AssignMovieToTheaterAsync(dto);
            return Ok(result);
        }

        [HttpGet("AllAssignments")]
        public async Task<IActionResult> GetAllAssignments()
        {
            var result = await _service.GetAllAssignmentsAsync();
            return Ok(result);
        }
    }
}
