using System;

namespace QuantityMeasurementApp
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        // Convert to base unit (KILOGRAM)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM:
                    return value;

                case WeightUnit.GRAM:
                    return value * 0.001;

                case WeightUnit.POUND:
                    return value * 0.453592;

                default:
                    throw new Exception("Invalid weight unit");
            }
        }

        // Convert from base unit (KILOGRAM) to target
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM:
                    return baseValue;

                case WeightUnit.GRAM:
                    return baseValue / 0.001;

                case WeightUnit.POUND:
                    return baseValue / 0.453592;

                default:
                    throw new Exception("Invalid weight unit");
            }
        }
    }
}