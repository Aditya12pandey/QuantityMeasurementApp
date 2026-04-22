namespace QuantityService.Models;

public class TwoQuantityRequest
{
    public QuantityDTO First { get; set; }
    public QuantityDTO Second { get; set; }
}

public class ConvertRequest
{
    public QuantityDTO Quantity { get; set; }
    public string TargetUnit { get; set; }
}

public class AddSubtractRequest
{
    public QuantityDTO First { get; set; }
    public QuantityDTO Second { get; set; }
    public string TargetUnit { get; set; }
}

public class CompareResultDto
{
    public bool Equal { get; set; }
}

public class DivideResultDto
{
    public double Value { get; set; }
}

public class QuantityMeasurementRecordDto
{
    public string OperationId { get; set; }
    public string OperationType { get; set; }
    public double Operand1Value { get; set; }
    public string Operand1Unit { get; set; }
    public double? Operand2Value { get; set; }
    public string Operand2Unit { get; set; }
    public string ResultValue { get; set; }
    public string ResultUnit { get; set; }
    public DateTime Timestamp { get; set; }
}

public class MeasurementCountDto
{
    public int Count { get; set; }
}
