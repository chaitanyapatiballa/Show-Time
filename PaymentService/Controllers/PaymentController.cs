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

            bool success = await _service.ProcessPaymentAsync(payment);
            if (!success)
                return BadRequest("Payment failed");

            return Ok(payment);
        }
    }
}
