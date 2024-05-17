using System.Text.Json;
using Enums;

namespace Entities;

public sealed class User
{
    // Primary Key
    public Guid Id { get; set; }

    // Properties
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public uint Roles { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class UIntExtensions
{
    public static string ToRolesJson(this uint number)
    {
        List<string> roles = [];

        for (int i = 0; i < 32; i++)
        {
            if ((number & (1 << i)) != 0)
            {
                roles.Add(((Role)i).ToString());
            }
        }

        return JsonSerializer.Serialize(roles);
    }
}
