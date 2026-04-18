using System.Collections.Generic;
using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppRepository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityEntity entity);

        List<QuantityEntity> GetAll();
        List<QuantityEntity> GetAllMeasurements(string userId);
        List<QuantityEntity> GetByOperationType(string userId, string operationType);
        List<QuantityEntity> GetByMeasurementType(string userId, string measurementType);

        QuantityEntity? GetById(string operationId);

        int  GetCount(string userId);
        void Clear();

        // Default implementations — overridden by DB repository
        string GetPoolStatistics() => "N/A (cache)";
        void   ReleaseResources()  { }
    }
}