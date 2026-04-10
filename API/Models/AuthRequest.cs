namespace API.Controllers;

/// <summary>
/// Request body used for user login.
/// </summary>
public sealed record LoginRequest(string Username, string Password);
