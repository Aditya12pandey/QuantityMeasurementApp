using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementApp
    {
        

        public static bool CompareFeet(double value1, double value2)
        {
            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            return f1.Equals(f2);
        }

        public static bool CompareInches(double value1, double value2)
        {
            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            return i1.Equals(i2);
        }

        

        public static void Main(string[] args)
        {
            Console.WriteLine("Feet Comparison:");
            Console.WriteLine(CompareFeet(1.0, 1.0));

            Console.WriteLine("Inches Comparison:");
            Console.WriteLine(CompareInches(1.0, 1.0));
        }
    }
}