using AuthService.Models;
using AuthService.Data;

namespace AuthService.Data;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public UserEntity? GetByUsername(string username)
        => _context.Users.FirstOrDefault(u => u.Username == username);

    public void AddUser(UserEntity user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public bool UsernameExists(string username)
        => _context.Users.Any(u => u.Username == username);
}
