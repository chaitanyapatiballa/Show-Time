using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic;

public class AuthLogic(UserRepository userRepo, IConfiguration config)
{
    private readonly UserRepository _userRepo = userRepo;
    private readonly IConfiguration _config = config;

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepo.GetUserByEmailAsync(dto.Email) != null)
            return false;

        using var hmac = new HMACSHA256();
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))),
            Role = dto.Role ?? "User"
        };

        await _userRepo.AddUserAsync(user);
        return true;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetUserByEmailAsync(dto.Email);
        if (user == null)
            return null;

        using var hmac = new HMACSHA256();
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        var storedHash = Convert.FromBase64String(user.PasswordHash);

        if (!storedHash.SequenceEqual(computedHash))
            return null;

        return GenerateJwtToken(user);
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