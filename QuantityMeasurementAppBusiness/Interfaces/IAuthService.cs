using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementAppBusiness.Interfaces;
/// <summary>
/// Contract for authentication operations:
/// - Register: hash password with BCrypt, persist user
/// - Login:    verify BCrypt hash, generate signed JWT
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user. Hashes the password using BCrypt before saving.
    /// Throws InvalidOperationException if username already exists.
    /// </summary>
    void Register(RegisterRequestDto request);

    /// <summary>
    /// Validates credentials. Returns a signed JWT on success.
    /// Throws UnauthorizedAccessException if credentials are invalid.
    /// </summary>
    AuthResponseDto Login(LoginRequestDto request);
}