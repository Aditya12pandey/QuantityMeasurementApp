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

    public static class LengthUnitHelper
    {
        public static double ConvertToBaseUnit(double value, LengthUnit unit)
        {
            if (unit == LengthUnit.FEET)
                return value;

            else if (unit == LengthUnit.INCH)
                return value / 12;

            else if (unit == LengthUnit.YARD)
                return value * 3;

            else if (unit == LengthUnit.CM)
                return (value * 0.393701) / 12;

            else
                throw new Exception("Invalid unit");
        }

        public static double ConvertFromBaseUnit(double value, LengthUnit unit)
        {
            if (unit == LengthUnit.FEET)
                return value;

            else if (unit == LengthUnit.INCH)
                return value * 12;

            else if (unit == LengthUnit.YARD)
                return value / 3;

            else if (unit == LengthUnit.CM)
                return (value * 12) / 0.393701;

            else
                throw new Exception("Invalid unit");
        }
    }
}