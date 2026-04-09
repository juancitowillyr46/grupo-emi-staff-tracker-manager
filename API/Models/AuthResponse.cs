namespace API.Controllers;

public sealed record AuthResponse(string Username, string Role, string Token);
