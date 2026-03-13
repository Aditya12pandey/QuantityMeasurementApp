using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementUC13Tests
    {
        double epsilon = 0.0001;
        // 1
        [TestMethod]
        public void testArithmeticOperation_Add_EnumComputation()
        {
            double result = ArithmeticOperation.ADD.Compute(10.0, 5.0);
            Assert.AreEqual(15.0, result, epsilon);
        }

        // 2
        [TestMethod]
        public void testArithmeticOperation_Subtract_EnumComputation()
        {
            double result = ArithmeticOperation.SUBTRACT.Compute(10.0, 5.0);
            Assert.AreEqual(5.0, result, epsilon);
        }

        // 3
        [TestMethod]
        public void testArithmeticOperation_Divide_EnumComputation()
        {
            double result = ArithmeticOperation.DIVIDE.Compute(10.0, 5.0);
            Assert.AreEqual(2.0, result, epsilon);
        }

        // 4
        [TestMethod]
        public void testEnumConstant_ADD_CorrectlyAdds()
        {
            double result = ArithmeticOperation.ADD.Compute(7.0, 3.0);
            Assert.AreEqual(10.0, result, epsilon);
        }

        // 5
        [TestMethod]
        public void testEnumConstant_SUBTRACT_CorrectlySubtracts()
        {
            double result = ArithmeticOperation.SUBTRACT.Compute(7.0, 3.0);
            Assert.AreEqual(4.0, result, epsilon);
        }

        // 6
        [TestMethod]
        public void testEnumConstant_DIVIDE_CorrectlyDivides()
        {
            double result = ArithmeticOperation.DIVIDE.Compute(7.0, 2.0);
            Assert.AreEqual(3.5, result, epsilon);
        }

        // 7
        [TestMethod]
        public void testArithmeticOperation_DivideByZero_EnumThrows()
        {
            try
            {
                ArithmeticOperation.DIVIDE.Compute(10.0, 0.0);
                Assert.Fail("Expected ArithmeticException was not thrown");
            }
            catch (ArithmeticException)
            {
               
            }
        }

        // 8
        [TestMethod]
        public void testValidation_NullOperand_Add_Throws()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            try
            {
                a.Add(null);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
               
            }
        }

        // 9
        [TestMethod]
        public void testValidation_NullOperand_Subtract_Throws()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            try
            {
                a.Subtract(null);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                
            }
        }

        // 10
        [TestMethod]
        public void testValidation_NullOperand_Divide_Throws()
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

        // 11
        [TestMethod]
        public void testValidation_NullOperand_ConsistentAcrossOperations()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);

            bool addThrew = false;
            bool subtractThrew = false;
            bool divideThrew = false;

            try { a.Add(null); }
            catch (ArgumentException) { addThrew = true; }

            try { a.Subtract(null); }
            catch (ArgumentException) { subtractThrew = true; }

            try { a.Divide(null); }
            catch (ArgumentException) { divideThrew = true; }

            Assert.IsTrue(addThrew, "Add should throw on null");
            Assert.IsTrue(subtractThrew, "Subtract should throw on null");
            Assert.IsTrue(divideThrew, "Divide should throw on null");
        }

        // 12
        [TestMethod]
        public void testValidation_FiniteValue_ConsistentAcrossOperations()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            bool addThrew = false;
            bool subtractThrew = false;
            bool divideThrew = false;

            try { a.Add(b); }
            catch (ArgumentException) { addThrew = true; }

            try { a.Subtract(b); }
            catch (ArgumentException) { subtractThrew = true; }

            try { a.Divide(b); }
            catch (ArgumentException) { divideThrew = true; }

            Assert.IsTrue(addThrew, "Add should throw for NaN value");
            Assert.IsTrue(subtractThrew, "Subtract should throw for NaN value");
            Assert.IsTrue(divideThrew, "Divide should throw for NaN value");
        }

        // 13
        [TestMethod]
        public void testValidation_InfiniteValue_ConsistentAcrossOperations()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(double.PositiveInfinity, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            bool addThrew = false;
            bool subtractThrew = false;
            bool divideThrew = false;

            try { a.Add(b); }
            catch (ArgumentException) { addThrew = true; }

            try { a.Subtract(b); }
            catch (ArgumentException) { subtractThrew = true; }

            try { a.Divide(b); }
            catch (ArgumentException) { divideThrew = true; }

            Assert.IsTrue(addThrew, "Add should throw for Infinite value");
            Assert.IsTrue(subtractThrew, "Subtract should throw for Infinite value");
            Assert.IsTrue(divideThrew, "Divide should throw for Infinite value");
        }


        // 14
        [TestMethod]
        public void testAdd_UC12_BehaviorPreserved_SameUnit()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Add(b, LengthUnit.FEET);
            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        // 15
        [TestMethod]
        public void testAdd_UC12_BehaviorPreserved_CrossUnit()
        {
            Quantity<WeightUnit> a = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> b = new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM);
            Quantity<WeightUnit> result = a.Add(b, WeightUnit.GRAM);
            Assert.AreEqual(15000.0, result.Value, epsilon);
        }

        // 16
        [TestMethod]
        public void testAdd_ImplicitTargetUnit()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Add(b);
            Assert.AreEqual(2.0, result.Value, epsilon);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }


        // 17
        [TestMethod]
        public void testSubtract_UC12_BehaviorPreserved_CrossUnit()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(9.5, result.Value, epsilon);
        }

        // 18
        [TestMethod]
        public void testSubtract_UC12_BehaviorPreserved_ExplicitTarget()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> result = a.Subtract(b, VolumeUnit.MILLILITRE);
            Assert.AreEqual(3000.0, result.Value, epsilon);
        }


        // 19
        [TestMethod]
        public void testDivide_UC12_BehaviorPreserved_SameUnit()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(5.0, result, epsilon);
        }

        // 20
        [TestMethod]
        public void testDivide_UC12_BehaviorPreserved_CrossUnit()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(24.0, LengthUnit.INCHES);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(1.0, result, epsilon);
        }

        // 21
        [TestMethod]
        public void testDivide_ByZero_Throws()
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
                
            }
        }


        // 22
        [TestMethod]
        public void testRounding_AddSubtract_TwoDecimalPlaces()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(1.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Subtract(b);
            Assert.AreEqual(0.92, result.Value, epsilon);
        }

        // 23
        [TestMethod]
        public void testRounding_Divide_NoRounding()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            double result = a.Divide(b);
            Assert.AreEqual(0.3333, result, 0.0001);
        }

        // 24
        [TestMethod]
        public void testRounding_Helper_Accuracy()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(1.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Add(b, LengthUnit.YARDS);
            Assert.AreEqual(0.36, result.Value, epsilon);
        }


        // 25
        [TestMethod]
        public void testImmutability_AfterAdd_ViaCentralizedHelper()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            a.Add(b);
            Assert.AreEqual(10.0, a.Value, epsilon);
            Assert.AreEqual(5.0, b.Value, epsilon);
        }

        // 26
        [TestMethod]
        public void testImmutability_AfterSubtract_ViaCentralizedHelper()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            a.Subtract(b);
            Assert.AreEqual(10.0, a.Value, epsilon);
            Assert.AreEqual(3.0, b.Value, epsilon);
        }

        // 27
        [TestMethod]
        public void testImmutability_AfterDivide_ViaCentralizedHelper()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            a.Divide(b);
            Assert.AreEqual(10.0, a.Value, epsilon);
            Assert.AreEqual(2.0, b.Value, epsilon);
        }


        // 28
        [TestMethod]
        public void testAllOperations_AcrossAllCategories()
        {
            // Length
            Quantity<LengthUnit> la = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> lb = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
            Assert.AreEqual(15.0, la.Add(lb, LengthUnit.FEET).Value, epsilon);
            Assert.AreEqual(5.0, la.Subtract(lb).Value, epsilon);
            Assert.AreEqual(2.0, la.Divide(lb), epsilon);

            // Weight
            Quantity<WeightUnit> wa = new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> wb = new Quantity<WeightUnit>(5.0, WeightUnit.KILOGRAM);
            Assert.AreEqual(15.0, wa.Add(wb, WeightUnit.KILOGRAM).Value, epsilon);
            Assert.AreEqual(5.0, wa.Subtract(wb).Value, epsilon);
            Assert.AreEqual(2.0, wa.Divide(wb), epsilon);

            // Volume
            Quantity<VolumeUnit> va = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> vb = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Assert.AreEqual(15.0, va.Add(vb, VolumeUnit.LITRE).Value, epsilon);
            Assert.AreEqual(5.0, va.Subtract(vb).Value, epsilon);
            Assert.AreEqual(2.0, va.Divide(vb), epsilon);
        }


        // 29
        [TestMethod]
        public void testImplicitTargetUnit_AddSubtract()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);

            Quantity<LengthUnit> addResult = a.Add(b);
            Quantity<LengthUnit> subResult = a.Subtract(b);

            Assert.AreEqual(LengthUnit.FEET, addResult.Unit);
            Assert.AreEqual(LengthUnit.FEET, subResult.Unit);
            Assert.AreEqual(15.0, addResult.Value, epsilon);
            Assert.AreEqual(5.0, subResult.Value, epsilon);
        }

        // 30
        [TestMethod]
        public void testExplicitTargetUnit_AddSubtract_Overrides()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            Quantity<LengthUnit> addResult = a.Add(b, LengthUnit.INCHES);
            Quantity<LengthUnit> subResult = a.Subtract(b, LengthUnit.INCHES);

            Assert.AreEqual(LengthUnit.INCHES, addResult.Unit);
            Assert.AreEqual(LengthUnit.INCHES, subResult.Unit);
            Assert.AreEqual(144.0, addResult.Value, epsilon);
            Assert.AreEqual(96.0, subResult.Value, epsilon);
        }


        // 31
        [TestMethod]
        public void testSubtractionAndDivision_Integration()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(4.0, LengthUnit.FEET);
            Quantity<LengthUnit> c = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            Quantity<LengthUnit> sub = a.Subtract(b);
            double div = sub.Divide(c);
            Assert.AreEqual(6.0, sub.Value, epsilon);
            Assert.AreEqual(2.0, div, epsilon);
        }

        // 32
        [TestMethod]
        public void testArithmetic_Chain_Operations()
        {
            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            Quantity<LengthUnit> q3 = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            Quantity<LengthUnit> q4 = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            double result = q1.Add(q2, LengthUnit.FEET)
                              .Subtract(q3, LengthUnit.FEET)
                              .Divide(q4);

            Assert.AreEqual(3.0, result, epsilon);
        }

        // 33
        [TestMethod]
        public void testSubtractionAddition_Inverse()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(3.0, LengthUnit.FEET);
            Quantity<LengthUnit> result = a.Add(b, LengthUnit.FEET).Subtract(b);
            Assert.AreEqual(10.0, result.Value, epsilon);
        }

        // 34
        [TestMethod]
        public void testBackwardCompatibility_AllUC12Tests_Length()
        {
            Quantity<LengthUnit> l1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> l2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Assert.IsTrue(l1.Equals(l2));
            Assert.AreEqual(12.0, l1.ConvertTo(LengthUnit.INCHES).Value, epsilon);
            Assert.AreEqual(2.0, l1.Add(l2, LengthUnit.FEET).Value, epsilon);
            Assert.AreEqual(9.5, new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCHES)).Value, epsilon);
            Assert.AreEqual(5.0, new Quantity<LengthUnit>(10.0, LengthUnit.FEET)
                .Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET)), epsilon);
        }

        // 35
        [TestMethod]
        public void testBackwardCompatibility_AllUC12Tests_Weight()
        {
            Quantity<WeightUnit> w1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Quantity<WeightUnit> w2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);
            Assert.IsTrue(w1.Equals(w2));
            Assert.AreEqual(1000.0, w1.ConvertTo(WeightUnit.GRAM).Value, epsilon);
            Assert.AreEqual(2.0, w1.Add(w2, WeightUnit.KILOGRAM).Value, epsilon);
            Assert.AreEqual(5.0, new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM)
                .Subtract(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM)).Value, epsilon);
            Assert.AreEqual(2.0, new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM)
                .Divide(new Quantity<WeightUnit>(5.0, WeightUnit.KILOGRAM)), epsilon);
        }

        // 36
        [TestMethod]
        public void testBackwardCompatibility_AllUC12Tests_Volume()
        {
            Quantity<VolumeUnit> v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Assert.IsTrue(v1.Equals(v2));
            Assert.AreEqual(1000.0, v1.ConvertTo(VolumeUnit.MILLILITRE).Value, epsilon);
            Assert.AreEqual(2.0, v1.Add(v2, VolumeUnit.LITRE).Value, epsilon);
            Assert.AreEqual(4.5, new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)
                .Subtract(new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE)).Value, epsilon);
            Assert.AreEqual(2.0, new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE)
                .Divide(new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE)), epsilon);
        }


        // 37
        [TestMethod]
        public void testFutureOperation_MultiplicationPattern()
        {
            double result = ArithmeticOperation.MULTIPLY.Compute(6.0, 3.0);
            Assert.AreEqual(18.0, result, epsilon);
        }

        // 38
        [TestMethod]
        public void testEnumDispatch_AllOperations_CorrectlyDispatched()
        {
            Assert.AreEqual(8.0,  ArithmeticOperation.ADD.Compute(5.0, 3.0),      epsilon);
            Assert.AreEqual(2.0,  ArithmeticOperation.SUBTRACT.Compute(5.0, 3.0), epsilon);
            Assert.AreEqual(2.5,  ArithmeticOperation.DIVIDE.Compute(5.0, 2.0),   epsilon);
            Assert.AreEqual(15.0, ArithmeticOperation.MULTIPLY.Compute(5.0, 3.0), epsilon);
        }

        // 39
        [TestMethod]
        public void testHelper_BaseUnitConversion_Correct()
        {
            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Add(b, LengthUnit.FEET);
            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        // 40
        [TestMethod]
        public void testHelper_ResultConversion_Correct()
        {            Quantity<LengthUnit> a = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<LengthUnit> b = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            Quantity<LengthUnit> result = a.Add(b, LengthUnit.INCHES);
            Assert.AreEqual(24.0, result.Value, epsilon);
        }
    }
}