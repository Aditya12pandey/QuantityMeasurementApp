namespace QuantityService.Business;

public interface IMeasurable
{
    double GetConversionFactor();
    double ConvertToBaseUnit(double value);
    double ConvertFromBaseUnit(double baseValue);
    string GetUnitName();
    string GetMeasurementType();
    bool SupportsArithmetic();
    void ValidateOperationSupport(string operation);
}
