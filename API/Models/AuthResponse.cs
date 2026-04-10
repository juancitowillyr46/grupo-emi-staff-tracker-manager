namespace API.Controllers;

/// <summary>
/// Response returned after a successful authentication operation.
/// </summary>
public sealed record AuthResponse(string Username, string Role, string Token);
