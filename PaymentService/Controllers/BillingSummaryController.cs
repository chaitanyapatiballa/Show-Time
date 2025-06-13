using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingSummaryController : ControllerBase
    {
        private readonly BillingSummaryService _service;
        public BillingSummaryController(BillingSummaryService service) => _service = service;

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetByBookingId(int bookingId)
        {
            var result = await _service.GetByBookingIdAsync(bookingId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int bookingId, [FromQuery] int showId, [FromQuery] string paymentMethod)
        {
            var result = await _service.CreateAsync(bookingId, showId, paymentMethod);
            return Ok(result);
        }
    }
}
