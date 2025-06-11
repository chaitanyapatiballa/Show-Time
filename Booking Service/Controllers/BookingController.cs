
using BookingService.Services;
using DBModels.Db;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingServices _service;

        public BookingController(BookingServices service)
        {
            _service = service;
        }

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            var result = await _service.CreateBookingWithDetailsAsync(booking);
            if (result == null)
                return BadRequest("Booking failed or invalid references.");

            return Ok(result);
        }

        [HttpGet("GetBooking/{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _service.GetBookingDetailsAsync(id);
            if (result == null)
                return NotFound("Booking not found.");

            return Ok(result);
        }
    }
}
