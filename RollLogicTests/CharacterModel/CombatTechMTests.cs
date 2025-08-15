using FateExplorer.GameData;
using FateExplorer.CharacterModel;
using Moq;
using NUnit.Framework;
using System;
using FateExplorer.Shared;

namespace UnitTests.CharacterModel
{
    [TestFixture]
    public class CombatTechMTests
    {
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
        }

        private CombatTechDbEntry SetupGameDataMock(CombatBranch branch, string primeAbility, string id)
        {
            bool canAttack = true; // all weapons can, even shields
            bool canParry = branch != CombatBranch.Ranged;

            CombatTechDbEntry Entry = new()
            {
                WeaponsBranch = branch,
                PrimeAttrID = primeAbility,
                Id = id,
                CanAttack = canAttack,
                CanParry = canParry,
                IsRanged = branch == CombatBranch.Ranged
            };
            return Entry;
        }

        private CombatTechM CreateCombatTechM(CombatTechDbEntry GameData, int CtSkill)
        {
            return new CombatTechM(
                GameData,
                CtSkill,
                this.mockCharacterM.Object);
        }


        [Test]
        [TestCase(6, CombatBranch.Melee, "TestId", AbilityM.COU, 10)]
        public void Instantiation(int CtValue, CombatBranch CtBranch, string CtId, string AbilityId, int Ability)
        {
            // Arrange
            mockCharacterM.Setup(c
                => c.GetAbility(It.Is<string>(s => s == AbilityM.COU))) // Melee
                .Returns(Ability);


            var GameData = SetupGameDataMock(CtBranch, AbilityId, CtId);

            // Act
            var combatTechM = this.CreateCombatTechM(GameData, CtValue);

            // Assert
            Assert.That(CtId, Is.EqualTo(combatTechM.Id));
            Assert.That(CtBranch == CombatBranch.Ranged, Is.EqualTo(combatTechM.IsRanged));
            Assert.That(CtValue, Is.EqualTo(combatTechM.Value));
        }



        [Test]
        [TestCase(6, 14, CombatBranch.Melee, ExpectedResult = 8, Description = "Louisa VR1, p. 51")]
        [TestCase(6, 13, CombatBranch.Melee, ExpectedResult = 7, Description = "One point less in courage should decrease skill")]
        [TestCase(12, 15, CombatBranch.Melee, ExpectedResult = 14, Description = "Chris VR1, p. 51")]
        [TestCase(12, 14, CombatBranch.Ranged, ExpectedResult = 14, Description = "Sarah VR1, p. 51")]
        [TestCase(12, 14, CombatBranch.Melee, ExpectedResult = 14, Description = "Same as before")]
        public int ComputeAttack__ReturnsExpected(int CtValue, int Ability, CombatBranch ct)
        {
            // Arrange
            if (ct == CombatBranch.Melee)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.COU))) // Melee
                    .Returns(Ability); 
            else if (ct == CombatBranch.Ranged)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.DEX))) // Ranged
                    .Returns(Ability);
            var GameData = SetupGameDataMock(ct, ct == CombatBranch.Melee ? AbilityM.COU : AbilityM.DEX, "MyId");
            var combatTechM = this.CreateCombatTechM(GameData, CtValue);
            int EffectiveBase = Ability;

            // Act
            var result = combatTechM.ComputeAttack(EffectiveBase);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }



        [Test]
        [TestCase(6,  14, CombatBranch.Melee, ExpectedResult = 5, Description = "Louisa VR1, p. 51")]
        [TestCase(12, 12, CombatBranch.Melee, ExpectedResult = 7, Description = "Chris VR1, p. 51")]
        [TestCase(12, 14, CombatBranch.Ranged, ExpectedResult = 0, Description = "Sarah VR1, p. 51")]
        public int ComputeParry__ExpectedBehavior(int CtValue, int Ability, CombatBranch ct)
        {
            // Arrange
            if (ct == CombatBranch.Melee)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.COU || s == AbilityM.AGI))) // Melee
                    .Returns(Ability);
            else if (ct == CombatBranch.Ranged)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.DEX))) // Ranged
                    .Returns(Ability);


            var GameData = SetupGameDataMock(ct, ct == CombatBranch.Melee ? AbilityM.AGI : AbilityM.DEX, "MyId");
            var combatTechM = this.CreateCombatTechM(GameData, CtValue);
            int EffectivePrimary = Ability;

            // Act
            var result = combatTechM.ComputeParry(EffectivePrimary);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }
    }
}
