namespace QuantityMeasurementApp.test;

[TestClass]
public sealed class FeetTest
{
    // testEquality_SameValue
    [TestMethod]
    public void TestEquality_SameValue()
    {
        var f1 = new Feet(1.0);
        var f2 = new Feet(1.0);

        Assert.IsTrue(f1.Equals(f2), "1.0 ft should equal 1.0 ft");
    }

    // testEquality_DifferentValue
    [TestMethod]
    public void TestEquality_DifferentValue()
    {
        var f1 = new Feet(1.0);
        var f2 = new Feet(2.0);

        Assert.IsFalse(f1.Equals(f2), "1.0 ft should not equal 2.0 ft");
    }

    // testEquality_NullComparison
    [TestMethod]
    public void TestEquality_NullComparison()
    {
        var f1 = new Feet(1.0);

        Assert.IsFalse(f1.Equals(null), "Value should not equal null");
    }

    // testEquality_NonNumericInput
    [TestMethod]
    public void TestEquality_NonNumericInput()
    {
        var f1 = new Feet(1.0);
        object invalid = "abc";

        Assert.IsFalse(f1.Equals(invalid), "Feet should not equal non-Feet object");
    }

    // testEquality_SameReference
    [TestMethod]
    public void TestEquality_SameReference()
    {
        var f1 = new Feet(1.0);

        Assert.IsTrue(f1.Equals(f1), "Same object reference should be equal");
    }
}
