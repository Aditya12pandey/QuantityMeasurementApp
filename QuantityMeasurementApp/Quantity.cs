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

        // Convert everything to FEET (base unit)
        private double ConvertToFeet()
        {
            if (unit == LengthUnit.FEET)
            {
                return value;
            }
            else if (unit == LengthUnit.INCH)
            {
                return value / 12;   // 12 inch = 1 foot
            }
            else if (unit == LengthUnit.YARD)
            {
                return value * 3;    // 1 yard = 3 feet
            }
            else if (unit == LengthUnit.CM)
            {
                // 1 cm = 0.393701 inch
                // inch to feet = divide by 12
                return (value * 0.393701) / 12;
            }
            else
            {
                throw new Exception("Invalid Unit");
            }
        }

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
    }
}