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
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var success = await _logic.RegisterAsync(dto);
            if (!success)
                return BadRequest("User already exists or registration failed.");

            return Ok("Registration successful");
        }
        catch (Exception )
        {
              return StatusCode(500, "An error occurred during registration.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var token = await _logic.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
        catch (Exception )
        {
             return StatusCode(500, "An error occurred during login.");
        }
    }
}
