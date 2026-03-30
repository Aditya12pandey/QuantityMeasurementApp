using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Data;
using QuantityMeasurementAppRepository.Exceptions;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementAppRepository.Repository;

public class QuantityMeasurementEfRepository : IQuantityMeasurementRepository
{
    private readonly QuantityMeasurementDbContext _db;

    public QuantityMeasurementEfRepository(QuantityMeasurementDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public void Save(QuantityEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            var existing = _db.QuantityMeasurements.Find(entity.OperationId);
            if (existing != null)
                _db.Entry(existing).CurrentValues.SetValues(entity);
            else
                _db.QuantityMeasurements.Add(entity);

            _db.SaveChanges();
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            throw new DatabaseException("Save failed: " + ex.Message, ex);
        }
    }

    public List<QuantityEntity> GetAll()
    {
        try
        {
            return _db.QuantityMeasurements
                .AsNoTracking()
                .OrderBy(x => x.Timestamp)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Query failed: " + ex.Message, ex);
        }
    }

    public List<QuantityEntity> GetAllMeasurements() => GetAll();

    public QuantityEntity? GetById(string operationId)
    {
        try
        {
            return _db.QuantityMeasurements
                .AsNoTracking()
                .FirstOrDefault(x => x.OperationId == operationId);
        }
        catch (Exception ex)
        {
            throw new DatabaseException("GetById failed: " + ex.Message, ex);
        }
    }

    public List<QuantityEntity> GetByOperationType(string operationType)
    {
        try
        {
            return _db.QuantityMeasurements
                .AsNoTracking()
                .Where(x => x.OperationType == operationType)
                .OrderBy(x => x.Timestamp)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new DatabaseException(
                "GetByOperationType failed: " + ex.Message, ex);
        }
    }

    public List<QuantityEntity> GetByMeasurementType(string measurementType)
    {
        try
        {
            return _db.QuantityMeasurements
                .AsNoTracking()
                .Where(x => x.Operand1Measurement == measurementType)
                .OrderBy(x => x.Timestamp)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new DatabaseException(
                "GetByMeasurementType failed: " + ex.Message, ex);
        }
    }

    public int GetCount()
    {
        try
        {
            return _db.QuantityMeasurements.Count();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("GetCount failed: " + ex.Message, ex);
        }
    }

    public void Clear()
    {
        try
        {
            _db.QuantityMeasurements.RemoveRange(_db.QuantityMeasurements);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Clear failed: " + ex.Message, ex);
        }
    }

    public string GetPoolStatistics() => "EF Core (SQL Server)";

    /// <summary>Console host may call this; web host relies on DI to dispose <see cref="DbContext"/>.</summary>
    public void ReleaseResources()
    {
    }
}
