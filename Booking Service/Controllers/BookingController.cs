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

            var enriched = await _service.GetBookingDetails(saved.Id);
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
    }
}