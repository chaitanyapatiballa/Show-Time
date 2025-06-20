using AuthService.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic;

public class AuthLogic(AuthRepository userRepo, IConfiguration config)
{
    private readonly AuthRepository _userRepo = userRepo;
    private readonly IConfiguration _config = config;

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        try
        {
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Passwordhash = hashedPassword,
                Role = dto.Role ?? "User"
            };

            await _userRepo.AddUserAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        try
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash))
                return null;

            return GenerateJwtToken(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoginAsync: {ex.Message}");
            return null;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
