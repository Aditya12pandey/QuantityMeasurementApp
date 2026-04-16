using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementApp.API.Controllers;

/// <summary>
/// Handles user registration and login.
/// Login returns a signed JWT that must be sent as Bearer token
/// in the Authorization header for all protected endpoints.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register a new user. Password is hashed with BCrypt before storage.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Register([FromBody] RegisterRequestDto request)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and Password are required.");

        try
        {
            _authService.Register(request);
            return StatusCode(StatusCodes.Status201Created,
                new { message = $"User '{request.Username}' registered successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Login with username + password.
    /// Returns a signed JWT token valid for the configured duration.
    /// Use this token as: Authorization: Bearer {token}
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username and Password are required.");

        try
        {
            AuthResponseDto response = _authService.Login(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }
    }
}