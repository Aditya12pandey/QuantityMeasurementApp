using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class VolumeQuantityTests
    {
        double epsilon = 0.0001;

        // ---------------- Equality ----------------

        [TestMethod]
        public void testEquality_LitreToLitre_SameValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_LitreToLitre_DifferentValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Assert.IsFalse(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_LitreToMillilitre_EquivalentValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_MillilitreToLitre_EquivalentValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_LitreToGallon_EquivalentValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(0.264172, VolumeUnit.GALLON);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_GallonToLitre_EquivalentValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_VolumeVsLength_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);

            Assert.IsFalse(volume.Equals(length));
        }

        [TestMethod]
        public void testEquality_VolumeVsWeight_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<WeightUnit> weight = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(volume.Equals(weight));
        }

        [TestMethod]
        public void testEquality_NullComparison()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsFalse(a.Equals(null));
        }

        [TestMethod]
        public void testEquality_SameReference()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(a));
        }

        [TestMethod]
        public void testEquality_ZeroValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(0.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(0.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_NegativeVolume()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(-1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(-1000.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_LargeVolumeValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testEquality_SmallVolumeValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(0.001, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(a.Equals(b));
        }

        // ---------------- Conversion ----------------

        [TestMethod]
        public void testConversion_LitreToMillilitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(1000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_MillilitreToLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(1.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_GallonToLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(3.78541, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_LitreToGallon()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.GALLON);

            Assert.AreEqual(1.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_SameUnit()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(5.0, result.Value, epsilon);
        }

        // ---------------- Addition ----------------

        [TestMethod]
        public void testAddition_SameUnit_LitrePlusLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(3.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_SameUnit_MillilitrePlusMillilitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.MILLILITRE);

            Assert.AreEqual(1000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_CrossUnit_LitrePlusMillilitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_WithZero()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(0.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(5.0, result.Value, epsilon);
        }

        // ---------------- Enum Tests ----------------

        [TestMethod]
        public void testVolumeUnitEnum_LitreConstant()
        {
            double result = VolumeUnit.LITRE.GetConversionFactor();

            Assert.AreEqual(1.0, result, epsilon);
        }

        [TestMethod]
        public void testVolumeUnitEnum_MillilitreConstant()
        {
            double result = VolumeUnit.MILLILITRE.GetConversionFactor();

            Assert.AreEqual(0.001, result, epsilon);
        }

        [TestMethod]
        public void testVolumeUnitEnum_GallonConstant()
        {
            double result = VolumeUnit.GALLON.GetConversionFactor();

            Assert.AreEqual(3.78541, result, epsilon);
        }

        // ---------------- Architecture Tests ----------------

        [TestMethod]
        public void testGenericQuantity_VolumeOperations_Consistency()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void testScalability_VolumeIntegration()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(2000.0, result.Value, epsilon);
        }
        // ---------------- Additional Equality Tests ----------------

        [TestMethod]
        public void testEquality_TransitiveProperty()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> c = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(c));
            Assert.IsTrue(a.Equals(c));
        }

        // ---------------- Additional Conversion Tests ----------------

        [TestMethod]
        public void testConversion_MillilitreToGallon()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.GALLON);

            Assert.AreEqual(0.264172, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_ZeroValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(0.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(0.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_NegativeValue()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(-1.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(-1000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testConversion_RoundTrip()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.5, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result =
                a.ConvertTo(VolumeUnit.MILLILITRE).ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(1.5, result.Value, epsilon);
        }

        // ---------------- Additional Addition Tests ----------------

        [TestMethod]
        public void testAddition_CrossUnit_MillilitrePlusLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.MILLILITRE);

            Assert.AreEqual(2000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_CrossUnit_GallonPlusLitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.GALLON);

            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Litre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Millilitre()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.MILLILITRE);

            Assert.AreEqual(2000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_ExplicitTargetUnit_Gallon()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.GALLON);

            Assert.AreEqual(2.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_Commutativity()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> r1 = a.Add(b, VolumeUnit.LITRE);
            Quantity<VolumeUnit> r2 = b.Add(a, VolumeUnit.MILLILITRE);

            Assert.IsTrue(r1.ConvertTo(VolumeUnit.MILLILITRE).Equals(r2));
        }

        [TestMethod]
        public void testAddition_NegativeValues()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(-2000.0, VolumeUnit.MILLILITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(3.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_LargeValues()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(1000000.0, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(1000000.0, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(2000000.0, result.Value, epsilon);
        }

        [TestMethod]
        public void testAddition_SmallValues()
        {
            Quantity<VolumeUnit> a = new Quantity<VolumeUnit>(0.001, VolumeUnit.LITRE);
            Quantity<VolumeUnit> b = new Quantity<VolumeUnit>(0.002, VolumeUnit.LITRE);

            Quantity<VolumeUnit> result = a.Add(b, VolumeUnit.LITRE);

            Assert.AreEqual(0.003, result.Value, epsilon);
        }

        // ---------------- Base Conversion Tests ----------------

        [TestMethod]
        public void testConvertToBaseUnit_LitreToLitre()
        {
            double result = VolumeUnit.LITRE.ConvertToBaseUnit(5.0);

            Assert.AreEqual(5.0, result, epsilon);
        }

        [TestMethod]
        public void testConvertToBaseUnit_MillilitreToLitre()
        {
            double result = VolumeUnit.MILLILITRE.ConvertToBaseUnit(1000.0);

            Assert.AreEqual(1.0, result, epsilon);
        }

        [TestMethod]
        public void testConvertToBaseUnit_GallonToLitre()
        {
            double result = VolumeUnit.GALLON.ConvertToBaseUnit(1.0);

            Assert.AreEqual(3.78541, result, epsilon);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_LitreToLitre()
        {
            double result = VolumeUnit.LITRE.ConvertFromBaseUnit(2.0);

            Assert.AreEqual(2.0, result, epsilon);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_LitreToMillilitre()
        {
            double result = VolumeUnit.MILLILITRE.ConvertFromBaseUnit(1.0);

            Assert.AreEqual(1000.0, result, epsilon);
        }

        [TestMethod]
        public void testConvertFromBaseUnit_LitreToGallon()
        {
            double result = VolumeUnit.GALLON.ConvertFromBaseUnit(3.78541);

            Assert.AreEqual(1.0, result, epsilon);
        }

        // ---------------- Architecture Tests ----------------

        [TestMethod]
        public void testBackwardCompatibility_AllUC1Through10Tests()
        {
            Quantity<LengthUnit> length =
                new Quantity<LengthUnit>(1.0, LengthUnit.FEET);

            Quantity<WeightUnit> weight =
                new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsNotNull(length);
            Assert.IsNotNull(weight);
        }
    }
}