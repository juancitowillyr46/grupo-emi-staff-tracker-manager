namespace API.Controllers;

public sealed record AuthRequest(string Username, string Password, string Role);
