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
        // Convert to FEET (base)
        // =========================
        public double ConvertToFeet()
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
        // STATIC CONVERT METHOD (UC5)
        // =========================
        public static double Convert(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new Exception("Invalid value");

            double valueInFeet;

            // Convert to feet
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

            // Convert to target
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
        // ADD METHOD (UC6)
        // =========================
        public static Quantity Add(Quantity q1, Quantity q2)
        {
            if (q1 == null || q2 == null)
                throw new Exception("Null value not allowed");

            // Convert both to feet
            double value1 = q1.ConvertToFeet();
            double value2 = q2.ConvertToFeet();

            // Add
            double sumInFeet = value1 + value2;

            // Convert back to unit of first operand
            double resultValue = Convert(sumInFeet, LengthUnit.FEET, q1.unit);

            return new Quantity(resultValue, q1.unit);
        }

        // Instance method
        public Quantity Add(Quantity other)
        {
            return Add(this, other);
        }

        // =========================
        // EQUALS (UC3)
        // =========================
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;

            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }

        public override string ToString()
        {
            return value + " " + unit;
        }

        public static Quantity Add(Quantity q1, Quantity q2, LengthUnit targetUnit)
        {
            // Validation
            if (q1 == null || q2 == null)
                throw new Exception("Null quantity not allowed");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new Exception("Invalid target unit");

            // Convert both to FEET
            double value1 = q1.ConvertToFeet();
            double value2 = q2.ConvertToFeet();

            // Add
            double sumInFeet = value1 + value2;

            // Convert to target unit
            double resultValue = Convert(sumInFeet, LengthUnit.FEET, targetUnit);

            return new Quantity(resultValue, targetUnit);
        }
    }
}