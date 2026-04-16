namespace QuantityMeasurementAppEntity.DTOs;

/// <summary>
/// API/history projection of a persisted measurement — not mapped to the database.
/// </summary>
public class QuantityMeasurementRecordDto
{
    public string OperationId { get; set; }
    public string OperationType { get; set; }

    public double Operand1Value { get; set; }
    public string Operand1Unit { get; set; }
    public string Operand1Measurement { get; set; }

    public double? Operand2Value { get; set; }
    public string Operand2Unit { get; set; }
    public string Operand2Measurement { get; set; }

    public string ResultValue { get; set; }
    public string ResultUnit { get; set; }
    public string ResultMeasurement { get; set; }

    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }

    public DateTime Timestamp { get; set; }
}
