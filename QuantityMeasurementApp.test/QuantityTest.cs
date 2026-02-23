using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementTests
{
    [TestClass]
    public class QuantityTest
    {
        [TestMethod]
        public void Test_FeetToInches()
        {
            double result = Quantity.Convert(1.0, LengthUnit.FEET, LengthUnit.INCH);

            Assert.AreEqual(12.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_InchesToFeet()
        {
            double result = Quantity.Convert(24.0, LengthUnit.INCH, LengthUnit.FEET);

            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_YardsToInches()
        {
            double result = Quantity.Convert(1.0, LengthUnit.YARD, LengthUnit.INCH);

            Assert.AreEqual(36.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_InchesToYards()
        {
            double result = Quantity.Convert(72.0, LengthUnit.INCH, LengthUnit.YARD);

            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_CMToInches()
        {
            double result = Quantity.Convert(2.54, LengthUnit.CM, LengthUnit.INCH);

            Assert.AreEqual(1.0, result, 0.01);
        }

        [TestMethod]
        public void Test_FeetToYard()
        {
            double result = Quantity.Convert(6.0, LengthUnit.FEET, LengthUnit.YARD);

            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_ZeroValue()
        {
            double result = Quantity.Convert(0.0, LengthUnit.FEET, LengthUnit.INCH);

            Assert.AreEqual(0.0, result);
        }

        [TestMethod]
        public void Test_NegativeValue()
        {
            double result = Quantity.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCH);

            Assert.AreEqual(-12.0, result, 0.0001);
        }

        [TestMethod]
        public void Test_RoundTrip()
        {
            double value = 5.0;

            double result = Quantity.Convert(
                Quantity.Convert(value, LengthUnit.FEET, LengthUnit.INCH),
                LengthUnit.INCH,
                LengthUnit.FEET);

            Assert.AreEqual(value, result, 0.0001);
        }

        
    }
}