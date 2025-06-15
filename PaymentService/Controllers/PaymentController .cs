using BookingService.DTOs;
using Microsoft.AspNetCore.Mvc;


using PaymentService.Services;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;

    public PaymentController(IPaymentService service)
    {
        _service = service;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> Pay([FromBody] PaymentDto dto)
    {
        var result = await _service.ProcessPaymentAsync(dto);
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPayments(int userId)
    {
        var payments = await _service.GetUserPaymentsAsync(userId);
        return Ok(payments);
    }
}
