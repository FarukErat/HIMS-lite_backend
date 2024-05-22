using Enums;

namespace Controllers.Dtos.AssignRole;

public sealed record AssignRoleRequest(string Email, string Role);
