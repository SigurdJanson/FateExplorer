using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace vmCode_UnitTests.CharacterModel
{
    [TestFixture]
    public class WeaponUnarmedMTests
    {
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;
        private Mock<IGameDataService> mockGameDB;


        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockCharacterM = this.mockRepository.Create<ICharacterM>();

            // needed to get the primary attribute from the data base
            mockGameDB = this.mockRepository.Create<IGameDataService>();
        }


        private CombatTechDB MockCombatTechDB()
        {
            var Result = new CombatTechDB();
            var ResultList = new List<CombatTechDbEntry>();

            var Entry = MockUnarmedCombatTechDbEntry();
            ResultList.Add(Entry);
            Result.Data = ResultList;

            return Result;
        }

        private CombatTechDbEntry MockUnarmedCombatTechDbEntry()
            => new CombatTechDbEntry()
            {
                Id = "CT_9",
                PrimeAttrID = "ATTR_6/ATTR_8",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Unarmed
            };



        private Dictionary<string,CombatTechM> MockCombatTechCollection(int SkillValue)
        {
            CombatTechDbEntry combatTechDbEntry = MockUnarmedCombatTechDbEntry();
            CombatTechM CtUnarmed = new(combatTechDbEntry, SkillValue, mockCharacterM.Object);
            Dictionary<string, CombatTechM> Result = new();
            Result.Add(CombatTechM.Unarmed, CtUnarmed);
            return Result;
        }


        private Dictionary<string, IActivatableM> MockSpecialAbilities(int Tier)
        {
            Dictionary<string, IActivatableM> Result = new();
            Result.Add(SA.TwoHandedCombat, new TieredActivatableM(SA.TwoHandedCombat, Tier));
            return Result;
        }


        private WeaponUnarmedM CreateLayarielsWeaponUnarmedM()
        {
            //mockCharacterM.SetupGet(x => x.Name).Returns("Layariel Wipfelglanz"); // just for show

            var Abs = HeroWipfelglanz.AbilityValues;
            foreach (var a in Abs)
                mockCharacterM.Setup(x => x.GetAbility(It.Is<string>(s => s == a.Key)))
                    .Returns(a.Value);
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).Returns(MockCombatTechCollection(HeroWipfelglanz.UnarmedSkill));

            // needed to get the primary attribute from the data base
            mockGameDB.SetupGet(p => p.CombatTechs).Returns(MockCombatTechDB());

            return new WeaponUnarmedM(this.mockCharacterM.Object);
        }



        // Two-handed weapon fighting should make no effect here
        // Advantage Ambidexterous shall NOT have an effect here
        [Test] // Layariel: Unarmed TP 1W6, AT 7, PA 5
        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Points_RightHand_UnarmedLeft_BaseValues(bool IsAmbidext, bool isTwoHanded)
        {
            const bool MainHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();
            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(a => a == ADV.Ambidexterous)))
                .Returns(IsAmbidext);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(a => a == SA.TwoHandedCombat)))
                .Returns(isTwoHanded);
            mockCharacterM.SetupGet(p => p.SpecialAbilities).Returns(MockSpecialAbilities(2));

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.AreEqual(7, Fist.BaseAtSkill);
            Assert.AreEqual(7, Fist.AtSkill(MainHand, CombatBranch.Unarmed));

            Assert.AreEqual(5, Fist.BasePaSkill);
            Assert.AreEqual(5, Fist.PaSkill(MainHand, CombatBranch.Unarmed, 0, false));

            Assert.AreEqual(1, Fist.DamageDieCount);
            Assert.AreEqual(6, Fist.DamageDieSides);
            Assert.AreEqual(0, Fist.DamageBonus);

            //this.mockRepository.VerifyAll();
        }



        // two-handed weapon fighting should make no effect here
        // Advantage Ambidexterous should make a difference
        [Test] // Layariel: Unarmed TP 1W6, AT 7, PA 5
        [TestCase(false, false, -4)]
        [TestCase(true, false, 0)]
        [TestCase(false, true, -4)]
        [TestCase(true, true, 0)]
        public void Points_LeftHand_UnarmedRight_YieldsOffhandPenalty(bool IsAmbidext, bool isTwoHanded, int ExpectedPenalty)
        {
            const bool OffHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();
            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(a => a == ADV.Ambidexterous)))
                .Returns(IsAmbidext);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(a => a == SA.TwoHandedCombat)))
                .Returns(isTwoHanded);
            mockCharacterM.SetupGet(p => p.SpecialAbilities).Returns(MockSpecialAbilities(2));

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.AreEqual(7, Fist.BaseAtSkill);
            Assert.AreEqual(7 + ExpectedPenalty, Fist.AtSkill(OffHand, CombatBranch.Unarmed));

            Assert.AreEqual(5, Fist.BasePaSkill);
            Assert.AreEqual(5 + ExpectedPenalty, Fist.PaSkill(OffHand, CombatBranch.Unarmed, 0, false));

            Assert.AreEqual(1, Fist.DamageDieCount);
            Assert.AreEqual(6, Fist.DamageDieSides);
            Assert.AreEqual(0, Fist.DamageBonus);

            //this.mockRepository.VerifyAll();
        }
    }
}
