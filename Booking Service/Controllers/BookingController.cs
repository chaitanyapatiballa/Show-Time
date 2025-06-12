using BookingService.DTOs;
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
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
        {
            if (bookingDto == null)
                return BadRequest("bookingDto is required.");

            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                MovieId = bookingDto.MovieId,
                TheaterId = bookingDto.TheaterId,
                PaymentId = bookingDto.PaymentId,
                SeatNumber = bookingDto.SeatNumber,
                BookingTime = bookingDto.BookingTime,
                IsCancelled = bookingDto.IsCancelled,
                Status = bookingDto.Status
            };

            var saved = await _service.CreateBooking(booking);
            var enriched = await _service.GetBookingDetails(saved.BookingId);
            return Ok(enriched);
        }

        [HttpGet("GetBooking/{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _service.GetBookingDetails(id);
            if (result == null)
                return NotFound("Booking not found.");

            return Ok(result);
        }

        [HttpGet("available-seats")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableSeats(int movieId, int theaterId, DateTime showTime)
        {
            try
            {
                var seats = await _service.GetAvailableSeats(movieId, theaterId, showTime);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching available seats: {ex.Message}");
            }
        }

        [HttpGet("GetBookingHistoryByUserId")]
        public async Task<IActionResult> GetBookingHistoryByUserId(
            int userId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? status)
        {
            try
            {
                var result = await _service.GetBookingHistoryByUserId(userId, startDate, endDate, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving booking history: {ex.Message}");
            }
        }
    }
}
