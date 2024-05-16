namespace Entities;

public sealed class User
{
    // Primary Key
    Guid Id { get; set; }

    // Properties
    string Name { get; set; } = string.Empty;
    string Email { get; set; } = string.Empty;
    string PasswordHash { get; set; } = string.Empty;
    uint Roles { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
