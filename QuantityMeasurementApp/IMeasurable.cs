using System;

namespace QuantityMeasurementApp
{
    public interface ISupportsArithmetic
    {
        bool IsSupported();
    }

    public interface IMeasurable
    {
        double GetConversionFactor();
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);
        string GetUnitName();

        bool SupportsArithmetic()
        {
            return true;
        }

        void ValidateOperationSupport(string operation)
        {
        
        }
    }
}