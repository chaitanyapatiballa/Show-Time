using BookingService.DTOs;
using BookingService.Services;
using DBModels.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;

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
                return BadRequest("Booking data is required.");

            var booking = new Booking
            {
                Userid = bookingDto.UserId,
                Movieid = bookingDto.MovieId,
                Theaterid = bookingDto.TheaterId,
                Seatnumber = bookingDto.SeatNumber,
                Showtime = bookingDto.ShowTime,
                Bookingtime = DateTime.UtcNow,
                IsCancelled = false,
                Status = "Confirmed"
            };

            var savedBooking = await _service.CreateBooking(booking);

            var paymentClient = _service.CreatePaymentServiceClient();
            var paymentRequest = new PaymentDto
            {
                BookingId = savedBooking.Bookingid,
                UserId = savedBooking.Userid,
                AmountPaid = 300.0M // default
            };

            var paymentResponse = await paymentClient.PostAsJsonAsync("/api/Payment", paymentRequest);
            if (!paymentResponse.IsSuccessStatusCode)
                return StatusCode(500, "Payment failed.");

            var payment = await paymentResponse.Content.ReadFromJsonAsync<PaymentDto>();
            savedBooking.Paymentid = payment?.PaymentId ?? 0;

            await _service.UpdateBooking(savedBooking);

            var result = await _service.GetBookingDetails(savedBooking.Bookingid);
            return Ok(result);
        }

        [HttpGet("GetBooking/{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var result = await _service.GetBookingDetails(id);
            return result == null ? NotFound("Booking not found.") : Ok(result);
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
        public async Task<IActionResult> GetBookingHistoryByUserId(int userId, [FromQuery] string? status)
        {
            try
            {
                var result = await _service.GetBookingHistoryByUserId(userId, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving booking history: {ex.Message}");
            }
        }
    }
}
