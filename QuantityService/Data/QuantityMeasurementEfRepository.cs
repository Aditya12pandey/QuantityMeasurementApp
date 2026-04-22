using Microsoft.EntityFrameworkCore;
using QuantityService.Models;

namespace QuantityService.Data;

public class QuantityMeasurementEfRepository : IQuantityMeasurementRepository
{
    private readonly QuantityDbContext _db;

    public QuantityMeasurementEfRepository(QuantityDbContext db)
    {
        _db = db;
    }

    public void Save(QuantityEntity entity)
    {
        _db.QuantityMeasurements.Add(entity);
        _db.SaveChanges();
    }

    public List<QuantityEntity> GetAllMeasurements()
    {
        return _db.QuantityMeasurements
            .AsNoTracking()
            .OrderBy(x => x.Timestamp)
            .ToList();
    }

    public List<QuantityEntity> GetByOperationType(string operationType)
    {
        return _db.QuantityMeasurements
            .AsNoTracking()
            .Where(x => x.OperationType == operationType)
            .OrderBy(x => x.Timestamp)
            .ToList();
    }

    public List<QuantityEntity> GetByMeasurementType(string measurementType)
    {
        return _db.QuantityMeasurements
            .AsNoTracking()
            .Where(x => x.Operand1Measurement == measurementType)
            .OrderBy(x => x.Timestamp)
            .ToList();
    }

    public int GetCount()
    {
        return _db.QuantityMeasurements.Count();
    }
}
