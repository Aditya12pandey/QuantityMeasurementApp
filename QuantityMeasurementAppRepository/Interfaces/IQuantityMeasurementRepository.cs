using System.Collections.Generic;
using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppRepository.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityEntity entity);
        List<QuantityEntity> GetAll();
        List<QuantityEntity> GetAllMeasurements();
        QuantityEntity GetById(string operationId);
        List<QuantityEntity> GetByOperationType(string operationType);
        int GetCount();
        void Clear();
    }
}