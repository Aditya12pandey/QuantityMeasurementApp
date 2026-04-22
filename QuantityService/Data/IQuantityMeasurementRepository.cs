using QuantityService.Models;

namespace QuantityService.Data;

public interface IQuantityMeasurementRepository
{
    void Save(QuantityEntity entity);
    List<QuantityEntity> GetAllMeasurements();
    List<QuantityEntity> GetByOperationType(string operationType);
    List<QuantityEntity> GetByMeasurementType(string measurementType);
    int GetCount();
}
