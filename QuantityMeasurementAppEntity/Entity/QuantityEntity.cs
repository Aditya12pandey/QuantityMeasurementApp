using System;
using QuantityMeasurementAppEntity.DTOs;
namespace QuantityMeasurementAppEntity.Entity
{
    [Serializable]
    public class QuantityEntity
    {
        public string OperationId { get; set; }
        public string OperationType { get; set; }  // COMPARE | CONVERT | ADD | SUBTRACT | DIVIDE

        // First operand
        public double Operand1Value { get; set; }
        public string Operand1Unit { get; set; }
        public string Operand1Measurement { get; set; }

        // Second operand (optional)
        public double? Operand2Value { get; set; }
        public string Operand2Unit { get; set; }
        public string Operand2Measurement { get; set; }

        // Result
        public string ResultValue { get; set; }
        public string ResultUnit { get; set; }
        public string ResultMeasurement { get; set; }

        // Error
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; }

        // Default constructor for serialization
        public QuantityEntity() { }

        // Constructor for single-operand operations (e.g. CONVERT)
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

        // Constructor for double-operand operations (COMPARE | ADD | SUBTRACT | DIVIDE)
        public QuantityEntity(string operationType,
                              QuantityDTO operand1,
                              QuantityDTO operand2,
                              string resultValue,
                              string? resultUnit = null,
                              string? resultMeasurement = null)

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

        // Constructor for error records
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
}