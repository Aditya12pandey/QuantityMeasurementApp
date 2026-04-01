using System;

namespace QuantityMeasurementAppBusiness.Interfaces
{
    public interface IMeasurable
    {
        // Conversion factor relative to the base unit
        double GetConversionFactor();

        // Convert value in this unit -> base unit
        double ConvertToBaseUnit(double value);

        // Convert base unit value -> this unit
        double ConvertFromBaseUnit(double baseValue);

        // Returns unit name e.g. "FEET", "CELSIUS"
        string GetUnitName();

        // Returns category e.g. "LENGTH", "WEIGHT", "VOLUME", "TEMPERATURE"
        string GetMeasurementType();

        // Returns true if arithmetic is supported for this unit type
        bool SupportsArithmetic();

        // Throws NotSupportedException if operation is not allowed
        void ValidateOperationSupport(string operation);
    }
}