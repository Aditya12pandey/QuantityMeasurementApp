using System;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp
{
    class QuantityMeasurementApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Weight Equality:");

            var w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Console.WriteLine(w1.Equals(w2)); // true

            Console.WriteLine("\nWeight Conversion:");

            var pound = new QuantityWeight(2.20462, WeightUnit.POUND);
            Console.WriteLine(pound.ConvertTo(WeightUnit.KILOGRAM));

            Console.WriteLine("\nWeight Addition:");

            var result = QuantityWeight.Add(
                new QuantityWeight(1.0, WeightUnit.KILOGRAM),
                new QuantityWeight(1000.0, WeightUnit.GRAM));

            Console.WriteLine(result); // 2 KILOGRAM
        }
    }
}