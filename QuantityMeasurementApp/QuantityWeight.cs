using System;

namespace QuantityMeasurementApp
{
    public class QuantityWeight
    {
        private readonly double value;
        private readonly WeightUnit unit;

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new Exception("Invalid weight value");

            this.value = value;
            this.unit = unit;
        }

        private double ToBase()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // Convert weight to another unit
        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = ToBase();
            double result = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(result, targetUnit);
        }

        // Default addition (result in first operand unit)
        public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2)
        {
            return Add(w1, w2, w1.unit);
        }

        // Addition with explicit target unit
        public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2, WeightUnit targetUnit)
        {
            if (w1 == null || w2 == null)
                throw new Exception("Weight cannot be null");

            double sum = w1.ToBase() + w2.ToBase();
            double result = targetUnit.ConvertFromBaseUnit(sum);

            return new QuantityWeight(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;

            if (obj is not QuantityWeight other)
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