using System;
using QuantityService.Models;

namespace QuantityService.Business;

public class LengthMeasurable : IMeasurable
{
    private readonly LengthUnit _unit;
    public LengthMeasurable(LengthUnit unit) { _unit = unit; }

    public double GetConversionFactor()
    {
        return _unit switch
        {
            LengthUnit.FEET => 0.3048,
            LengthUnit.INCHES => 0.0254,
            LengthUnit.YARDS => 0.9144,
            LengthUnit.CENTIMETERS => 0.01,
            _ => throw new ArgumentException($"Unknown LengthUnit: {_unit}")
        };
    }

    public double ConvertToBaseUnit(double value) => value * GetConversionFactor();
    public double ConvertFromBaseUnit(double baseValue) => baseValue / GetConversionFactor();
    public string GetUnitName() => _unit.ToString();
    public string GetMeasurementType() => "LENGTH";
    public bool SupportsArithmetic() => true;
    public void ValidateOperationSupport(string operation) { }
}

public class WeightMeasurable : IMeasurable
{
    private readonly WeightUnit _unit;
    public WeightMeasurable(WeightUnit unit) { _unit = unit; }

    public double GetConversionFactor()
    {
        return _unit switch
        {
            WeightUnit.GRAM => 0.001,
            WeightUnit.KILOGRAM => 1.0,
            WeightUnit.POUND => 0.453592,
            _ => throw new ArgumentException($"Unknown WeightUnit: {_unit}")
        };
    }

    public double ConvertToBaseUnit(double value) => value * GetConversionFactor();
    public double ConvertFromBaseUnit(double baseValue) => baseValue / GetConversionFactor();
    public string GetUnitName() => _unit.ToString();
    public string GetMeasurementType() => "WEIGHT";
    public bool SupportsArithmetic() => true;
    public void ValidateOperationSupport(string operation) { }
}

public class VolumeMeasurable : IMeasurable
{
    private readonly VolumeUnit _unit;
    public VolumeMeasurable(VolumeUnit unit) { _unit = unit; }

    public double GetConversionFactor()
    {
        return _unit switch
        {
            VolumeUnit.LITRE => 1.0,
            VolumeUnit.MILLILITRE => 0.001,
            VolumeUnit.GALLON => 3.78541,
            _ => throw new ArgumentException($"Unknown VolumeUnit: {_unit}")
        };
    }

    public double ConvertToBaseUnit(double value) => value * GetConversionFactor();
    public double ConvertFromBaseUnit(double baseValue) => baseValue / GetConversionFactor();
    public string GetUnitName() => _unit.ToString();
    public string GetMeasurementType() => "VOLUME";
    public bool SupportsArithmetic() => true;
    public void ValidateOperationSupport(string operation) { }
}

public class TemperatureMeasurable : IMeasurable
{
    private readonly TemperatureUnit _unit;
    public TemperatureMeasurable(TemperatureUnit unit) { _unit = unit; }

    public double GetConversionFactor() => 1.0;

    public double ConvertToBaseUnit(double value)
    {
        return _unit switch
        {
            TemperatureUnit.CELSIUS => value,
            TemperatureUnit.FAHRENHEIT => (value - 32.0) * 5.0 / 9.0,
            _ => throw new ArgumentException($"Unknown TemperatureUnit: {_unit}")
        };
    }

    public double ConvertFromBaseUnit(double baseValue)
    {
        return _unit switch
        {
            TemperatureUnit.CELSIUS => baseValue,
            TemperatureUnit.FAHRENHEIT => baseValue * 9.0 / 5.0 + 32.0,
            _ => throw new ArgumentException($"Unknown TemperatureUnit: {_unit}")
        };
    }

    public string GetUnitName() => _unit.ToString();
    public string GetMeasurementType() => "TEMPERATURE";
    public bool SupportsArithmetic() => false;
    public void ValidateOperationSupport(string operation)
    {
        if (operation == "ADD" || operation == "SUBTRACT" || operation == "DIVIDE")
            throw new NotSupportedException($"Operation '{operation}' is not supported for TEMPERATURE.");
    }
}
