namespace Controllers.Dtos.Login;

public sealed record LoginResponse(
    string FirstName,
    string LastName,
    string Role
);
