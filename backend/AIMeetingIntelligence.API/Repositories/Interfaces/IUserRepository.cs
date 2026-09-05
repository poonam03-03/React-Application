// AIMeetingIntelligence.API/Repositories/IUserRepository.cs
using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;

namespace AIMeetingIntelligence.API.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User> RegisterAsync(RegisterDto dto);
}
