using FateExplorer.CharacterModel;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
{
    [TestFixture]
    public class DodgeMTests
    {
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
        }

        private DodgeM CreateDodgeM()
        {
            return new DodgeM(this.mockCharacterM.Object);
        }


        [Test, Sequential]
        public void Dodge([Values(4,8,16,19)] int doVal, [Values(2,4,8,10)] int Expected)
        {
            // Arrange
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == AbilityM.AGI)))
                .Returns(doVal);

            // Act
            var dodgeM = this.CreateDodgeM();

            // Assert
            Assert.AreEqual(Expected, dodgeM.Value);
            mockCharacterM.Verify(c => c.GetAbility(It.Is<string>(s => s == AbilityM.AGI)), Times.Once);
        }

        [Test]
        public void ComputeDodge_Layariel()
        {
            // Arrange
            int EffectiveAgility = HeroWipfelglanz.AbilityValues[AbilityM.AGI];

            // Act
            var result = DodgeM.ComputeDodge(EffectiveAgility);

            // Assert
            Assert.AreEqual(HeroWipfelglanz.Dodge, result);
            this.mockRepository.VerifyAll();
        }



        [Test]
        public void ComputeDodge_Arbosch()
        {
            // Arrange
            int EffectiveAgility = HeroArbosch.AbilityValues[AbilityM.AGI];

            // Act
            var result = DodgeM.ComputeDodge(EffectiveAgility);

            // Assert
            Assert.AreEqual(HeroArbosch.Dodge, result);
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void ComputeDodge_Grassberger()
        {
            // Arrange
            int EffectiveAgility = HeroGrassberger.AbilityValues[AbilityM.AGI];

            // Act
            var result = DodgeM.ComputeDodge(EffectiveAgility);

            // Assert
            Assert.AreEqual(HeroGrassberger.Dodge, result);
            this.mockRepository.VerifyAll();
        }
    }
}
