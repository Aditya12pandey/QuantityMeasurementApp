using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Entity;

namespace QuantityMeasurementAppEntity.Mappings;

public static class QuantityEntityMappings
{
    public static QuantityMeasurementRecordDto ToRecordDto(this QuantityEntity entity)
    {
        if (entity == null) return null;

        return new QuantityMeasurementRecordDto
        {
            OperationId = entity.OperationId,
            OperationType = entity.OperationType,
            Operand1Value = entity.Operand1Value,
            Operand1Unit = entity.Operand1Unit,
            Operand1Measurement = entity.Operand1Measurement,
            Operand2Value = entity.Operand2Value,
            Operand2Unit = entity.Operand2Unit,
            Operand2Measurement = entity.Operand2Measurement,
            ResultValue = entity.ResultValue,
            ResultUnit = entity.ResultUnit,
            ResultMeasurement = entity.ResultMeasurement,
            IsError = entity.IsError,
            ErrorMessage = entity.ErrorMessage,
            Timestamp = entity.Timestamp
        };
    }

    public static List<QuantityMeasurementRecordDto> ToRecordDtoList(
        this IEnumerable<QuantityEntity> entities)
    {
        return entities.Select(e => e.ToRecordDto()).ToList();
    }
}
