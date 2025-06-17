using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class AuthRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
