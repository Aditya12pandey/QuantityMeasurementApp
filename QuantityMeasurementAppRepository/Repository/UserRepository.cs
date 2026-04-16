using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Data;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementAppRepository.Repository;

/// <summary>
/// EF Core implementation of IUserRepository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly QuantityMeasurementDbContext _context;

    public UserRepository(QuantityMeasurementDbContext context)
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