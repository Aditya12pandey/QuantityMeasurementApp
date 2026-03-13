using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementUC12Tests
    {
        double epsilon = 0.0001;

        // ==================== SUBTRACTION TESTS ====================

        // 1
        [TestMethod]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(5.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 2
        [TestMethod]
        public void testSubtraction_SameUnit_LitreMinusLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(3.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = a.Subtract(b);
            Assert.AreEqual(7.0, result.Value, epsilon);
            Assert.AreEqual(VolumeUnit.LITRE, result.Unit);
        }

        // 3
        [TestMethod]
        public void testSubtraction_CrossUnit_FeetMinusInches()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(9.5, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 4
        [TestMethod]
        public void testSubtraction_CrossUnit_InchesMinusFeet()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(120.0, LengthUnit.INCHES);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(60.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.INCHES, result.Unit);
        }

        // 5
        [TestMethod]
        public void testSubtraction_ExplicitTargetUnit_Feet()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b, LengthUnit.FEET);
            Assert.AreEqual(9.5, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 6
        [TestMethod]
        public void testSubtraction_ExplicitTargetUnit_Inches()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b, LengthUnit.INCHES);
            Assert.AreEqual(114.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.INCHES, result.Unit);
        }

        // 7
        [TestMethod]
        public void testSubtraction_ExplicitTargetUnit_Millilitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = a.Subtract(b, VolumeUnit.MILLILITRE);
            Assert.AreEqual(3000.0, result.Value, epsilon);
            Assert.AreEqual(VolumeUnit.MILLILITRE, result.Unit);
        }

        // 8
        [TestMethod]
        public void testSubtraction_ResultingInNegative()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(-5.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 9
        [TestMethod]
        public void testSubtraction_ResultingInZero()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(120.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(0.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 10
        [TestMethod]
        public void testSubtraction_WithZeroOperand()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(0.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(5.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 11
        [TestMethod]
        public void testSubtraction_WithNegativeValues()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(-2.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(7.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 12
        [TestMethod]
        public void testSubtraction_NonCommutative()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> r1 = a.Subtract(b);
            Quantity<LengthUnit> r2 = b.Subtract(a);
            Assert.AreEqual(5.0, r1.Value, epsilon);
            Assert.AreEqual(-5.0, r2.Value, epsilon);
            Assert.IsFalse(r1.Equals(r2));
        }

        // 13
        [TestMethod]
        public void testSubtraction_WithLargeValues()
        {
            Quantity<WeightUnit> a = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> b = new Quantity<WeightUnit>(5e5, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> result = a.Subtract(b);
            Assert.AreEqual(5e5, result.Value, epsilon);
            Assert.AreEqual(WeightUnit.KILOGRAM, result.Unit);
        }
        [TestMethod]
        public void testSubtraction_NullOperand()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            try
            {
                a.Subtract(null);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }
       

        // 17
        [TestMethod]
        public void testSubtraction_CrossCategory()
        {
            // Cross-category is prevented by the generic type system at compile time.
            // This test verifies that LengthUnit and WeightUnit are distinct types,
            // confirming the type system enforces category isolation.
            Assert.AreNotEqual(typeof(LengthUnit), typeof(WeightUnit));

            Quantity<LengthUnit> length = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> length2 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = length.Subtract(length2);
            Assert.AreEqual(5.0, result.Value, epsilon);
        }

        // 18
        [TestMethod]
        public void testSubtraction_AllMeasurementCategories()
        {
            Quantity<LengthUnit> la = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> lb = new Quantity<LengthUnit>(4.0, LengthUnit.FEET);
            Quantity<LengthUnit> lengthResult = la.Subtract(lb);
            Assert.AreEqual(6.0, lengthResult.Value, epsilon);

            Quantity<WeightUnit> wa = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> wb = new Quantity<WeightUnit>(3.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> weightResult = wa.Subtract(wb);
            Assert.AreEqual(7.0, weightResult.Value, epsilon);

            Quantity<VolumeUnit> va = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> vb = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> volumeResult = va.Subtract(vb);
            Assert.AreEqual(3.0, volumeResult.Value, epsilon);
        }

        // 19
        [TestMethod]
        public void testSubtraction_ChainedOperations()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            Quantity<LengthUnit> c = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Subtract(b).Subtract(c);
            Assert.AreEqual(7.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // ==================== DIVISION TESTS ====================

        // 20
        [TestMethod]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(5.0, result, epsilon);
        }

        // 21
        [TestMethod]
        public void testDivision_SameUnit_LitreDividedByLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            double result = a.Divide(b);
            Assert.AreEqual(2.0, result, epsilon);
        }

        // 22
        [TestMethod]
        public void testDivision_CrossUnit_FeetDividedByInches()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(24.0, LengthUnit.INCHES);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(1.0, result, epsilon);
        }

        // 23
        [TestMethod]
        public void testDivision_CrossUnit_KilogramDividedByGram()
        {
            Quantity<WeightUnit> a = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> b = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM);
            double result = a.Divide(b);
            Assert.AreEqual(1.0, result, epsilon);
        }

        // 24
        [TestMethod]
        public void testDivision_RatioGreaterThanOne()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.IsTrue(result > 1.0);
            Assert.AreEqual(5.0, result, epsilon);
        }

        // 25
        [TestMethod]
        public void testDivision_RatioLessThanOne()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.IsTrue(result < 1.0);
            Assert.AreEqual(0.5, result, epsilon);
        }

        // 26
        [TestMethod]
        public void testDivision_RatioEqualToOne()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(1.0, result, epsilon);
        }

        // 27
        [TestMethod]
        public void testDivision_NonCommutative()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            double r1 = a.Divide(b);
            double r2 = b.Divide(a);
            Assert.AreEqual(2.0, r1, epsilon);
            Assert.AreEqual(0.5, r2, epsilon);
            Assert.AreNotEqual(r1, r2, epsilon);
        }

        // 28
        [TestMethod]
        public void testDivision_ByZero()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(0.0, LengthUnit.FEET);
            try
            {
                a.Divide(b);
                Assert.Fail("Expected ArithmeticException was not thrown");
            }
            catch (ArithmeticException)
            {
                // Expected
            }
        }

        // 29
        [TestMethod]
        public void testDivision_WithLargeRatio()
        {
            Quantity<WeightUnit> a = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> b = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            double result = a.Divide(b);
            Assert.AreEqual(1e6, result, epsilon);
        }

        // 30
        [TestMethod]
        public void testDivision_WithSmallRatio()
        {
            Quantity<WeightUnit> a = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> b = new Quantity<WeightUnit>(1e6, WeightUnit.KILOGRAM);
            double result = a.Divide(b);
            Assert.AreEqual(1e-6, result, 1e-10);
        }

        // 31
        [TestMethod]
        public void testDivision_NullOperand()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            try
            {
                a.Divide(null);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
            }
        }

        // 32
        [TestMethod]
        public void testDivision_CrossCategory()
        {
            Assert.AreNotEqual(typeof(LengthUnit), typeof(WeightUnit));

            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(2.0, result, epsilon);
        }

        // 33
        [TestMethod]
        public void testDivision_AllMeasurementCategories()
        {
            Quantity<LengthUnit> la = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> lb = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            Assert.AreEqual(5.0, la.Divide(lb), epsilon);

            Quantity<WeightUnit> wa = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> wb = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
            Assert.AreEqual(5.0, wa.Divide(wb), epsilon);

            Quantity<VolumeUnit> va = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> vb = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Assert.AreEqual(5.0, va.Divide(vb), epsilon);
        }

        // 34
        [TestMethod]
        public void testDivision_Associativity()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(12.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            Quantity<LengthUnit> c = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            double r1 = a.Divide(b) / c.Value * b.Value;

            double ab = a.Divide(b); // 4.0
            double bc = b.Divide(c); // 1.5
            Assert.AreEqual(4.0, ab, epsilon);
            Assert.AreEqual(1.5, bc, epsilon);
            Assert.AreNotEqual(ab, bc, epsilon);
        }

        // 35
        [TestMethod]
        public void testSubtractionAndDivision_Integration()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(4.0, LengthUnit.FEET);
            Quantity<LengthUnit> c = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);

            Quantity<LengthUnit> sub = a.Subtract(b);   // 6.0 FEET
            double div = sub.Divide(c);                  // 6.0 / 3.0 = 2.0

            Assert.AreEqual(6.0, sub.Value, epsilon);
            Assert.AreEqual(2.0, div, epsilon);
        }

        // 36
        [TestMethod]
        public void testSubtractionAddition_Inverse()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);

            Quantity<LengthUnit> result = a.Add(b, LengthUnit.FEET).Subtract(b);

            Assert.AreEqual(10.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        // 37
        [TestMethod]
        public void testSubtraction_Immutability()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);

            a.Subtract(b);

            Assert.AreEqual(10.0, a.Value, epsilon);
            Assert.AreEqual(3.0, b.Value, epsilon);
        }

        // 38
        [TestMethod]
        public void testDivision_Immutability()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            a.Divide(b);

            Assert.AreEqual(10.0, a.Value, epsilon);
            Assert.AreEqual(2.0, b.Value, epsilon);
        }

        // 39
        [TestMethod]
        public void testSubtraction_PrecisionAndRounding()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(1.0, LengthUnit.INCHES);

            Quantity<LengthUnit> result = a.Subtract(b);

            // 1 foot - 1 inch = 11/12 feet ≈ 0.9167, rounded to 2 dp = 0.92
            Assert.AreEqual(0.92, result.Value, epsilon);
        }

        // 40
        [TestMethod]
        public void testDivision_PrecisionHandling()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);

            double result = a.Divide(b);
            Assert.AreEqual(0.3333, result, 0.0001);
        }
    }
}