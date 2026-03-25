using System.Collections.Generic;
using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppRepository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityEntity entity);

        List<QuantityEntity> GetAll();
        List<QuantityEntity> GetAllMeasurements();
        List<QuantityEntity> GetByOperationType(string operationType);
        List<QuantityEntity> GetByMeasurementType(string measurementType);

        QuantityEntity GetById(string operationId);

        int  GetCount();
        void Clear();

        // Default implementations — overridden by DB repository
        string GetPoolStatistics() => "N/A (cache)";
        void   ReleaseResources()  { }
    }
}