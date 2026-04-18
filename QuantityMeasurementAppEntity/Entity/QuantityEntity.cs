using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuantityMeasurementAppEntity.DTOs;

namespace QuantityMeasurementAppEntity.Entity;

/// <summary>
/// Persistence model for quantity measurement history (EF Core mapped table).
/// </summary>
[Table("quantity_measurements")]
public class QuantityEntity
{
    [Key]
    [Column("operation_id")]
    [MaxLength(64)]
    public string OperationId { get; set; }

    [Required]
    [Column("operation_type")]
    [MaxLength(32)]
    public string OperationType { get; set; }

    [Column("operand1_value")]
    public double Operand1Value { get; set; }

    [Column("operand1_unit")]
    [MaxLength(64)]
    public string Operand1Unit { get; set; }

    [Column("operand1_measurement")]
    [MaxLength(32)]
    public string Operand1Measurement { get; set; }

    [Column("operand2_value")]
    public double? Operand2Value { get; set; }

    [Column("operand2_unit")]
    [MaxLength(64)]
    public string Operand2Unit { get; set; }

    [Column("operand2_measurement")]
    [MaxLength(32)]
    public string Operand2Measurement { get; set; }

    [Column("result_value")]
    [MaxLength(128)]
    public string ResultValue { get; set; }

    [Column("result_unit")]
    [MaxLength(64)]
    public string ResultUnit { get; set; }

    [Column("result_measurement")]
    [MaxLength(32)]
    public string ResultMeasurement { get; set; }

    [Column("is_error")]
    public bool IsError { get; set; }

    [Column("error_message")]
    [MaxLength(1024)]
    public string ErrorMessage { get; set; }

    [Column("timestamp")]
    public DateTime Timestamp { get; set; }

    [Required]
    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; }

    public QuantityEntity() { }

    public QuantityEntity(string operationType,
                          QuantityDTO operand1,
                          QuantityDTO result)
    {
        OperationId = Guid.NewGuid().ToString();
        OperationType = operationType;
        Timestamp = DateTime.UtcNow;

        Operand1Value = operand1.Value;
        Operand1Unit = operand1.UnitName;
        Operand1Measurement = operand1.MeasurementType;

        ResultValue = result.Value.ToString("F4");
        ResultUnit = result.UnitName;
        ResultMeasurement = result.MeasurementType;

        IsError = false;
    }

    public QuantityEntity(string operationType,
                          QuantityDTO operand1,
                          QuantityDTO operand2,
                          string resultValue,
                          string resultUnit = null,
                          string resultMeasurement = null)
    {
        OperationId = Guid.NewGuid().ToString();
        OperationType = operationType;
        Timestamp = DateTime.UtcNow;

        Operand1Value = operand1.Value;
        Operand1Unit = operand1.UnitName;
        Operand1Measurement = operand1.MeasurementType;

        Operand2Value = operand2.Value;
        Operand2Unit = operand2.UnitName;
        Operand2Measurement = operand2.MeasurementType;

        ResultValue = resultValue;
        ResultUnit = resultUnit;
        ResultMeasurement = resultMeasurement;

        IsError = false;
    }

    public QuantityEntity(string operationType,
                          QuantityDTO operand1,
                          QuantityDTO operand2,
                          string errorMessage,
                          bool isError)
    {
        OperationId = Guid.NewGuid().ToString();
        OperationType = operationType;
        Timestamp = DateTime.UtcNow;

        Operand1Value = operand1.Value;
        Operand1Unit = operand1.UnitName;
        Operand1Measurement = operand1.MeasurementType;

        if (operand2 != null)
        {
            Operand2Value = operand2.Value;
            Operand2Unit = operand2.UnitName;
            Operand2Measurement = operand2.MeasurementType;
        }

        IsError = isError;
        ErrorMessage = errorMessage;
        ResultValue = "ERROR";
    }

    public override string ToString()
    {
        if (IsError)
            return $"[{Timestamp:u}] {OperationType} ERROR: {ErrorMessage}";

        string op2 = Operand2Value.HasValue
            ? $" | Op2: {Operand2Value} {Operand2Unit}"
            : string.Empty;

        return $"[{Timestamp:u}] {OperationType} | " +
               $"Op1: {Operand1Value} {Operand1Unit}{op2} => {ResultValue} {ResultUnit}";
    }
}
