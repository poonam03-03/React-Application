using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Models;

namespace AIMeetingIntelligence.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User> RegisterAsync(RegisterDto dto);
    }
}
