using Booking_Service.DTOs;
using Booking_Service.HttpClients;
using Booking_Service.Services;
using DBModels.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Booking_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingServices _bookingService;
        private readonly IMovieServiceClient _movieService;
        private readonly ITheaterServiceClient _theaterService;
        private readonly IPaymentServiceClient _paymentService;

        public BookingController(
            BookingServices bookingService,
            IMovieServiceClient movieService,
            ITheaterServiceClient theaterService,
            IPaymentServiceClient paymentService)
        {
            _bookingService = bookingService;
            _movieService = movieService;
            _theaterService = theaterService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto dto)
        {
            if (dto == null)
                return BadRequest("Booking data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Validate Movie and Theater existence
                var movieResult = await _movieService.GetMovieAsync(dto.MovieId);
                var theaterResult = await _theaterService.GetTheaterAsync(dto.TheaterId);

                if (movieResult == null || theaterResult == null)
                    return BadRequest("Invalid Movie or Theater ID.");

                // Process payment (simulate external service)
                var paymentResponse = await _paymentService.ProcessPaymentAsync(dto.PaymentId);
                if (string.IsNullOrEmpty(paymentResponse))
                    return BadRequest("Payment processing failed.");

                // Map BookingDto to Booking entity
                var booking = new Booking
                {
                    MovieId = dto.MovieId,
                    TheaterId = dto.TheaterId,
                    SeatNumber = dto.SeatNumber,
                    UserId = dto.UserId,
                    PaymentId = dto.PaymentId,
                    BookingTime = DateTime.UtcNow,
                    IsCancelled = false
                };

                // Fix: Pass BookingDto instead of Booking entity
                var createdBooking = await _bookingService.AddBooking(dto);

                return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, new
                {
                    Message = "Booking created successfully.",
                    Booking = createdBooking
                });
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
                var booking = await _bookingService.GetBookingByIdAsync(id);
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
