using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register()
    {
        return Ok(new { message = "Register" });
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok(new { message = "Login" });
    }
}
