using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    class QuantityMeasurementApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1 Feet to Inches = " + Quantity.Convert(1.0, LengthUnit.FEET, LengthUnit.INCH));

            Console.WriteLine("3 Yards to Feet = " + Quantity.Convert(3.0, LengthUnit.YARD, LengthUnit.FEET));

            Console.WriteLine("36 Inches to Yards = " + Quantity.Convert(36.0, LengthUnit.INCH, LengthUnit.YARD));

            Console.WriteLine("1 CM to Inches = " + Quantity.Convert(1.0, LengthUnit.CM, LengthUnit.INCH));

            Console.WriteLine("0 Feet to Inches = " + Quantity.Convert(0.0, LengthUnit.FEET, LengthUnit.INCH));
        }
    }
}