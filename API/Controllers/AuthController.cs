using Application.Contracts;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserStore _userStore;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IUserStore userStore, IJwtTokenService jwtTokenService)
    {
        _userStore = userStore;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Registers a new user and returns a JWT token.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!await _userStore.RegisterAsync(request.Username, request.Password, request.Role))
        {
            return Conflict("User already exists.");
        }

        var token = _jwtTokenService.CreateToken(request.Username, request.Role);
        return Ok(new AuthResponse(request.Username, request.Role, token));
    }

    /// <summary>
    /// Validates the user credentials and returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userStore.ValidateAsync(request.Username, request.Password);
        if (user is null)
        {
            return Unauthorized();
        }

        var token = _jwtTokenService.CreateToken(user.Username, user.Role);
        return Ok(new AuthResponse(user.Username, user.Role, token));
    }
}
