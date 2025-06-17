using BusinessLogic;
using DBModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthLogic _logic;

    public AuthController(AuthLogic logic)
    {
        _logic = logic;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _logic.RegisterAsync(dto);
        if (!result)
            return BadRequest("User already exists or registration failed.");

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _logic.LoginAsync(dto);
        if (token == null)
            return Unauthorized("Invalid credentials");

        return Ok(new { Token = token });
    }
}