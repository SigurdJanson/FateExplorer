﻿using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
{

    [TestFixture]
    //[TestOf(typeof(WeaponM))]
    public partial class WeaponMTests
    {
        //
        //
        // 
        #region DATA SOURCES ###############

        private static CombatTechDbEntry CombatTechDaggers
        {
            get => new()
            {
                Name = "Dolche",
                Id = "CT_3",
                PrimeAttrID = "ATTR_6",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Melee
            };
        }

        private static CombatTechDbEntry CombatTechSwords
        {
            get => new()
            {
                Name = "Schwerter",
                Id = "CT_12",
                PrimeAttrID = "ATTR_6/ATTR_8",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Melee
            };
        }

        private static CombatTechDbEntry CombatTechBows
        {
            get => new()
            {
                Name = "Bögen",
                Id = "CT_2",
                PrimeAttrID = "ATTR_5",
                CanAttack = true,
                CanParry = false,
                IsRanged = true,
                WeaponsBranch = CombatBranch.Ranged
            };
        }

        private static CombatTechM MakeCombatTechM(CombatTechDbEntry ct, int Skill, ICharacterM hero)
        {
            CombatTechM Result = new(ct, Skill, hero);
            return Result;
        }

        private Dictionary<string, CombatTechM> MakeCombatTechDict(int Skill, ICharacterM character)
        {
            Dictionary<string, CombatTechM> Result = new();

            Result.Add(CombatTechDaggers.Id, MakeCombatTechM(CombatTechDaggers, Skill, character));
            Result.Add(CombatTechBows.Id, MakeCombatTechM(CombatTechBows, Skill, character));
            Result.Add(CombatTechSwords.Id, MakeCombatTechM(CombatTechSwords, Skill, character));

            return Result;
        }



        private static IEnumerable<TestCaseData> LayarielsWeapons
        {
            get
            {
                yield return new TestCaseData(HeroWipfelglanz.LayarielsDagger);
                yield return new TestCaseData(HeroWipfelglanz.LayarielsElvenBow);
            }
        }


        private static IEnumerable<TestCaseData> LayarielsWeaponsHitPointBonus
        {
            get
            {
                yield return new TestCaseData(HeroWipfelglanz.LayarielsDagger, 1);
                yield return new TestCaseData(HeroWipfelglanz.LayarielsElvenBow, 0);
            }
        }

        #endregion




        //
        //
        // SETTING UP THE TEST FIXTURE
        #region SETUP ###############
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;
        private Mock<IGameDataService> mockGameDataM;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
            mockCharacterM.SetupGet(x => x.Name).Returns("Layariel Wipfelglanz");
            var Abs = HeroWipfelglanz.AbilityValues;
            foreach(var a in Abs)
                mockCharacterM.Setup(x => x.GetAbility(It.Is<string>(s => s == a.Key)))
                    .Returns(a.Value);

            this.mockGameDataM = this.mockRepository.Create<IGameDataService>();
        }


        private WeaponM CreateWeaponM()
        {
            return new WeaponM(this.mockCharacterM.Object);
        }

        #endregion



        //
        //
        // TESTS

        [Test, Description("Layariels primary ability increase *dagger* hit points by 1")]
        public void AT_SingleWeapon_MainHand_DamageIncreasedBy1()
        {
            // Arrange
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).
                Returns(HeroWipfelglanz.CombatTechs(mockCharacterM.Object));

            WeaponDTO WeaponData = HeroWipfelglanz.LayarielsDagger;
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.DamageBonus;

            // Assert
            Assert.AreEqual(HeroWipfelglanz.LayarielsDagger.DamageBonus + 1, result);
            Assert.AreEqual(1, weaponM.DamageDieCount);
            Assert.AreEqual(6, weaponM.DamageDieSides);
            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_6")), Times.AtLeastOnce);
        }


        [Test]
        [TestCase(CombatBranch.Unarmed)]
        [TestCase(CombatBranch.Shield)]
        public void AtSkill_CtDagger_EmptyOffhand_GivesSkillValue(CombatBranch otherHand)
        {
            const int LayarielsDaggerAttackVal = 9;

            // Arrange
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).
                Returns(HeroWipfelglanz.CombatTechs(mockCharacterM.Object));
            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)))
                .Returns(false);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)))
                .Returns(false);

            WeaponDTO WeaponData = HeroWipfelglanz.LayarielsDagger;
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = true;

            // Act
            var result = weaponM.AtSkill(MainHand, otherHand);

            // Assert
            Assert.AreEqual(LayarielsDaggerAttackVal, result);

            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_6")), Times.AtLeastOnce);

            mockCharacterM.Verify(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)), Times.Once);
            mockCharacterM.Verify(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)), Times.Once);
        }



        [Test]
        public void PaSkill_CtDagger_EmptyOffhand_GivesSkillValue()
        {
            const int LayarielsDaggerParryVal = 6;

            // Arrange
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).
                Returns(HeroWipfelglanz.CombatTechs(mockCharacterM.Object));
            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)))
                .Returns(false);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)))
                .Returns(false);

            WeaponDTO WeaponData = HeroWipfelglanz.LayarielsDagger;
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = true;
            CombatBranch otherHand = CombatBranch.Unarmed;
            bool otherIsParry = false;
            int otherPaSkill = 0;

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, otherPaSkill);

            // Assert
            Assert.AreEqual(LayarielsDaggerParryVal, result);
            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_6")), Times.AtLeastOnce);

            mockCharacterM.Verify(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)), Times.Once);
            mockCharacterM.Verify(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)), Times.Once);
        }



        [Test, Description("A shield in the other hand adds the passive weapon bonus to the parry value")]
        public void PaSkill_CtDagger_OffhandShield_AddsPassiveShieldToParry([Values(1, 2, 4)] int shieldPaSkill)
        {
            const int LayarielsDaggerParryVal = 6;
            const bool MainHand = true;
            const bool otherIsParry = false;
            const CombatBranch otherHand = CombatBranch.Shield;

            // Arrange
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).
                Returns(HeroWipfelglanz.CombatTechs(mockCharacterM.Object));
            mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)))
                .Returns(false);
            mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)))
                .Returns(false);

            WeaponDTO WeaponData = HeroWipfelglanz.LayarielsDagger;
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, shieldPaSkill);


            // Assert
            Assert.AreEqual(LayarielsDaggerParryVal + shieldPaSkill, result);
            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_6")), Times.AtLeastOnce);

            mockCharacterM.Verify(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)), Times.Once);
            mockCharacterM.Verify(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)), Times.Once);
        }



        [Test, Ignore("nyi")]
        public void Initialise_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            WeaponDTO WeaponData = HeroWipfelglanz.LayarielsDagger;

            var weaponM = this.CreateWeaponM();

            // Act
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }


        [Test, Ignore("nyi")]
        public void ComputeAttackVal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            Dictionary<string,AbilityM> Abilities = null;
            //Dictionary<string,CombatTechM> CombatTecSkill = null;
            CombatTechM CombatTecSkill = null;

            // Act
            var result = weaponM.ComputeAttackVal(Abilities, CombatTecSkill);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }


        [Test, Ignore("nyi")]
        public void ComputeParryVal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(HeroWipfelglanz.LayarielsDagger, mockGameDataM.Object);
            CombatTechM CombatTecSkill = null;

            // Act
            var result = weaponM.ComputeParryVal(HeroWipfelglanz.Abilities, CombatTecSkill);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }


        [Test]
        [TestCaseSource(nameof(LayarielsWeaponsHitPointBonus))]
        public void HitpointBonus_VaryingPrimaryAbility(
            WeaponDTO Weapon,
            int Expected)
        {
            // Arrange
            mockCharacterM.SetupGet(p => p.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.SetupGet(p => p.CombatTechs).
                Returns(HeroWipfelglanz.CombatTechs(mockCharacterM.Object));

            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(Weapon, mockGameDataM.Object);

            // Act
            var result = weaponM.HitpointBonus(HeroWipfelglanz.Abilities);

            // Assert
            Assert.AreEqual(Expected, result);
            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_6")), Times.AtLeastOnce);

        }



        [Test]
        [TestCase("CT_3", ExpectedResult = true)]
        [TestCase("CT_12", ExpectedResult = true)]
        [TestCase("CT_2", ExpectedResult = false)]
        public bool CanParry(string CombatTechId)
        {
            Assume.That(mockCharacterM.Object, Is.Not.Null);

            // Arrange collection combat technique in the character
            Dictionary<string, CombatTechM> TestCombatTechs = MakeCombatTechDict(10, mockCharacterM.Object);
            mockCharacterM.SetupGet(c => c.CombatTechs).Returns(TestCombatTechs);

            // Arrange weapon
            var weaponM = this.CreateWeaponM();
            weaponM.CombatTechId = CombatTechId;

            // Act
            bool Result = weaponM.CanParry;

            // Assert
            return Result;
        }
    }
}
