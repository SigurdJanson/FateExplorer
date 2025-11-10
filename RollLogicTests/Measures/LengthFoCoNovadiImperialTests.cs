using Aventuria.Measures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Measures;

internal class LengthFoCoNovadiImperialTests
{
    [SetUp]
    public void Setup()
    {
    }


    [TestCase(0.0, ExpectedResult = 0.0)]
    [TestCase(10.0, ExpectedResult = 10.0 / 15000 * 0.9144)]
    public double ToBaryd_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoNovadiImperial.ToBaryd(value);

        // Assert
        return result;
    }
}
