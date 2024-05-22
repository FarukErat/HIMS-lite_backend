using System.Web.Http.Cors;
using Configuration;
using Controllers.Dtos.AssignRole;
using Entities;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using Security;

namespace Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors(origins: "*", headers: "*", methods: "*")]
public sealed class AdminPanelController(
    UserRepository userRepository
) : ControllerBase
{
    private readonly UserRepository _userRepository = userRepository;

    [HttpGet("users")]
    [SessionAuth(Role.Admin)]
    public async Task<IActionResult> GetUsers()
    {
        List<User> users = await _userRepository.GetAllAsync();

        Session? session = HttpContext.Items[Configurations.SessionItemsKey] as Session;
        string? email = session?.Email;

        if (email is not null)
        {
            // Remove the current user from the list
            users.RemoveAll(u => u.Email == email);
        }

        uint firstRole;
        return Ok(users.Select(user =>
        {
            for (firstRole = 0; firstRole < 32; firstRole++)
            {
                if ((user.Roles & (1 << (int)firstRole)) != 0)
                {
                    break;
                }
            }
            return new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                Role = ((Role)firstRole).ToString()
            };
        }));
    }

    [HttpPost("assign-role")]
    [SessionAuth(Role.Admin)]
    public async Task<IActionResult> AssignRoleByEmail(AssignRoleRequest assignRoleRequest)
    {
        User? user = await _userRepository.FindByEmailAsync(assignRoleRequest.Email);
        if (user is null)
        {
            return NotFound(new { message = "User not found" });
        }

        if (!Enum.TryParse(assignRoleRequest.Role, true, out Role role))
        {
            return BadRequest(new { message = "Invalid role" });
        }

        user.Roles = (uint)(1 << (int)role);
        await _userRepository.UpdateAsync(user);

        return Ok(new { message = "Role assigned" });
    }
}
