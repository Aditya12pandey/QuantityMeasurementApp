using System;

namespace QuantityMeasurementApp
{
    public enum TemperatureUnit
    {
        CELSIUS,
        FAHRENHEIT
    }

    public static class TemperatureUnitExtensions
    {
        // Lambda expression: temperature does not support arithmetic
        public static ISupportsArithmetic supportsArithmetic = new SupportsArithmeticImpl(() => false);

        // Lambda: Celsius to Celsius (identity)
        private static readonly Func<double, double> CELSIUS_TO_CELSIUS = (celsius) => celsius;

        // Lambda: Fahrenheit to Celsius
        private static readonly Func<double, double> FAHRENHEIT_TO_CELSIUS = (fahrenheit) => (fahrenheit - 32.0) * 5.0 / 9.0;

        // Lambda: Celsius to Fahrenheit
        private static readonly Func<double, double> CELSIUS_TO_FAHRENHEIT = (celsius) => (celsius * 9.0 / 5.0) + 32.0;

        public static double GetConversionFactor(this TemperatureUnit unit)
        {
            // Temperature uses non-linear conversion, factor not applicable
            return 1.0;
        }

        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            // Base unit is CELSIUS
            switch (unit)
            {
                case TemperatureUnit.CELSIUS:
                    return CELSIUS_TO_CELSIUS(value);
                case TemperatureUnit.FAHRENHEIT:
                    return FAHRENHEIT_TO_CELSIUS(value);
                default:
                    throw new ArgumentException("Invalid temperature unit");
            }
        }

        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            // baseValue is in CELSIUS
            switch (unit)
            {
                case TemperatureUnit.CELSIUS:
                    return CELSIUS_TO_CELSIUS(baseValue);
                case TemperatureUnit.FAHRENHEIT:
                    return CELSIUS_TO_FAHRENHEIT(baseValue);
                default:
                    throw new ArgumentException("Invalid temperature unit");
            }
        }

        public static string GetUnitName(this TemperatureUnit unit)
        {
            return unit.ToString();
        }

        public static bool SupportsArithmetic(this TemperatureUnit unit)
        {
            return supportsArithmetic.IsSupported();
        }

        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            if (!supportsArithmetic.IsSupported())
            {
                throw new NotSupportedException("Temperature does not support " + operation +". Temperature arithmetic (e.g., adding two absolute temperatures) is not meaningful.");
            }
        }
    }
}