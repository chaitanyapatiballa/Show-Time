using Booking_Service.DTOs;
using Booking_Service.Services;
using DBModels.Db;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingServices _bookingService;

        public BookingController(BookingServices bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto dto)
        {
            if (dto == null)
                return BadRequest("Booking data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = new Booking
            {
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId,
                SeatNumber = dto.SeatNumber,
                UserId = dto.UserId,
                BookingTime = DateTime.UtcNow,
                IsCancelled = false
            };

            try
            {
                var createdBooking = await _bookingService.AddBooking(booking);
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, createdBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the booking: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid booking ID.");

            try
            {
                var booking = await _bookingService.GetBookingDetailsAsync(id);
                if (booking == null)
                    return NotFound($"Booking with ID {id} not found.");

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the booking: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid booking ID.");

            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound($"Booking with ID {id} not found.");

                if (booking.IsCancelled)
                    return BadRequest("Booking is already cancelled.");

                await _bookingService.CancelBookingAsync(id);
                return Ok(new { Message = "Booking cancelled successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while cancelling the booking: {ex.Message}");
            }
        }
    }
}
