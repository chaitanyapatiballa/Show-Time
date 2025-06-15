using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;


namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService.Services.PaymentService _paymentService;

        public PaymentController(PaymentService.Services.PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("make-payment")]
        public async Task<IActionResult> MakePayment([FromQuery] int bookingId, [FromQuery] int showId, [FromQuery] string paymentMethod = "cash")
        {
            try
            {
                var payment = await _paymentService.MakePaymentAsync(bookingId, showId, paymentMethod);
                return Ok(new PaymentDto
                {
                    PaymentId = payment.Paymentid,
                    BookingId = payment.Bookingid,
                    AmountPaid = payment.AmountPaid,
                    PaymentMethod = payment.Paymentmethod,
                    PaymentDate = payment.Paymentdate,
                    UserId = payment.Userid
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Payment failed", error = ex.Message });
            }
        }

        [HttpGet("GetPaymentByBooking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBooking([FromRoute] int bookingId)
        {
            var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
            if (payment == null)
                return NotFound();

            return Ok(new PaymentDto
            {
                PaymentId = payment.Paymentid,
                BookingId = payment.Bookingid,
                AmountPaid = payment.AmountPaid,
                PaymentMethod = payment.Paymentmethod,
                PaymentDate = payment.Paymentdate,
                UserId = payment.Userid
            });
        }
    }
}

