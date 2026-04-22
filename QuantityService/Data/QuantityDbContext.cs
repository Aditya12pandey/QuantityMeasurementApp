using Microsoft.EntityFrameworkCore;
using QuantityService.Models;

namespace QuantityService.Data;

public class QuantityDbContext : DbContext
{
    public QuantityDbContext(DbContextOptions<QuantityDbContext> options)
        : base(options)
    {
    }

    public DbSet<QuantityEntity> QuantityMeasurements => Set<QuantityEntity>();
}
