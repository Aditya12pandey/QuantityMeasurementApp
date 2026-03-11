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

            Console.WriteLine(q1.Equals(q2)); // True

            Console.WriteLine(q1.ConvertTo(LengthUnit.INCH)); // 12 INCH

            Console.WriteLine(Quantity.Add(q1, q2, LengthUnit.FEET)); // 2 FEET

            Console.WriteLine(Quantity.Add(q1, q2, LengthUnit.YARD)); // 0.667 YARD
        }
    }
}