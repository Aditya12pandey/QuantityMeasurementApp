using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Feet f1 = new Feet(1.0);
            Feet f2 = new Feet(1.0);

            Console.WriteLine(f1.Equals(f2)
                ? "Equal (true)"
                : "Not Equal (false)");
        }
    }
}