using System.ComponentModel.DataAnnotations;

namespace Controllers.Dtos;

public sealed record RegisterRequest(
    [Required]
    string FirstName,

    [Required]
    string LastName,

    [EmailAddress, Required]
    string Email,

    [Required, MinLength(6), MaxLength(32)]
    string Password
);
