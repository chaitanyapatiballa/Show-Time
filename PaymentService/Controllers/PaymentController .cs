﻿using DBModels.Dto;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using PaymentService.Logic;

namespace Payment_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentLogic _logic;

    public PaymentController(PaymentLogic logic)
    {
        _logic = logic;
    }

    [HttpPost("billing-summary")]
    public async Task<IActionResult> AddSummary(BillingsummaryDto dto)
    {
        var result = await _logic.CreateSummaryAsync(dto);
        return Ok(result);
    }

    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment(PaymentDto dto)
    {
        var result = await _logic.MakePaymentAsync(dto);
        if (result == null)
            return BadRequest("Payment failed.");

        return Ok(result);
    }

    [HttpGet("user/{userId}/payments")]
    public async Task<IActionResult> GetUserPayments(int userId)
    {
        var payments = await _logic.GetPaymentsByUserAsync(userId);
        return Ok(payments);
    }
}