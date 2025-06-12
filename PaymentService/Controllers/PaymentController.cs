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
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] PaymentDto request)
        {
            // Validate Booking exists
            if (!_context.Bookings.Any(b => b.BookingId == request.BookingId))
                return NotFound("Booking not found");

            // Create payment
            var payment = new Payment
            {
                BookingId = request.BookingId,
                UserId = request.UserId,
                Amount = request.Amount,
                PaymentTime = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Return with populated fields
            return Ok(new PaymentDto
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                PaymentTime = payment.PaymentTime
            });
        }
    }
}

