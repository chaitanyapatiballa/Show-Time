using Booking_Service.Controllers;
using BookingService.DTOs;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using DBModels.Dto;

namespace BookingService.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase 
    {
        private readonly BookingLogic _service;

        public BookingController(BookingLogic service) 
        {
            _service = service;
        }

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
