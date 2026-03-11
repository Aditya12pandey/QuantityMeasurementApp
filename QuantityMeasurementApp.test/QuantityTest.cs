using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementTests
{
    [TestClass]
    public class QuantityTest
    {
        [TestMethod]
        public void Test_FeetPlusFeet()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(2.0, LengthUnit.FEET);

            var result = Quantity.Add(q1, q2);

            Assert.AreEqual(3.0, Quantity.Convert(result.ConvertToFeet(), LengthUnit.FEET, LengthUnit.FEET), 0.0001);
        }

        [TestMethod]
        public void Test_FeetPlusInch()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(2.0, LengthUnit.FEET)));
        }

        [TestMethod]
        public void Test_InchPlusFeet()
        {
            var q1 = new Quantity(12.0, LengthUnit.INCH);
            var q2 = new Quantity(1.0, LengthUnit.FEET);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(24.0, LengthUnit.INCH)));
        }

        [TestMethod]
        public void Test_YardPlusFeet()
        {
            var q1 = new Quantity(1.0, LengthUnit.YARD);
            var q2 = new Quantity(3.0, LengthUnit.FEET);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(2.0, LengthUnit.YARD)));
        }

        [TestMethod]
        public void Test_CMPlusInch()
        {
            var q1 = new Quantity(2.54, LengthUnit.CM);
            var q2 = new Quantity(1.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(5.08, LengthUnit.CM)));
        }

        [TestMethod]
        public void Test_AddZero()
        {
            var q1 = new Quantity(5.0, LengthUnit.FEET);
            var q2 = new Quantity(0.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(5.0, LengthUnit.FEET)));
        }

        [TestMethod]
        public void Test_NegativeValues()
        {
            var q1 = new Quantity(5.0, LengthUnit.FEET);
            var q2 = new Quantity(-2.0, LengthUnit.FEET);

            var result = Quantity.Add(q1, q2);

            Assert.IsTrue(result.Equals(new Quantity(3.0, LengthUnit.FEET)));
        }

        [TestMethod]
        public void Test_TargetUnit_Feet()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.FEET);

            Assert.IsTrue(result.Equals(new Quantity(2.0, LengthUnit.FEET)));
        }

        [TestMethod]
        public void Test_TargetUnit_Inch()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.INCH);

            Assert.IsTrue(result.Equals(new Quantity(24.0, LengthUnit.INCH)));
        }

        [TestMethod]
        public void Test_TargetUnit_Yard()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.YARD);

            Assert.IsTrue(result.Equals(new Quantity(0.6667, LengthUnit.YARD)));
        }

        [TestMethod]
        public void Test_TargetUnit_CM()
        {
            var q1 = new Quantity(1.0, LengthUnit.INCH);
            var q2 = new Quantity(1.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.CM);

            Assert.IsTrue(result.Equals(new Quantity(5.08, LengthUnit.CM)));
        }

        [TestMethod]
        public void Test_Commutativity()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var r1 = Quantity.Add(q1, q2, LengthUnit.YARD);
            var r2 = Quantity.Add(q2, q1, LengthUnit.YARD);

            Assert.IsTrue(r1.Equals(r2));
        }

        [TestMethod]
        public void Test_WithZero()
        {
            var q1 = new Quantity(5.0, LengthUnit.FEET);
            var q2 = new Quantity(0.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.YARD);

            Assert.IsTrue(result.Equals(new Quantity(1.6667, LengthUnit.YARD)));
        }
        [TestMethod]
        public void Test_Convert_FeetToInch()
        {
            var q = new Quantity(1.0, LengthUnit.FEET);

            var result = q.ConvertTo(LengthUnit.INCH);

            Assert.IsTrue(result.Equals(new Quantity(12.0, LengthUnit.INCH)));
        }

        [TestMethod]
        public void Test_Add_WithTarget()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.YARD);

            Assert.IsTrue(result.Equals(new Quantity(0.6667, LengthUnit.YARD)));
        }

    }
}