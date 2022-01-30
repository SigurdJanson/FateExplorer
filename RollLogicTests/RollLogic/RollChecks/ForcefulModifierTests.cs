using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;
using System;

namespace RollLogicTests.RollLogic
{
    [TestFixture]
    public class ForcefulModifierTests
    {
        private MockRepository mockRepository;
        private Mock<IRollM> MockedRoll;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new(MockBehavior.Strict);

            MockedRoll = mockRepository.Create<IRollM>();
        }





        [Test]
        public void ApplyRoll__YieldsForcedValueOnAllPlaces([Values(1, 3, 6)] int Expected)
        {
            int[] Before = new int[3] { 2, 4, 6 };

            // Arrange
            var forcefulModifier = new ForcefulModifier(Expected);
            MockedRoll.Setup(s => s.OpenRoll).Returns(Before);

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.AreEqual(
                new int[3] { Expected, Expected, Expected },
                result);
        }



        [Test]
        public void ApplyArray__YieldsForcedValue(
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
