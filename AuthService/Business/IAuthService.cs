using AuthService.Models;

namespace AuthService.Business;

public interface IAuthService
{
    void Register(RegisterRequestDto request);
    AuthResponseDto Login(LoginRequestDto request);
}
