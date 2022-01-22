using FateExplorer.RollLogic;
using NUnit.Framework;
using System;

namespace RollLogicTests.RollLogic.RollChecks
{
    [TestFixture]
    public class ForcefulModifierTests
    {
        [Test, Ignore("Not yet implemented")]
        public void Apply_StateUnderTest_ExpectedBehavior(int New)
        {
            // Arrange
            var forcefulModifier = new ForcefulModifier(New);
            IRollM Before = null;

            // Act
            var result = forcefulModifier.Apply(
                Before);

            // Assert
            Assert.Fail();
        }



        [Test]
        public void Apply_Array_YieldsForcedValue(
            [Random(0, 50, 10)] int Expected)
        {
            const int Length = 3;
            // Arrange
            var forcefulModifier = new ForcefulModifier(Expected);
            int[] Before = new int[Length] { 2, 4, 6 };

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.AreEqual(
                new int[Length] { Expected, Expected, Expected },
                result);
        }



        [Test]
        public void Apply_SingleValue_YieldsForcedValue(
            [Random(0, 50, 5)] int Before,
            [Random(0, 50, 5)] int Expected)
        {
            // Arrange
            var forcefulModifier = new ForcefulModifier(Expected);

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.AreEqual(Expected, result);
        }



        //[Test, Ignore("Too simple")]
        //public void Set_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var forcefulModifier = new ForcefulModifier(2);
        //    int value = 0;

        //    // Act
        //    forcefulModifier.Set(value);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
