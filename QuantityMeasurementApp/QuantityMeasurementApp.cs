using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    class QuantityMeasurementApp
    {
        static void Main(string[] args)
        {
            // Yard to Feet
            Quantity q1 = new Quantity(1.0, LengthUnit.YARD);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            Console.WriteLine(q1.Equals(q2)); // True

            // Yard to Inch
            Quantity q3 = new Quantity(1.0, LengthUnit.YARD);
            Quantity q4 = new Quantity(36.0, LengthUnit.INCH);

            Console.WriteLine(q3.Equals(q4)); // True

            // CM to Inch
            Quantity q5 = new Quantity(1.0, LengthUnit.CM);
            Quantity q6 = new Quantity(0.393701, LengthUnit.INCH);

            Console.WriteLine(q5.Equals(q6)); // True
        }
    }
}