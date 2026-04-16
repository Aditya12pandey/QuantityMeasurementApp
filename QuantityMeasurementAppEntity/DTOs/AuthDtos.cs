namespace QuantityMeasurementAppEntity.DTOs;

/// <summary>Request body for user registration.</summary>
public class RegisterRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

/// <summary>Request body for user login.</summary>
public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

/// <summary>Response returned after successful login — contains the JWT token.</summary>
public class AuthResponseDto
{
    public string Token     { get; set; }
    public string Username  { get; set; }
    public string Role      { get; set; }
    public DateTime Expiry  { get; set; }
}