using FateExplorer.GameData;
using FateExplorer.GameLogic;
using Moq;
using NUnit.Framework;
using System;

namespace RollLogicTests.GameLogic
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

        private CombatTechDbEntry SetupGameDataMock(CombatTechniques branch, string primeAbility, string id)
        {
            bool canAttack = true; // all weapons can, even shields
            bool canParry = branch != CombatTechniques.Ranged;

            CombatTechDbEntry Entry = new()
            {
                WeaponsBranch = branch,
                PrimeAttrID = primeAbility,
                Id = id,
                CanAttack = canAttack,
                CanParry = canParry,
                IsRanged = branch == CombatTechniques.Ranged
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
        [TestCase(6, CombatTechniques.Melee, "TestId", AbilityM.COU, 10)]
        public void Instantiation(int CtValue, CombatTechniques CtBranch, string CtId, string AbilityId, int Ability)
        {
            // Arrange
            mockCharacterM.Setup(c
                => c.GetAbility(It.Is<string>(s => s == AbilityM.COU))) // Melee
                .Returns(Ability);


            var GameData = SetupGameDataMock(CtBranch, AbilityId, CtId);

            // Act
            var combatTechM = this.CreateCombatTechM(GameData, CtValue);

            // Assert
            Assert.AreEqual(CtId, combatTechM.Id);
            Assert.AreEqual(CtBranch == CombatTechniques.Ranged, combatTechM.IsRanged);
            Assert.AreEqual(CtValue, combatTechM.Value);
        }



        [Test]
        [TestCase(6, 14, CombatTechniques.Melee, ExpectedResult = 8, Description = "Louisa VR1, p. 51")]
        [TestCase(6, 13, CombatTechniques.Melee, ExpectedResult = 7, Description = "One point less in courage should decrease skill")]
        [TestCase(12, 15, CombatTechniques.Melee, ExpectedResult = 14, Description = "Chris VR1, p. 51")]
        [TestCase(12, 14, CombatTechniques.Ranged, ExpectedResult = 14, Description = "Sarah VR1, p. 51")]
        [TestCase(12, 14, CombatTechniques.Melee, ExpectedResult = 14, Description = "Same as before")]
        public int ComputeAttack__ReturnsExpected(int CtValue, int Ability, CombatTechniques ct)
        {
            // Arrange
            if (ct == CombatTechniques.Melee)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.COU))) // Melee
                    .Returns(Ability); 
            else if (ct == CombatTechniques.Ranged)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.DEX))) // Ranged
                    .Returns(Ability);
            var GameData = SetupGameDataMock(ct, ct == CombatTechniques.Melee ? AbilityM.COU : AbilityM.DEX, "MyId");
            var combatTechM = this.CreateCombatTechM(GameData, CtValue);
            int EffectiveBase = Ability;

            // Act
            var result = combatTechM.ComputeAttack(EffectiveBase);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }



        [Test]
        [TestCase(6,  14, CombatTechniques.Melee, ExpectedResult = 5, Description = "Louisa VR1, p. 51")]
        [TestCase(12, 12, CombatTechniques.Melee, ExpectedResult = 7, Description = "Chris VR1, p. 51")]
        [TestCase(12, 14, CombatTechniques.Ranged, ExpectedResult = 0, Description = "Sarah VR1, p. 51")]
        public int ComputeParry__ExpectedBehavior(int CtValue, int Ability, CombatTechniques ct)
        {
            // Arrange
            if (ct == CombatTechniques.Melee)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.COU || s == AbilityM.AGI))) // Melee
                    .Returns(Ability);
            else if (ct == CombatTechniques.Ranged)
                mockCharacterM.Setup(c
                    => c.GetAbility(It.Is<string>(s => s == AbilityM.DEX))) // Ranged
                    .Returns(Ability);


            var GameData = SetupGameDataMock(ct, ct == CombatTechniques.Melee ? AbilityM.AGI : AbilityM.DEX, "MyId");
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
