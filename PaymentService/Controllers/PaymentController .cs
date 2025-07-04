﻿using DBModels.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using PaymentService.Logic;
using System.Security.Claims;

namespace PaymentService.Controllers;

[Authorize] 
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
        try
        {
            var result = await _logic.CreateSummaryAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("make-payment")]
    public async Task<IActionResult> MakePayment(PaymentDto dto)
    {
        try
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Unauthorized("User is not authenticated");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User not found in token");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _logic.MakePaymentAsync(dto, userId);
            if (result == null)
                return BadRequest("Payment failed.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("user/{userId}/payments")]
    public async Task<IActionResult> GetUserPayments(int userId)
    {
        try
        {
            var payments = await _logic.GetPaymentsByUserAsync(userId);
            return Ok(payments);
        }
        catch (Exception ex)
        {
           
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
