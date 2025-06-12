using DBModels.Db;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServices _service;

        public PaymentController(PaymentServices service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] PaymentDto dto)
        {
            var payment = new Payment
            {
                BookingId = dto.BookingId,
                UserId = dto.UserId,
                Amount = dto.Amount
            };

            bool success = await _service.ProcessPayment(payment);
            if (!success)
                return BadRequest("Payment failed");

            var result = new PaymentDto
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentTime = payment.PaymentTime,
                IsSuccessful = payment.IsSuccessful
            };

            return Ok(result);
        }
    }
}
