using Booking_Service.DTOs;
using Booking_Service.Models;
using Booking_Service.Services;
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
            if (dto == null || !ModelState.IsValid)
                return BadRequest("Invalid booking data");

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
                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id },
                    new { Message = "Booking created successfully", Booking = createdBooking });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid booking ID");

            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound();

                return Ok(booking);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the booking");
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid booking ID");

            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null || booking.IsCancelled)
                    return NotFound($"Booking with ID {id} not found or already cancelled");

                await _bookingService.CancelBookingAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while cancelling the booking");
            }
        }
    }
}
