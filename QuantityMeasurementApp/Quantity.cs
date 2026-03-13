using System;

namespace QuantityMeasurementApp
{
    public class Quantity<U> where U : struct
    {
        private readonly double value;
        private readonly U unit;

        public Quantity(double value, U unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid value");
            }

            if (object.Equals(unit, null))
            {
                throw new ArgumentException("Unit cannot be null");
            }

            this.value = value;
            this.unit = unit;
        }

        private double ToBase()
        {
            if (unit is LengthUnit)
            {
                LengthUnit u = (LengthUnit)(object)unit;
                return u.ConvertToBaseUnit(value);
            }

            if (unit is WeightUnit)
            {
                WeightUnit u = (WeightUnit)(object)unit;
                return u.ConvertToBaseUnit(value);
            }

            throw new ArgumentException("Unsupported unit");
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBase();

            double result;

            if (targetUnit is LengthUnit)
            {
                LengthUnit u = (LengthUnit)(object)targetUnit;
                result = u.ConvertFromBaseUnit(baseValue);
            }
            else if (targetUnit is WeightUnit)
            {
                WeightUnit u = (WeightUnit)(object)targetUnit;
                result = u.ConvertFromBaseUnit(baseValue);
            }
            else
            {
                throw new ArgumentException("Unsupported unit");
            }

            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            double sum = this.ToBase() + other.ToBase();

            return ConvertTo(unit).ConvertFromBase(sum);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            double sum = this.ToBase() + other.ToBase();

            if (targetUnit is LengthUnit)
            {
                LengthUnit u = (LengthUnit)(object)targetUnit;
                double value = u.ConvertFromBaseUnit(sum);
                return new Quantity<U>(value, targetUnit);
            }

            if (targetUnit is WeightUnit)
            {
                WeightUnit u = (WeightUnit)(object)targetUnit;
                double value = u.ConvertFromBaseUnit(sum);
                return new Quantity<U>(value, targetUnit);
            }

            throw new ArgumentException("Unsupported unit");
        }

        private Quantity<U> ConvertFromBase(double baseValue)
        {
            if (unit is LengthUnit)
            {
                LengthUnit u = (LengthUnit)(object)unit;
                double v = u.ConvertFromBaseUnit(baseValue);
                return new Quantity<U>(v, unit);
            }

            if (unit is WeightUnit)
            {
                WeightUnit u = (WeightUnit)(object)unit;
                double v = u.ConvertFromBaseUnit(baseValue);
                return new Quantity<U>(v, unit);
            }

            throw new ArgumentException("Unsupported unit");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(Quantity<U>))
            {
                return false;
            }

            Quantity<U> other = (Quantity<U>)obj;

            double a = this.ToBase();
            double b = other.ToBase();

            return Math.Abs(a - b) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToBase().GetHashCode();
        }

        public override string ToString()
        {
            return value + " " + unit;
        }
    }
}