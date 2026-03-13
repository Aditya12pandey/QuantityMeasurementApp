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
        private static ISupportsArithmetic supportsArithmetic = new SupportsArithmeticImpl(() => true);

        public static double GetConversionFactor(this WeightUnit unit)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM: 
                    return 1.0;
                case WeightUnit.GRAM:     
                    return 0.001;
                case WeightUnit.POUND:    
                    return 0.453592;
                default: 
                    throw new ArgumentException("Invalid unit");
            }
        }

        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }

        public static bool SupportsArithmetic(this WeightUnit unit)
        {
            return supportsArithmetic.IsSupported();
        }

        public static void ValidateOperationSupport(this WeightUnit unit, string operation)
        {
        }
    }
}