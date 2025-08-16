using FateExplorer.RollLogic;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.RollLogic
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
            var forcefulModifier = new SimpleCheckModificatorM(new Modifier(Expected, Modifier.Op.Force));
            MockedRoll.Setup(s => s.OpenRoll).Returns(Before);

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.That(
                new int[3] { Math.Min(Expected, Before[0]), Math.Min(Expected, Before[1]), Math.Min(Expected, Before[2]) },
                Is.EqualTo(result));
        }



        [Test]
        public void ApplyArray__YieldsForcedValue(
            [Random(0, 40, 5)] int Expected)
        {
            const int Length = 3;
            // Arrange
            var forcefulModifier = new SimpleCheckModificatorM(new Modifier(Expected, Modifier.Op.Force));
            int[] Before = new int[Length] { 2, 4, 6 };

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.That(
                new int[Length] { Math.Min(Expected, Before[0]), Math.Min(Expected, Before[1]), Math.Min(Expected, Before[2]) },
                Is.EqualTo(result));
        }



        [Test]
        public void Apply_SingleValue_YieldsForcedValue(
            [Values(1, 15, 30)] int Before,
            [Random(0, 40, 5)] int Expected)
        {
            // Arrange
            var forcefulModifier = new SimpleCheckModificatorM(new Modifier(Expected, Modifier.Op.Force));

            // Act
            var result = forcefulModifier.Apply(Before);

            // Assert
            Assert.That(Math.Min(Expected, Before), Is.EqualTo(result));
        }


    }
}
