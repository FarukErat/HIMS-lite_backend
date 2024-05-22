namespace Controllers.Dtos.Login;

public sealed record LoginResponse(
    string Email,
    string FirstName,
    string LastName,
    string Role
);
