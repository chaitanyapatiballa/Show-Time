using BookingService.DTOs;
using BookingService.Services;
using DBModels.Db;
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

            //  Create Booking object (no PaymentId here)
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                MovieId = bookingDto.MovieId,
                TheaterId = bookingDto.TheaterId,
                SeatNumber = bookingDto.SeatNumber,
                ShowTime = bookingDto.ShowTime,
                BookingTime = DateTime.UtcNow,
                IsCancelled = false,
                Status = "Confirmed"
            };

            //  Save booking in DB
            var savedBooking = await _service.CreateBooking(booking);

            //  Call PaymentService
            var paymentClient = _service.CreatePaymentServiceClient();
            var paymentRequest = new PaymentDto
            {
                BookingId = savedBooking.BookingId,
                UserId = savedBooking.UserId, 
                AmountPaid = 300.0M  
            };

            var paymentResponse = await paymentClient.PostAsJsonAsync("/api/Payment", paymentRequest);
            if (!paymentResponse.IsSuccessStatusCode)
                return StatusCode(500, "Payment failed.");

            var payment = await paymentResponse.Content.ReadFromJsonAsync<PaymentDto>();

            //  Update booking with payment id
            savedBooking.PaymentId = payment.PaymentId;
            await _service.UpdateBooking(savedBooking);

            //  Return final booking with payment info
            var result = await _service.GetBookingDetails(savedBooking.BookingId);
            return Ok(result);
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
        public async Task<IActionResult> GetBookingHistoryByUserId( int userId, [FromQuery] string? status)
            //[FromQuery] DateTime? startDate,
            //[FromQuery] DateTime? endDate,
            
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
