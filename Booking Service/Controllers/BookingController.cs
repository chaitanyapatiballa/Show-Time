using BookingService.DTOs;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Service.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController(BookingLogic service) : ControllerBase
    {
        private readonly BookingLogic _service = service;

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking(BookingDto dto)
        {
            var result = await _service.CreateBookingAsync(dto);
            if (result == null)
                return BadRequest("Invalid Movie or Theater ID.");

            return Ok(result);
        }

        [HttpPost("billing-summary")]
        public async Task<IActionResult> AddSummary(BillingsummaryDto dto)
        {
            var result = await _service.CreateSummaryAsync(dto);
            return Ok(result);
        }

        [HttpPost("make-payment")]
        public async Task<IActionResult> MakePayment(PaymentDto dto)
        {
            var result = await _service.MakePaymentAsync(dto);
            if (result == null)
                return BadRequest("Payment failed.");

            return Ok(result);
        }
    }

}
