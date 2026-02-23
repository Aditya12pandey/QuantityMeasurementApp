using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        private double value;
        private LengthUnit unit;

        public Quantity(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        // =========================
        // Convert to base unit (FEET)
        // =========================
        private double ConvertToFeet()
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
                throw new Exception("Invalid Unit");
        }

        // =========================
        // STATIC CONVERSION METHOD
        // =========================
        public static double Convert(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            // Validation
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new Exception("Invalid value");

            // Step 1: Convert to feet
            double valueInFeet;

            if (fromUnit == LengthUnit.FEET)
                valueInFeet = value;

            else if (fromUnit == LengthUnit.INCH)
                valueInFeet = value / 12;

            else if (fromUnit == LengthUnit.YARD)
                valueInFeet = value * 3;

            else if (fromUnit == LengthUnit.CM)
                valueInFeet = (value * 0.393701) / 12;

            else
                throw new Exception("Invalid source unit");

            // Step 2: Convert from feet to target
            if (toUnit == LengthUnit.FEET)
                return valueInFeet;

            else if (toUnit == LengthUnit.INCH)
                return valueInFeet * 12;

            else if (toUnit == LengthUnit.YARD)
                return valueInFeet / 3;

            else if (toUnit == LengthUnit.CM)
                return (valueInFeet * 12) / 0.393701;

            else
                throw new Exception("Invalid target unit");
        }

        // =========================
        // INSTANCE METHOD
        // =========================
        public double ConvertTo(LengthUnit targetUnit)
        {
            return Convert(this.value, this.unit, targetUnit);
        }

        // =========================
        // EQUALS METHOD
        // =========================
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;

            double thisValue = this.ConvertToFeet();
            double otherValue = other.ConvertToFeet();

            return Math.Abs(thisValue - otherValue) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        // =========================
        // ToString (for debugging)
        // =========================
        public override string ToString()
        {
            return value + " " + unit;
        }
    }
}