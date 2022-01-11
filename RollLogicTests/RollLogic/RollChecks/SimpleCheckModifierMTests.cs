using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;
using System;

namespace RollLogicTests.RollLogic.RollChecks
{
    [TestFixture]
    public class SimpleCheckModifierMTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private SimpleCheckModifierM CreateSimpleCheckModifierM(int Value)
        {
            return new SimpleCheckModifierM(Value);
        }

        [Test, Repeat(10)]
        public void Apply_ModIs0_NoChanges()
        {
            const int ModValue = 0;

            // Arrange
            var simpleCheckModifierM = this.CreateSimpleCheckModifierM(ModValue);
            IRollM Roll = new D20Roll(null);
            var Before = Roll.Roll();

            // Act
            var result = simpleCheckModifierM.Apply(Roll);

            // Assert
            Assert.That(result, Is.EqualTo(Before));
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void Apply_D20_ModCausesDifference([Random(-5, 5, 10)] int ModValue)
        {

            // Arrange
            var simpleCheckModifierM = this.CreateSimpleCheckModifierM(ModValue);
            IRollM Roll = new D20Roll(null);
            var Before = Roll.Roll();

            // Act
            var result = simpleCheckModifierM.Apply(Roll);

            // Assert
            Assert.That(result[0] - ModValue, Is.EqualTo(Before[0]));
            this.mockRepository.VerifyAll();
        }
    }
}
