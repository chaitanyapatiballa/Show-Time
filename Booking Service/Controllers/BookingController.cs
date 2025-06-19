using BusinessLogic;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingLogic _logic;

        public BookingController(BookingLogic logic)
        {
            _logic = logic;
        }

        [HttpPost("bookseat")]
        public async Task<IActionResult> BookSeat([FromBody] BookingDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim.Value);
            var result = await _logic.BookSeatAsync(dto, userId);

            return result.Success
                ? Ok($"Seat booked. Amount: ₹{result.Amount}")
                : BadRequest(result.ErrorMessage);
        }

        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var success = await _logic.CancelBookingAsync(bookingId);
            return success ? Ok("Booking cancelled.") : BadRequest("Cancellation failed.");
        }

        [HttpGet("shows")]
        public async Task<IActionResult> GetShows([FromQuery] int movieId, [FromQuery] int theaterId, [FromQuery] DateOnly date)
        {
            var shows = await _logic.GetShowsAsync(movieId, theaterId, date);
            if (shows == null || shows.Count == 0)
                return NotFound("No shows found for this movie and theater on the given date.");

            return Ok(shows);
        }

        [HttpGet("seats")]
        public async Task<IActionResult> GetAvailableSeats([FromQuery] int showinstanceId)
        {
            var seats = await _logic.GetAvailableSeatsAsync(showinstanceId);
            return Ok(seats);
        }

        [HttpPost("autogenerate-next-day-shows")]
        public async Task<IActionResult> AutoGenerateNextDayShows()
        {
            await _logic.GenerateNextDayShowinstancesAsync();
            return Ok("Next day's showinstances created successfully.");
        }
    }
}
