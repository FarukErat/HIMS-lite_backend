using Controllers.Dtos;
using Entities;
using Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController(
    UserRepository userRepository,
    PasswordHasher passwordHasher
) : ControllerBase
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly PasswordHasher _passwordHasher = passwordHasher;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        User user = new()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            PasswordHash = _passwordHasher.HashPassword(registerRequest.Password),
            Roles = 1 << (int)Role.Unverified,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        return Ok(new { message = "User registered" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        User? user = await _userRepository.FindByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            return NotFound(new { message = "User not found" });
        }

        if (!_passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            return BadRequest(new { message = "Invalid password" });
        }

        return Ok(new { message = "User logged in" });
    }
}
