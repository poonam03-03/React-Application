using AIMeetingIntelligence.API.Data;
using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;
using AIMeetingIntelligence.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIMeetingIntelligence.API.Repositories;

public class AuthRepository(AppDbContext context) : IAuthRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}

