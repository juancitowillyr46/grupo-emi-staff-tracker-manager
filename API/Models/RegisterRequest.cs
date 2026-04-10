namespace API.Controllers;

/// <summary>
/// Request body used for user registration.
/// </summary>
public sealed record RegisterRequest(string Username, string Password, string Role);
