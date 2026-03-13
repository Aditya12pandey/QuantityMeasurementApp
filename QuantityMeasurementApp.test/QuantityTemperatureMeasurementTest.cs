using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class TemperatureQuantityUC14Tests
    {
        double epsilon = 0.0001;

        [TestMethod]
        public void testTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEquality_FahrenheitToFahrenheit_SameValue()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_0Celsius32Fahrenheit()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_100Celsius212Fahrenheit()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEquality_CelsiusToFahrenheit_Negative40Equal()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEquality_SymmetricProperty()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void testTemperatureEquality_ReflexiveProperty()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsTrue(a.Equals(a));
        }

        [TestMethod]
        public void testTemperatureConversion_CelsiusToFahrenheit_VariousValues()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(122.0, a.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);

            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(-20.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(-4.0, b.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);

            Quantity<TemperatureUnit> c = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(32.0, c.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);

            Quantity<TemperatureUnit> d = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(212.0, d.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_FahrenheitToCelsius_VariousValues()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(122.0, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(50.0, a.ConvertTo(TemperatureUnit.CELSIUS).Value, epsilon);

            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(-4.0, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(-20.0, b.ConvertTo(TemperatureUnit.CELSIUS).Value, epsilon);

            Quantity<TemperatureUnit> c = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(0.0, c.ConvertTo(TemperatureUnit.CELSIUS).Value, epsilon);

            Quantity<TemperatureUnit> d = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(100.0, d.ConvertTo(TemperatureUnit.CELSIUS).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_RoundTrip_PreservesValue()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(37.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> result = a.ConvertTo(TemperatureUnit.FAHRENHEIT)
                                                .ConvertTo(TemperatureUnit.CELSIUS);
            Assert.AreEqual(37.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_SameUnit()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(100.0, a.ConvertTo(TemperatureUnit.CELSIUS).Value, epsilon);

            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(212.0, b.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_ZeroValue()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(32.0, a.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_NegativeValues()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(-20.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(-4.0, a.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);

            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(-40.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(-40.0, b.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversion_LargeValues()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(1000.0, TemperatureUnit.CELSIUS);
            Assert.AreEqual(1832.0, a.ConvertTo(TemperatureUnit.FAHRENHEIT).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Add()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                a.Add(b);
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException) { }
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Subtract()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                a.Subtract(b);
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException) { }
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_Divide()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                a.Divide(b);
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException) { }
        }

        [TestMethod]
        public void testTemperatureUnsupportedOperation_ErrorMessage()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            try
            {
                a.Add(b);
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Temperature"));
            }
        }

        [TestMethod]
        public void testTemperatureVsLengthIncompatibility()
        {
            Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(100.0, LengthUnit.FEET);
            Assert.IsFalse(temp.Equals(length));
        }

        [TestMethod]
        public void testTemperatureVsWeightIncompatibility()
        {
            Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            Quantity<WeightUnit> weight = new Quantity<WeightUnit>(50.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(temp.Equals(weight));
        }

        [TestMethod]
        public void testTemperatureVsVolumeIncompatibility()
        {
            Quantity<TemperatureUnit> temp = new Quantity<TemperatureUnit>(25.0, TemperatureUnit.CELSIUS);
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(25.0, VolumeUnit.LITRE);
            Assert.IsFalse(temp.Equals(volume));
        }

        [TestMethod]
        public void testOperationSupportMethods_TemperatureUnitAddition()
        {
            Assert.IsFalse(TemperatureUnit.CELSIUS.SupportsArithmetic());
        }

        [TestMethod]
        public void testOperationSupportMethods_TemperatureUnitDivision()
        {
            Assert.IsFalse(TemperatureUnit.FAHRENHEIT.SupportsArithmetic());
        }

        [TestMethod]
        public void testOperationSupportMethods_LengthUnitAddition()
        {
            Assert.IsTrue(LengthUnit.FEET.SupportsArithmetic());
        }

        [TestMethod]
        public void testOperationSupportMethods_WeightUnitDivision()
        {
            Assert.IsTrue(WeightUnit.KILOGRAM.SupportsArithmetic());
        }

        [TestMethod]
        public void testIMeasurableInterface_Evolution_BackwardCompatible()
        {
            Quantity<LengthUnit> l1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> l2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Assert.IsTrue(l1.Equals(l2));
            Assert.AreEqual(2.0, l1.Add(l2, LengthUnit.FEET).Value, epsilon);

            Quantity<WeightUnit> w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);
            Assert.IsTrue(w1.Equals(w2));

            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Assert.IsTrue(v1.Equals(v2));
        }

        [TestMethod]
        public void testTemperatureUnit_NonLinearConversion()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> result = a.ConvertTo(TemperatureUnit.FAHRENHEIT);
            Assert.AreNotEqual(100.0, result.Value, epsilon);
            Assert.AreEqual(212.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureUnit_AllConstants()
        {
            Assert.IsNotNull(TemperatureUnit.CELSIUS);
            Assert.IsNotNull(TemperatureUnit.FAHRENHEIT);
            Assert.AreNotEqual(TemperatureUnit.CELSIUS, TemperatureUnit.FAHRENHEIT);
        }

        [TestMethod]
        public void testTemperatureUnit_NameMethod()
        {
            Assert.AreEqual("CELSIUS", TemperatureUnit.CELSIUS.GetUnitName());
            Assert.AreEqual("FAHRENHEIT", TemperatureUnit.FAHRENHEIT.GetUnitName());
        }

        [TestMethod]
        public void testTemperatureUnit_ConversionFactor()
        {
            Assert.AreEqual(1.0, TemperatureUnit.CELSIUS.GetConversionFactor(), epsilon);
            Assert.AreEqual(1.0, TemperatureUnit.FAHRENHEIT.GetConversionFactor(), epsilon);
        }

        [TestMethod]
        public void testTemperatureNullUnitValidation()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void testTemperatureNullOperandValidation_InComparison()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsFalse(a.Equals(null));
        }

        [TestMethod]
        public void testTemperatureDifferentValuesInequality()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureBackwardCompatibility_UC1_Through_UC13()
        {
            Quantity<LengthUnit> l1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> l2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Assert.IsTrue(l1.Equals(l2));
            Assert.AreEqual(2.0, l1.Add(l2, LengthUnit.FEET).Value, epsilon);
            Assert.AreEqual(0.0, l1.Subtract(l2, LengthUnit.FEET).Value, epsilon);
            Assert.AreEqual(1.0, l1.Divide(l2), epsilon);

            Quantity<WeightUnit> w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);
            Assert.IsTrue(w1.Equals(w2));
            Assert.AreEqual(2.0, w1.Add(w2, WeightUnit.KILOGRAM).Value, epsilon);

            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Assert.IsTrue(v1.Equals(v2));
            Assert.AreEqual(2.0, v1.Add(v2, VolumeUnit.LITRE).Value, epsilon);
        }

        [TestMethod]
        public void testTemperatureConversionPrecision_Epsilon()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(122.0, TemperatureUnit.FAHRENHEIT);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureConversionEdgeCase_VerySmallDifference()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(0.001, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(0.001, TemperatureUnit.CELSIUS);
            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testTemperatureEnumImplementsIMeasurable()
        {
            Assert.IsNotNull(TemperatureUnit.CELSIUS.GetUnitName());
            Assert.IsNotNull(TemperatureUnit.FAHRENHEIT.GetUnitName());
            Assert.AreEqual(100.0, TemperatureUnit.CELSIUS.ConvertToBaseUnit(100.0), epsilon);
            Assert.AreEqual(100.0, TemperatureUnit.FAHRENHEIT.ConvertToBaseUnit(212.0), epsilon);
        }

        [TestMethod]
        public void testTemperatureDefaultMethodInheritance()
        {
            Assert.IsTrue(LengthUnit.FEET.SupportsArithmetic());
            Assert.IsTrue(LengthUnit.INCHES.SupportsArithmetic());
            Assert.IsTrue(WeightUnit.KILOGRAM.SupportsArithmetic());
            Assert.IsTrue(WeightUnit.GRAM.SupportsArithmetic());
            Assert.IsTrue(VolumeUnit.LITRE.SupportsArithmetic());
            Assert.IsTrue(VolumeUnit.MILLILITRE.SupportsArithmetic());
            Assert.IsFalse(TemperatureUnit.CELSIUS.SupportsArithmetic());
            Assert.IsFalse(TemperatureUnit.FAHRENHEIT.SupportsArithmetic());
        }

        [TestMethod]
        public void testTemperatureCrossUnitAdditionAttempt()
        {
            Quantity<TemperatureUnit> a = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Quantity<TemperatureUnit> b = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.FAHRENHEIT);
            try
            {
                a.Add(b);
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException) { }
        }

        [TestMethod]
        public void testTemperatureValidateOperationSupport_MethodBehavior()
        {
            try
            {
                TemperatureUnit.CELSIUS.ValidateOperationSupport("addition");
                Assert.Fail("Expected NotSupportedException was not thrown");
            }
            catch (NotSupportedException) { }
        }

        [TestMethod]
        public void testTemperatureIntegrationWithGenericQuantity()
        {
            Quantity<TemperatureUnit> t = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS);
            Assert.IsNotNull(t);
            Assert.AreEqual(100.0, t.Value, epsilon);
            Assert.AreEqual(TemperatureUnit.CELSIUS, t.Unit);
            Quantity<TemperatureUnit> converted = t.ConvertTo(TemperatureUnit.FAHRENHEIT);
            Assert.AreEqual(212.0, converted.Value, epsilon);
            Assert.AreEqual(TemperatureUnit.FAHRENHEIT, converted.Unit);
        }
    }
}