using AIMeetingIntelligence.API.DTOs;
using AIMeetingIntelligence.API.Services;
using Microsoft.AspNetCore.Mvc;
using AIMeetingIntelligence.API.Repositories.Interfaces;

namespace AIMeetingIntelligence.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtService _jwt;

    public AuthController(IAuthRepository authRepository, JwtService jwt)
    {
        _authRepository = authRepository;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _authRepository.EmailExistsAsync(dto.Email))
            return BadRequest("Email already exists.");

        await _authRepository.RegisterAsync(dto);

        return Ok("User Registered Successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _authRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized();

        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!valid)
            return Unauthorized();

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            token,
            user.FullName,
            user.Email
        });
    }
}
