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
        public async Task<IActionResult> BookSeat(BookingDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token");

                int userId = int.Parse(userIdClaim.Value);
                var (success, amount, errorMessage) = await _logic.BookSeatAsync(dto, userId);

                return success
                    ? Ok("Seat booked. Amount: ₹{amount}")
                    : BadRequest(errorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while booking the seat: {ex.Message}");
            }
        }

        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                var success = await _logic.CancelBookingAsync(bookingId);
                return success ? Ok("Booking cancelled.") : BadRequest("Cancellation failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while cancelling the booking: {ex.Message}");
            }
        }

        [HttpGet("shows")]
        public async Task<IActionResult> GetShows(int movieId, int theaterId, DateOnly date)
        {
            try
            {
                var shows = await _logic.GetShowsAsync(movieId, theaterId, date);
                if (shows == null || shows.Count == 0)
                    return NotFound("No shows found for this movie and theater on the given date.");

                return Ok(shows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching shows: {ex.Message}");
            }
        }

        [HttpGet("seats")]
        public async Task<IActionResult> GetAvailableSeats(int showinstanceId)
        {
            try
            {
                var seats = await _logic.GetAvailableSeatsAsync(showinstanceId);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching seats: {ex.Message}");
            }
        }

        [HttpPost("autogenerate-next-day-shows")]
        public async Task<IActionResult> AutoGenerateNextDayShows()
        {
            try
            {
                await _logic.GenerateNextDayShowinstancesAsync();
                return Ok("Next day's showinstances created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating shows: {ex.Message}");
            }
        }
    }
}
