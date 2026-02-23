namespace QuantityMeasurementApp.test;

[TestClass]
public sealed class TestClass
{

    [TestMethod]
    public void TestFeetEquality_SameValue()
    {
        var f1 = new Feet(1.0);
        var f2 = new Feet(1.0);

        Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod]
    public void TestFeetEquality_DifferentValue()
    {
        var f1 = new Feet(1.0);
        var f2 = new Feet(2.0);

        Assert.IsFalse(f1.Equals(f2));
    }

    
    [TestMethod]
    public void TestInchesEquality_SameValue()
    {
        var i1 = new Inches(1.0);
        var i2 = new Inches(1.0);

        Assert.IsTrue(i1.Equals(i2));
    }

    [TestMethod]
    public void TestInchesEquality_DifferentValue()
    {
        var i1 = new Inches(1.0);
        var i2 = new Inches(2.0);

        Assert.IsFalse(i1.Equals(i2));
    }

    

    [TestMethod]
    public void TestEquality_NullComparison()
    {
        var f1 = new Feet(1.0);

        Assert.IsFalse(f1.Equals(null));
    }

    [TestMethod]
    public void TestEquality_NonNumericInput()
    {
        var f1 = new Feet(1.0);

        Assert.IsFalse(f1.Equals("abc"));
    }

    [TestMethod]
    public void TestEquality_SameReference()
    {
        var f1 = new Feet(1.0);

        Assert.IsTrue(f1.Equals(f1));
    }


    [TestMethod]
    public void TestCompareFeet()
    {
        Assert.IsTrue(QuantityMeasurementApp.CompareFeet(1.0, 1.0));
    }

    [TestMethod]
    public void TestCompareInches()
    {
        Assert.IsTrue(QuantityMeasurementApp.CompareInches(1.0, 1.0));
    }
}
