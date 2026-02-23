using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementTests
{
    [TestClass]
    public class QuantityTest
    {
        [TestMethod]
        public void Test_YardToYard_SameValue()
        {
            var q1 = new Quantity(1.0, LengthUnit.YARD);
            var q2 = new Quantity(1.0, LengthUnit.YARD);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_YardToFeet()
        {
            var q1 = new Quantity(1.0, LengthUnit.YARD);
            var q2 = new Quantity(3.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_YardToInch()
        {
            var q1 = new Quantity(1.0, LengthUnit.YARD);
            var q2 = new Quantity(36.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_CMToInch()
        {
            var q1 = new Quantity(1.0, LengthUnit.CM);
            var q2 = new Quantity(0.393701, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_CMToFeet_NotEqual()
        {
            var q1 = new Quantity(1.0, LengthUnit.CM);
            var q2 = new Quantity(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_MultiUnit_Transitive()
        {
            var q1 = new Quantity(1.0, LengthUnit.YARD);
            var q2 = new Quantity(3.0, LengthUnit.FEET);
            var q3 = new Quantity(36.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q3));
            Assert.IsTrue(q1.Equals(q3));
        }

        [TestMethod]
        public void Test_NullComparison()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
        public void Test_SameReference()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q1));
        }
    }
}