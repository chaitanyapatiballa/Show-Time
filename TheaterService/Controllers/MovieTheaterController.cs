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

        [HttpPost("assign")]
        public async Task<IActionResult> AssignMovieToTheater([FromBody] MovieTheaterDto dto)
        {
            if (dto.MovieId <= 0 || dto.TheaterId <= 0)
                return BadRequest("Invalid movie or theater ID.");

            var result = await _service.AssignMovieToTheater(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await _service.GetAllAssignments();
            return Ok(assignments);
        }
    }
}