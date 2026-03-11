using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        private double value;
        private LengthUnit unit;

        public Quantity(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new Exception("Invalid value");

            this.value = value;
            this.unit = unit;
        }

        // Convert to base unit (FEET)
        private double ToBase()
        {
            return LengthUnitHelper.ConvertToBaseUnit(value, unit);
        }

        // Convert to another unit
        public Quantity ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ToBase();
            double result = LengthUnitHelper.ConvertFromBaseUnit(baseValue, targetUnit);

            return new Quantity(result, targetUnit);
        }

        // Equals
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;

            return Math.Abs(this.ToBase() - other.ToBase()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToBase().GetHashCode();
        }

        // Add (UC6)
        public static Quantity Add(Quantity q1, Quantity q2)
        {
            return Add(q1, q2, q1.unit);
        }

        // Add with target (UC7)
        public static Quantity Add(Quantity q1, Quantity q2, LengthUnit targetUnit)
        {
            if (q1 == null || q2 == null)
                throw new Exception("Null value");

            double sum = q1.ToBase() + q2.ToBase();

            double result = LengthUnitHelper.ConvertFromBaseUnit(sum, targetUnit);

            return new Quantity(result, targetUnit);
        }

        public override string ToString()
        {
            return value + " " + unit;
        }
    }
}