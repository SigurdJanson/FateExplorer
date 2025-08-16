using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
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


        private static CombatTechDB MockCombatTechDB()
        {
            var Result = new CombatTechDB();
            var ResultList = new List<CombatTechDbEntry>();

            var Entry = MockUnarmedCombatTechDbEntry();
            ResultList.Add(Entry);
            Result.Data = ResultList;

            return Result;
        }

        private static CombatTechDbEntry MockUnarmedCombatTechDbEntry()
            => new()
            {
                Id = "CT_9",
                PrimeAttrID = "ATTR_6/ATTR_8",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Unarmed
            };

        private static WeaponMeleeDB MockGameDataWeaponMelee()
        {
            var Result = new WeaponMeleeDB();
            List<WeaponMeleeDbEntry> Weapons = new();
            var Entry = new WeaponMeleeDbEntry()
            {
                Id = "WEAPONLESS",
                TemplateID = "WEAPONLESS",
                Name = "Brawling",
                CombatTechID = "CT_9",
                AtMod = 0,
                PaMod = 0,
                Bonus = 0,
                Reach = WeaponsReach.Short,
                Damage = "1W6",
                Threshold = 21,
                CloseRange = true,
                TwoHanded = false,
                Improvised = false,
                Parry = false,
                Armed = false,
                PrimeAttrID = "ATTR_6/ATTR_8",
                Price = 0,
                Sf = 12,
                Url = "",
                Weight = 0
            };
            Weapons.Add(Entry);
            Result.Data = Weapons;
            return Result;
        }

        private Dictionary<string,CombatTechM> MockCombatTechCollection(int SkillValue)
        {
            CombatTechDbEntry combatTechDbEntry = MockUnarmedCombatTechDbEntry();
            CombatTechM CtUnarmed = new(combatTechDbEntry, SkillValue, mockCharacterM.Object);
            Dictionary<string, CombatTechM> Result = new()
            {
                { CombatTechM.Unarmed, CtUnarmed }
            };
            return Result;
        }


        private static Dictionary<string, IActivatableM> MockSpecialAbilities(int Tier)
        {
            Dictionary<string, IActivatableM> Result = new()
            {
                { SA.TwoHandedCombat, new TieredActivatableM(SA.TwoHandedCombat, Tier, null) }
            };
            return Result;
        }


        /// <summary>
        /// Set up mocks for special abilities and advantages
        /// </summary>
        private void MockCompensation(bool IsAmbidext, int TwoHandedTier)
        {
            if (TwoHandedTier < 0 || TwoHandedTier > 2) 
                throw new ArgumentOutOfRangeException(nameof(TwoHandedTier));

            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(a => a == ADV.Ambidextrous)))
                .Returns(IsAmbidext);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(a => a == SA.TwoHandedCombat)))
                .Returns(TwoHandedTier > 0);
            mockCharacterM.SetupGet(p => p.SpecialAbilities).Returns(MockSpecialAbilities(TwoHandedTier));
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
        public void WeaponStats_RightHand_UnarmedLeft_BaseValues(bool IsAmbidext, bool isTwoHanded)
        {
            const bool MainHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();

            MockCompensation(IsAmbidext, isTwoHanded ? 2 : 0);
            mockGameDB.SetupGet(p => p.WeaponsMelee)
                .Returns(MockGameDataWeaponMelee());

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.That(7, Is.EqualTo(Fist.BaseAtSkill));
            Assert.That(7, Is.EqualTo(Fist.AtSkill(MainHand, CombatBranch.Unarmed)));

            Assert.That(5, Is.EqualTo(Fist.BasePaSkill));
            Assert.That(5, Is.EqualTo(Fist.PaSkill(MainHand, CombatBranch.Unarmed, false, 0)));

            Assert.That(1, Is.EqualTo(Fist.DamageDieCount));
            Assert.That(6, Is.EqualTo(Fist.DamageDieSides));
            Assert.That(0, Is.EqualTo(Fist.DamageBonus));

            //this.mockRepository.VerifyAll();
        }



        // two-handed weapon fighting should make no effect here
        // Advantage Ambidexterous should make a difference
        [Test] // Layariel: Unarmed TP 1W6, AT 7, PA 5
        [TestCase(false, false, -4)]
        [TestCase(true, false, 0)]
        [TestCase(false, true, -4)]
        [TestCase(true, true, 0)]
        public void WeaponStats_LeftHand_UnarmedRight_YieldsOffhandPenalty(bool IsAmbidext, bool isTwoHanded, int ExpectedPenalty)
        {
            const bool OffHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();

            this.MockCompensation(IsAmbidext, isTwoHanded ? 2 : 0);
            mockGameDB.SetupGet(p => p.WeaponsMelee)
                .Returns(MockGameDataWeaponMelee());

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.That(7, Is.EqualTo(Fist.BaseAtSkill));
            Assert.That(7 + ExpectedPenalty, Is.EqualTo(Fist.AtSkill(!OffHand, CombatBranch.Unarmed)));

            Assert.That(5, Is.EqualTo(Fist.BasePaSkill));
            Assert.That(5 + ExpectedPenalty, Is.EqualTo(Fist.PaSkill(!OffHand, CombatBranch.Unarmed, false, 0)));

            Assert.That(1, Is.EqualTo(Fist.DamageDieCount));
            Assert.That(6, Is.EqualTo(Fist.DamageDieSides));
            Assert.That(0, Is.EqualTo(Fist.DamageBonus));

            //this.mockRepository.VerifyAll();
        }


        // Two-handed weapon fighting is crucial
        // Advantage Ambidexterous makes no difference
        // 
        [Test] // Layariel: Unarmed TP 1W6, AT 7, PA 5
        [TestCase(false, false, CombatBranch.Melee, -2)]
        [TestCase(false, false, CombatBranch.Ranged, -2)]
        [TestCase(false, false, CombatBranch.Shield, 0)]
        [TestCase(true, false, CombatBranch.Melee, -2)]
        [TestCase(true, false, CombatBranch.Ranged, -2)]
        [TestCase(true, false, CombatBranch.Shield, 0)]
        [TestCase(false, true, CombatBranch.Melee, 0)]
        [TestCase(false, true, CombatBranch.Ranged, 0)]
        [TestCase(false, true, CombatBranch.Shield, 0)]
        [TestCase(true, true, CombatBranch.Melee, 0)]
        [TestCase(true, true, CombatBranch.Ranged, 0)]
        [TestCase(true, true, CombatBranch.Shield, 0)]
        public void WeaponStats_RightHand_ArmedLeft_YieldsTwohandPenalty(bool IsAmbidext, bool isTwoHanded, CombatBranch OffHandWeapon, int ExpectedPenalty)
        {
            const bool MainHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();

            this.MockCompensation(IsAmbidext, isTwoHanded ? 2 : 0);
            mockGameDB.SetupGet(p => p.WeaponsMelee)
                .Returns(MockGameDataWeaponMelee());

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.That(7, Is.EqualTo(Fist.BaseAtSkill));
            Assert.That(7 + ExpectedPenalty, Is.EqualTo(Fist.AtSkill(MainHand, OffHandWeapon)));

            Assert.That(5, Is.EqualTo(Fist.BasePaSkill));
            Assert.That(5 + ExpectedPenalty, Is.EqualTo(Fist.PaSkill(MainHand, OffHandWeapon, false, 0)));

            Assert.That(1, Is.EqualTo(Fist.DamageDieCount));
            Assert.That(6, Is.EqualTo(Fist.DamageDieSides));
            Assert.That(0, Is.EqualTo(Fist.DamageBonus));

            //this.mockRepository.VerifyAll();
        }




        // Two-handed weapon fighting is in effect
        // Advantage Ambidexterous is in effect
        // 
        [Test] // Layariel: Unarmed TP 1W6, AT 7, PA 5
        [TestCase(false, false, CombatBranch.Melee, -6)]
        [TestCase(false, false, CombatBranch.Ranged, -6)]
        [TestCase(false, false, CombatBranch.Shield, -4)]
        [TestCase(true, false, CombatBranch.Melee, -2)]
        [TestCase(true, false, CombatBranch.Ranged, -2)]
        [TestCase(true, false, CombatBranch.Shield, 0)]
        [TestCase(false, true, CombatBranch.Melee, -4)]
        [TestCase(false, true, CombatBranch.Ranged, -4)]
        [TestCase(false, true, CombatBranch.Shield, -4)]
        [TestCase(true, true, CombatBranch.Melee, 0)]
        [TestCase(true, true, CombatBranch.Ranged, 0)]
        [TestCase(true, true, CombatBranch.Shield, 0)]
        public void WeaponStats_OffHand_ArmedMain_YieldsBothPenalties(bool IsAmbidext, bool isTwoHanded, CombatBranch OffHandWeapon, int ExpectedPenalty)
        {
            const bool MainHand = true;
            // Arrange
            var Fist = this.CreateLayarielsWeaponUnarmedM();

            this.MockCompensation(IsAmbidext, isTwoHanded ? 2 : 0);
            mockGameDB.SetupGet(p => p.WeaponsMelee)
                .Returns(MockGameDataWeaponMelee());

            // Act
            Fist.Initialise(mockGameDB.Object);

            // Assert
            Assert.That(7, Is.EqualTo(Fist.BaseAtSkill));
            Assert.That(7 + ExpectedPenalty, Is.EqualTo(Fist.AtSkill(!MainHand, OffHandWeapon)));

            Assert.That(5, Is.EqualTo(Fist.BasePaSkill));
            Assert.That(Math.Max(5 + ExpectedPenalty, 0), Is.EqualTo(Fist.PaSkill(!MainHand, OffHandWeapon, false, 0)));

            Assert.That(1, Is.EqualTo(Fist.DamageDieCount));
            Assert.That(6, Is.EqualTo(Fist.DamageDieSides));
            Assert.That(0, Is.EqualTo(Fist.DamageBonus));

            //this.mockRepository.VerifyAll();
        }
    }
}
