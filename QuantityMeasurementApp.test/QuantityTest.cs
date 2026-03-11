using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.test
{
    [TestClass]
    public class QuantityTest
    {
        [TestMethod]
        public void Test_FeetToInchConversion()
        {
            var q = new Quantity(1.0, LengthUnit.FEET);

            var result = q.ConvertTo(LengthUnit.INCH);

            Assert.IsTrue(result.Equals(new Quantity(12.0, LengthUnit.INCH)));
        }

        [TestMethod]
        public void Test_Equality_FeetAndInch()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Test_Addition_FeetAndInch()
        {
            var q1 = new Quantity(1.0, LengthUnit.FEET);
            var q2 = new Quantity(12.0, LengthUnit.INCH);

            var result = Quantity.Add(q1, q2, LengthUnit.FEET);

            Assert.IsTrue(result.Equals(new Quantity(2.0, LengthUnit.FEET)));
        }
    }
}