using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppRepository.Data;

public class QuantityMeasurementDbContext : DbContext
{
    public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
        : base(options)
    {
    }

    public DbSet<QuantityEntity> QuantityMeasurements => Set<QuantityEntity>();

    public DbSet<UserEntity> Users => Set<UserEntity>();
}