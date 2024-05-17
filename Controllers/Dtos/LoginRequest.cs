using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos;

public sealed record LoginRequest(
    [Required, EmailAddress]
    string Email,

    [Required, MinLength(6), MaxLength(32)]
    string Password
);
