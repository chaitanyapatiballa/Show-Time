// Logic/AuthLogic.cs

using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Logic;

public class AuthLogic(AppDbContext context, IConfiguration config)
{
}
    
