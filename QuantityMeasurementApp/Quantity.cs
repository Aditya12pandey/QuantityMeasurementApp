using System;

namespace QuantityMeasurementApp
{
    public class Quantity
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public Quantity(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new Exception("Invalid value");

            this.value = value;
            this.unit = unit;
        }

        private double ToBase()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // Convert to another unit
        public Quantity ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ToBase();
            double result = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Quantity(result, targetUnit);
        }

        // Addition (UC6)
        public static Quantity Add(Quantity q1, Quantity q2)
        {
            return Add(q1, q2, q1.unit);
        }

        // Addition with target unit (UC7)
        public static Quantity Add(Quantity q1, Quantity q2, LengthUnit targetUnit)
        {
            if (q1 == null || q2 == null)
                throw new Exception("Quantity cannot be null");

            double sum = q1.ToBase() + q2.ToBase();
            double result = targetUnit.ConvertFromBaseUnit(sum);

            return new Quantity(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj is not Quantity other)
                return false;

            return Math.Abs(this.ToBase() - other.ToBase()) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToBase().GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}