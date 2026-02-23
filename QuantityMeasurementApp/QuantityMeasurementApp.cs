using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    class QuantityMeasurementApp
    {
        static void Main(string[] args)
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCH);

            Console.WriteLine(Quantity.Add(q1, q2, LengthUnit.FEET));   // 2 FEET
            Console.WriteLine(Quantity.Add(q1, q2, LengthUnit.INCH));   // 24 INCH
            Console.WriteLine(Quantity.Add(q1, q2, LengthUnit.YARD));   // 0.667 YARD

            Quantity q3 = new Quantity(1.0, LengthUnit.YARD);
            Quantity q4 = new Quantity(3.0, LengthUnit.FEET);

            Console.WriteLine(Quantity.Add(q3, q4, LengthUnit.YARD));   // 2 YARD
        }
    }
}