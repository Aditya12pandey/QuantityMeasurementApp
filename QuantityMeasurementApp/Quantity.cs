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

        // Convert value to feet (base unit)
        private double ConvertToFeet()
        {
            if (unit == LengthUnit.FEET)
            {
                return value;
            }
            else if (unit == LengthUnit.INCH)
            {
                return value / 12;
            }
            else
            {
                throw new Exception("Invalid Unit");
            }
        }

        public override bool Equals(object obj)
        {
            // Same reference
            if (this == obj)
                return true;

            // Null or different type
            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;

            double thisValue = this.ConvertToFeet();
            double otherValue = other.ConvertToFeet();

            // Compare values
            return Math.Abs(thisValue - otherValue) < 0.0001;
        }

        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}