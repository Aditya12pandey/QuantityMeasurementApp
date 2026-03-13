using System;

namespace QuantityMeasurementApp
{
    public class QuantityMeasurementDemo
    {
        public static void DemonstrateEquality<U>(Quantity<U> a, Quantity<U> b) where U : struct
        {
            Console.WriteLine(a.Equals(b));
        }

        public static void DemonstrateConversion<U>(Quantity<U> a, U targetUnit) where U : struct
        {
            Quantity<U> result = a.ConvertTo(targetUnit);
            Console.WriteLine(result);
        }

        public static void DemonstrateAddition<U>(Quantity<U> a, Quantity<U> b, U unit) where U : struct
        {
            Quantity<U> result = a.Add(b, unit);
            Console.WriteLine(result);
        }

        public static void Main(string[] args)
        {
            Quantity<LengthUnit> l1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> l2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

            DemonstrateEquality(l1, l2);
            DemonstrateConversion(l1, LengthUnit.INCHES);
            DemonstrateAddition(l1, l2, LengthUnit.FEET);

            Quantity<WeightUnit> w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            DemonstrateEquality(w1, w2);
            DemonstrateConversion(w1, WeightUnit.GRAM);
            DemonstrateAddition(w1, w2, WeightUnit.KILOGRAM);
        }
    }
}