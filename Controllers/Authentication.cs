using System.Web.Http.Cors;
using Configuration;
using Controllers.Dtos.Login;
using Controllers.Dtos.Register;
using Entities;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using Services;

namespace Controllers;

public sealed class AuthenticationController(
    UserRepository userRepository,
    PasswordHasher passwordHasher,
    SessionRepository sessionRepository
) : BaseController
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly PasswordHasher _passwordHasher = passwordHasher;
    private readonly SessionRepository _sessionRepository = sessionRepository;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        User? existingUser = await _userRepository.FindByEmailAsync(registerRequest.Email);
        if (existingUser is not null)
        {
            return Conflict(new { message = "User already exists" });
        }

        uint role = 1 << (int)Role.Unverified;
        #region REMOVE THIS REGION BEFORE PRODUCTION
        // consider checking whether environment is development and can work fine in docker
        if (registerRequest.Email == "admin@admin.com"
            && registerRequest.Password == "adminadmin")
        {
            role = 1 << (int)Role.Admin;
        }
        #endregion

        User user = new()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            PasswordHash = _passwordHasher.HashPassword(registerRequest.Password),
            Roles = role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        return Ok(new { message = "User registered" });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
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

        // ---- user is authenticated ----

        Session newSession = new()
        {
            UserId = user.Id,
            Email = user.Email,
            Roles = user.Roles.ToRoleList(),
            IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            UserAgent = Request.Headers.UserAgent.ToString(),
        };

        Guid sessionId;
        Session? existingSession = await _sessionRepository.GetSessionByUserIdAsync(user.Id);
        if (existingSession is null)
        {
            sessionId = await _sessionRepository.SaveSessionAsync(newSession);
        }
        else
        {
            sessionId = existingSession.Id;
            newSession.CreatedAt = existingSession.CreatedAt;
            newSession.ExpireAt = existingSession.ExpireAt;
            await _sessionRepository.UpdateSessionByIdAsync(existingSession.Id, newSession);
        }

        Response.Cookies.Append(Configurations.SessionIdCookieKey, sessionId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTime.UtcNow + Configurations.SessionExpiry
        });

        return Ok(new LoginResponse(
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Role: user.Roles.ToRoleList().First().ToString()
        ));
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue(Configurations.SessionIdCookieKey, out string? sessionId))
        {
            return BadRequest(new { message = "Session not found" });
        }

        if (!Guid.TryParse(sessionId, out Guid sessionGuid))
        {
            return BadRequest(new { message = "Invalid session" });
        }

        await _sessionRepository.DeleteSessionByIdAsync(sessionGuid);

        Response.Cookies.Delete(Configurations.SessionIdCookieKey);

        return Ok(new { message = "User logged out" });
    }
}
