using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos.Register;

public sealed record RegisterRequest(
    [Required]
    string FirstName,

    [Required]
    string LastName,

    [Required, EmailAddress]
    string Email,

    [Required, MinLength(6), MaxLength(32)]
    string Password
);
