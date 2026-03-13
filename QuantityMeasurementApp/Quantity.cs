using System;

namespace QuantityMeasurementApp
{
    public class Quantity<U> where U : struct
    {
        private double value;
        private U unit;

        public Quantity(double value, U unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public double Value
        {
            get { return value; }
        }

        public U Unit
        {
            get { return unit; }
        }

        public double GetValue()
        {
            return value;
        }

        public U GetUnit()
        {
            return unit;
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

            if (unit is VolumeUnit)
            {
                VolumeUnit u = (VolumeUnit)(object)unit;
                return u.ConvertToBaseUnit(value);
            }

            throw new ArgumentException("Unsupported unit type");
        }

        private void ValidateOperand(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            if (!double.IsFinite(this.value) || !double.IsFinite(other.value))
                throw new ArgumentException("Quantity values must be finite");
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBase();
            double result = ConvertBaseValueToTarget(baseValue, targetUnit);
            return new Quantity<U>(result, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            double base1 = this.ToBase();
            double base2 = other.ToBase();
            double sum = base1 + base2;

            double result = ConvertBaseValueToTarget(sum, targetUnit);
            return new Quantity<U>(result, targetUnit);
        }

        /// <summary>
        /// Subtracts another quantity from this quantity.
        /// Result is expressed in this quantity's unit (implicit target).
        /// </summary>
        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, this.unit);
        }

        /// <summary>
        /// Subtracts another quantity from this quantity.
        /// Result is expressed in the specified target unit.
        /// </summary>
        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateOperand(other);

            double base1 = this.ToBase();
            double base2 = other.ToBase();
            double difference = base1 - base2;

            double resultValue = ConvertBaseValueToTarget(difference, targetUnit);
            return new Quantity<U>(Math.Round(resultValue, 2), targetUnit);
        }

        /// <summary>
        /// Divides this quantity by another quantity.
        /// Returns a dimensionless scalar (double) representing the ratio.
        /// </summary>
        public double Divide(Quantity<U> other)
        {
            ValidateOperand(other);

            double base2 = other.ToBase();

            if (Math.Abs(base2) < 1e-10)
                throw new ArithmeticException("Division by zero is not allowed");

            double base1 = this.ToBase();
            return base1 / base2;
        }

        private double ConvertBaseValueToTarget(double baseValue, U targetUnit)
        {
            if (targetUnit is LengthUnit)
            {
                LengthUnit u = (LengthUnit)(object)targetUnit;
                return u.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is WeightUnit)
            {
                WeightUnit u = (WeightUnit)(object)targetUnit;
                return u.ConvertFromBaseUnit(baseValue);
            }

            if (targetUnit is VolumeUnit)
            {
                VolumeUnit u = (VolumeUnit)(object)targetUnit;
                return u.ConvertFromBaseUnit(baseValue);
            }

            throw new ArgumentException("Unsupported unit type");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Quantity<U> other = obj as Quantity<U>;

            if (other == null)
                return false;

            double base1 = this.ToBase();
            double base2 = other.ToBase();

            return Math.Abs(base1 - base2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ToBase().GetHashCode();
        }

        public override string ToString()
        {
            return value + " " + unit.ToString();
        }
    }
}