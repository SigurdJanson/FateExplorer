using FateExplorer.RollLogic;
using NUnit.Framework;
using System;

namespace UnitTests.RollLogic
{
    [TestFixture]
    public class HalfModifierTests
    {
        [Test, Ignore("Not implemented")]
        public void Apply_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var halfModifier = new HalfModifier();
            IRollM Before = null;

            // Act
            var result = halfModifier.Apply(
                Before);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Apply_OddNumbers_RoundedHalf()
        {
            int[] Expected= new int[7] { 1, 2, 3, 4, 5, 6, 7 };

            // Arrange
            var halfModifier = new HalfModifier();
            int[] Before = new int[Expected.Length];
            for (int i = 0; i < Expected.Length; i++)
            {
                Before[i] = Expected[i] * 2 - 1;
            }

            // Act
            var result = halfModifier.Apply(Before);

            // Assert
            Assert.AreEqual(Expected, result);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(2, 1)]
        [TestCase(4, 2)]
        [TestCase(10, 5)]
        [TestCase(12, 6)]
        public void Apply_EvenNumbers_ExactHalf(int Before, int After)
        {
            // Arrange
            var halfModifier = new HalfModifier();

            // Act
            var result = halfModifier.Apply(Before);

            // Assert
            Assert.AreEqual(After, result);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(11, 6)]
        [TestCase(13, 7)]
        public void Apply_OddNumbers_RoundedHalf(int Before, int After)
        {
            // Arrange
            var halfModifier = new HalfModifier();

            // Act
            var result = halfModifier.Apply(Before);

            // Assert
            Assert.AreEqual(After, result);
        }

    }
}
