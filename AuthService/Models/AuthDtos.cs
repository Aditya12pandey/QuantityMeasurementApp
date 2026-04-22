namespace AuthService.Models;

public class RegisterRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token     { get; set; }
    public string Username  { get; set; }
    public string Role      { get; set; }
    public DateTime Expiry  { get; set; }
}
