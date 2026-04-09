using Application.Contracts;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserStore : IUserStore
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "Admin",
        "User"
    };

    private readonly AppDbContext _context;

    public UserStore(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(string username, string password, string role)
    {
        if (!AllowedRoles.Contains(role))
        {
            return false;
        }

        var exists = await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
        if (exists)
        {
            return false;
        }

        var user = new User(username, PasswordHasher.Hash(password), role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> ValidateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        if (user is null)
        {
            return null;
        }

        return PasswordHasher.Verify(password, user.PasswordHash) ? user : null;
    }
}
