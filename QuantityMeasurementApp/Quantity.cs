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

        // Existing methods kept
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
            else if (targetUnit is VolumeUnit)
            {
                VolumeUnit u = (VolumeUnit)(object)targetUnit;
                result = u.ConvertFromBaseUnit(baseValue);
            }
            else
            {
                throw new ArgumentException("Unsupported unit type");
            }

            return new Quantity<U>(result, targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
            {
                throw new ArgumentException("Quantity cannot be null");
            }

            double base1 = this.ToBase();
            double base2 = other.ToBase();

            double sum = base1 + base2;

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

            if (targetUnit is VolumeUnit)
            {
                VolumeUnit u = (VolumeUnit)(object)targetUnit;
                double value = u.ConvertFromBaseUnit(sum);
                return new Quantity<U>(value, targetUnit);
            }

            throw new ArgumentException("Unsupported unit type");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Quantity<U> other = obj as Quantity<U>;

            if (other == null)
            {
                return false;
            }

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