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

        private static double RoundToTwoDecimals(double value)
        {
            return Math.Round(value, 2);
        }

        private void ValidateArithmeticOperands(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Operand quantity cannot be null");

            if (!double.IsFinite(this.value))
                throw new ArgumentException("This quantity's value must be finite");

            if (!double.IsFinite(other.value))
                throw new ArgumentException("Operand quantity's value must be finite");
        }

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double base1 = this.ToBase();
            double base2 = other.ToBase();
            return operation.Compute(base1, base2);
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            double baseValue = ToBase();
            double result = ConvertBaseValueToTarget(baseValue, targetUnit);
            return new Quantity<U>(result, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, this.unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other);
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);
            double resultValue = ConvertBaseValueToTarget(baseResult, targetUnit);
            return new Quantity<U>(RoundToTwoDecimals(resultValue), targetUnit);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            return Subtract(other, this.unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            ValidateArithmeticOperands(other);
            double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double resultValue = ConvertBaseValueToTarget(baseResult, targetUnit);
            return new Quantity<U>(RoundToTwoDecimals(resultValue), targetUnit);
        }

        public double Divide(Quantity<U> other)
        {
            ValidateArithmeticOperands(other);
            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
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