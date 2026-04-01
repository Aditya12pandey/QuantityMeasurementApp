using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Interfaces;
using QuantityMeasurementAppBusiness.Interfaces;

namespace QuantityMeasurementAppBusiness.Implementations;

/// <summary>
/// Implements registration and login with:
///   - BCrypt password hashing  (Hashing Algorithms)
///   - HMAC-SHA256 signed JWT   (JWT Security)
/// </summary>
public class AuthServiceImpl : IAuthService
{
    private readonly IUserRepository  _userRepo;
    private readonly IConfiguration   _config;

    public AuthServiceImpl(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config   = config;
    }

    // ─────────────────────────────────────────────
    //  REGISTER
    // ─────────────────────────────────────────────
    public void Register(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Username and Password are required.");

        if (_userRepo.UsernameExists(request.Username))
            throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

        // 🔐 Hashing: BCrypt with work factor 12
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);

        var user = new UserEntity(request.Username, passwordHash);
        _userRepo.AddUser(user);
    }

    // ─────────────────────────────────────────────
    //  LOGIN
    // ─────────────────────────────────────────────
    public AuthResponseDto Login(LoginRequestDto request)
    {
        var user = _userRepo.GetByUsername(request.Username)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        // 🔐 BCrypt verify — compares plain password against stored hash
        bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid)
            throw new UnauthorizedAccessException("Invalid username or password.");

        // 🔐 JWT generation
        var token  = GenerateJwt(user);
        var expiry = DateTime.UtcNow.AddMinutes(GetTokenLifetimeMinutes());

        return new AuthResponseDto
        {
            Token    = token,
            Username = user.Username,
            Role     = user.Role,
            Expiry   = expiry
        };
    }

    // ─────────────────────────────────────────────
    //  JWT Helper
    // ─────────────────────────────────────────────
    private string GenerateJwt(UserEntity user)
    {
        var jwtSection = _config.GetSection("Jwt");
        var secretKey  = jwtSection["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
        var issuer     = jwtSection["Issuer"]   ?? "QuantityMeasurementApp";
        var audience   = jwtSection["Audience"] ?? "QuantityMeasurementApp";
        int minutes    = GetTokenLifetimeMinutes();

        var keyBytes  = Encoding.UTF8.GetBytes(secretKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Claims embedded inside the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.UserId),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role,               user.Role),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer:             issuer,
            audience:           audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private int GetTokenLifetimeMinutes()
    {
        var raw = _config["Jwt:TokenLifetimeMinutes"];
        return int.TryParse(raw, out int parsed) ? parsed : 60;
    }
}