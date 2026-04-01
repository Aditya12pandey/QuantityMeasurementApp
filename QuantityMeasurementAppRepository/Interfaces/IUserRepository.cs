using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppRepository.Interfaces;

/// <summary>
/// Repository contract for user persistence operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>Find a user by username. Returns null if not found.</summary>
    UserEntity? GetByUsername(string username);

    /// <summary>Persist a new user to the database.</summary>
    void AddUser(UserEntity user);

    /// <summary>Check whether a username already exists.</summary>
    bool UsernameExists(string username);
}