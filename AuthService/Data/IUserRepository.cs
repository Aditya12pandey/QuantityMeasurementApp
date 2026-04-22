using AuthService.Models;

namespace AuthService.Data;

public interface IUserRepository
{
    UserEntity? GetByUsername(string username);
    void AddUser(UserEntity user);
    bool UsernameExists(string username);
}
