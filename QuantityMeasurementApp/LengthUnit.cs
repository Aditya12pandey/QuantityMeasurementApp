using System;

namespace QuantityMeasurementApp
{
    public enum LengthUnit
    {
        FEET,
        INCH,
        YARD,
        CM
    }

    public static class LengthUnitExtensions
    {
        // Convert value to base unit (FEET)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                    return value;

                case LengthUnit.INCH:
                    return value / 12;

                case LengthUnit.YARD:
                    return value * 3;

                case LengthUnit.CM:
                    return (value * 0.393701) / 12;

                default:
                    throw new Exception("Invalid Unit");
            }
        }

        // Convert value from base unit (FEET) to target unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                    return baseValue;

                case LengthUnit.INCH:
                    return baseValue * 12;

                case LengthUnit.YARD:
                    return baseValue / 3;

                case LengthUnit.CM:
                    return (baseValue * 12) / 0.393701;

                default:
                    throw new Exception("Invalid Unit");
            }
        }
    }
}