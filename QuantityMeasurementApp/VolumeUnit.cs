using System;

namespace QuantityMeasurementApp
{
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    public static class VolumeUnitExtensions
    {
        public static double GetConversionFactor(this VolumeUnit unit)
        {
            switch (unit)
            {
                case VolumeUnit.LITRE:
                    return 1.0;

                case VolumeUnit.MILLILITRE:
                    return 0.001;

                case VolumeUnit.GALLON:
                    return 3.78541;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }

        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit.ToString();
        }
    }
}