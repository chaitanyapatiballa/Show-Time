using BookingService.DTOs;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(PaymentLogic service) : ControllerBase
{
    private readonly PaymentLogic _service = service;

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
