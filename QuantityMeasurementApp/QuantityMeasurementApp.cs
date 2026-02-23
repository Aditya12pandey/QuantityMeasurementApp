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

            if (q1.Equals(q2))
                Console.WriteLine("Equal (true)");
            else
                Console.WriteLine("Not Equal (false)");

            Quantity q3 = new Quantity(1.0, LengthUnit.INCH);
            Quantity q4 = new Quantity(1.0, LengthUnit.INCH);

            Console.WriteLine(q3.Equals(q4)); // True
        }
    }
}