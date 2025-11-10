using Aventuria.Measures;
using NUnit.Framework;

namespace UnitTests.Measures;

public class LengthFoCoNovadiMetricTests
{
    [SetUp]
    public void Setup()
    {
    }


    [TestCase(0.0, ExpectedResult = 0.0)]
    [TestCase(10.0, ExpectedResult = 10.0/15000)]
    public double ToBaryd_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoNovadiMetric.ToBaryd(value);

        // Assert
        return result;
    }
}
