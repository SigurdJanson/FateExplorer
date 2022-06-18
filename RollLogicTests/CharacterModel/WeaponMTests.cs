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
    [TestOf(typeof(WeaponM))]
    public class WeaponMTests
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
            mockRepository = new MockRepository(MockBehavior.Strict);
            mockGameDataM = mockRepository.Create<IGameDataService>();
        }


        private WeaponM CreateWeaponM()
        {
            return new WeaponM(mockCharacterM.Object);
        }


        public enum TestHeroes { Layariel, Arbosch, Grassberger }

        private void MockHero(TestHeroes Hero, 
            bool mockAbs, bool mockCT, 
            bool? isAmbi = null, int? TwoHandedTier = null)
        {
            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();

            string Name = Hero switch
            {
                TestHeroes.Layariel => "Layariel Wipfelglanz",
                TestHeroes.Arbosch => "Arbosch Sohn des Angrax",
                TestHeroes.Grassberger => "Ulf Grassberger",
                _ => throw new NotImplementedException("Hero does not exist")
            };
            mockCharacterM.SetupGet(x => x.Name).Returns(Name);

            // AbilityValues
            var AbVals = Hero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.AbilityValues,
                TestHeroes.Arbosch => HeroArbosch.AbilityValues,
                TestHeroes.Grassberger => HeroGrassberger.AbilityValues,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            foreach (var a in AbVals)
                mockCharacterM.Setup(x => x.GetAbility(It.Is<string>(s => s == a.Key)))
                    .Returns(a.Value);

            // Abiltities
            if (mockAbs)
            {
                var Abs = Hero switch
                {
                    TestHeroes.Layariel => HeroWipfelglanz.Abilities,
                    TestHeroes.Arbosch => HeroArbosch.Abilities,
                    TestHeroes.Grassberger => HeroGrassberger.Abilities,
                    _ => throw new NotImplementedException("Hero does not exist")
                };
                mockCharacterM.SetupGet(p => p.Abilities).Returns(Abs);
            }

            // Combat techs
            if (mockCT)
            {
                mockCharacterM.SetupGet(p => p.CombatTechs).
                    Returns(Hero switch
                    {
                        TestHeroes.Layariel => HeroWipfelglanz.CombatTechs(mockCharacterM.Object),
                        TestHeroes.Arbosch => HeroArbosch.CombatTechs(mockCharacterM.Object),
                        TestHeroes.Grassberger => HeroGrassberger.CombatTechs(mockCharacterM.Object),
                        _ => throw new NotImplementedException("Hero does not exist")
                    });
            }

            // Activatables
            if (isAmbi is not null)
                mockCharacterM.Setup(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)))
                    .Returns(isAmbi ?? false);

            if (TwoHandedTier is not null)
            {
                mockCharacterM.Setup(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)))
                    .Returns(TwoHandedTier > 0);
                // Add mocked call like this: Hero.SpecialAbilities[SA.TwoHandedCombat].Tier
                Dictionary<string, IActivatableM> SpecAbs = new();
                SpecAbs.Add(SA.TwoHandedCombat, new TieredActivatableM(SA.TwoHandedCombat, TwoHandedTier ?? 0, null));
                mockCharacterM.SetupGet(p => p.SpecialAbilities).Returns(SpecAbs);
            }

        }

        #endregion



        //
        //
        // TESTS

        [Test, Description("Does the primary ability increase *dagger* hit points?")]
        [TestCase(TestHeroes.Layariel, 1, Description = "Layariels primary ability AGI increase *dagger* hit points by 1")]
        [TestCase(TestHeroes.Arbosch, 0, Description = "Arbosch has only 11; that is not enough for a bonus")]
        public void Damage_SingleWeapon_MainHand_DamageIncreasedBy1(TestHeroes testHero, int Mod)
        {
            // Arrange
            MockHero(testHero, true, true);

            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.LayarielsDagger,
                TestHeroes.Arbosch => HeroArbosch.ArboschsDagger,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.DamageBonus;

            // Assert
            Assert.AreEqual(WeaponData.DamageBonus + Mod, result);
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
        [TestCase(TestHeroes.Layariel, 9, CombatBranch.Unarmed)]
        [TestCase(TestHeroes.Layariel, 9, CombatBranch.Shield)]
        [TestCase(TestHeroes.Arbosch, 8, CombatBranch.Unarmed)]
        [TestCase(TestHeroes.Arbosch, 8, CombatBranch.Shield)]
        public void AtSkill_CtDagger_EmptyOffhand_GivesUnmodifiedSkillValue(TestHeroes testHero, int AtVal, CombatBranch otherHand)
        {
            // Arrange
            MockHero(testHero, true, true, false, 0);

            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.LayarielsDagger,
                TestHeroes.Arbosch => HeroArbosch.ArboschsDagger,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = true;

            // Act
            var result = weaponM.AtSkill(MainHand, otherHand);

            // Assert
            Assert.AreEqual(AtVal, result);

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



        // The wooden shield has a attack penalty of -4.
        // Additional -2 for two-handed combat without special ability
        // ONly parrying does not suffer from the off-hand penalty. So here additional -4 must be added.
        [Test]
        [TestCase(TestHeroes.Grassberger, 14-4-4, CombatBranch.Unarmed)]
        [TestCase(TestHeroes.Grassberger, 14-4-4-2, CombatBranch.Melee)]
        public void AtSkill_CtShields_SwordInMainHand_(TestHeroes testHero, int AtVal, CombatBranch otherHand)
        {
            // Arrange
            MockHero(testHero, true, true, false, 0);

            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Grassberger => HeroGrassberger.WoodenShield,
                _ => throw new NotImplementedException("Hero is not implemented for this test")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = false;

            // Act
            var result = weaponM.AtSkill(MainHand, otherHand);

            // Assert
            Assert.AreEqual(AtVal, result);

            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // STR to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_8")), Times.AtLeastOnce);

            mockCharacterM.Verify(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)), Times.Once);
            mockCharacterM.Verify(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)), Times.Once);
        }



        [Test]
        [TestCase(TestHeroes.Layariel, 6)]
        [TestCase(TestHeroes.Arbosch, 4)]
        public void PaSkill_CtDagger_EmptyOffhand_GivesSkillValue(TestHeroes testHero, int PaVal)
        {
            // Arrange
            MockHero(testHero, true, true, false, 0);

            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.LayarielsDagger,
                TestHeroes.Arbosch => HeroArbosch.ArboschsDagger,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = true;
            CombatBranch otherHand = CombatBranch.Unarmed;
            bool otherIsParry = false;
            int otherPaSkill = 0;

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, otherPaSkill);

            // Assert
            Assert.AreEqual(PaVal, result);
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


        // Parry bonus of the wooden shield is added twice (VR1.de, p. 233)
        // No parry penalty for using the off-hand
        // No penalty for fighting two-handed
        [Test, Description("")]
        [TestCase(TestHeroes.Grassberger, 8+2*1, CombatBranch.Unarmed)]
        [TestCase(TestHeroes.Grassberger, 8+2*1, CombatBranch.Melee)]
        public void PaSkill_CtShield_OffhandShield_AddsPassiveShieldToParry(TestHeroes testHero, int PaVal, CombatBranch otherHand)
        {
            const bool MainHand = false;
            const bool otherIsParry = false;
            const int otherPaSkill = 0;

            // Arrange
            MockHero(testHero, true, true, false, 0);


            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Grassberger => HeroGrassberger.WoodenShield,
                _ => throw new NotImplementedException("Hero not used in this test")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, otherPaSkill);


            // Assert
            Assert.AreEqual(PaVal, result);
            //
            mockCharacterM.VerifyGet(p => p.Abilities, Times.AtLeastOnce);
            mockCharacterM.VerifyGet(p => p.CombatTechs, Times.AtLeastOnce);
            // COU to determine the attack value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_1")), Times.AtLeastOnce);
            // AGI to determine the parry value
            mockCharacterM.Verify(m => m.GetAbility(It.Is<string>(s => s == "ATTR_8")), Times.AtLeastOnce);

            mockCharacterM.Verify(m => m.HasAdvantage(It.Is<string>(s => s == ADV.Ambidexterous)), Times.Once);
            mockCharacterM.Verify(m => m.HasSpecialAbility(It.Is<string>(s => s == SA.TwoHandedCombat)), Times.Once);
        }



        // Note: for a shield the two-handed penalty is not effective
        [Test, Description("A shield in the other hand adds the passive weapon bonus to the parry value")]
        [TestCase(TestHeroes.Layariel, 6, 1)]
        [TestCase(TestHeroes.Layariel, 6, 2)]
        [TestCase(TestHeroes.Layariel, 6, 4)]
        [TestCase(TestHeroes.Arbosch, 4, 1)]
        [TestCase(TestHeroes.Arbosch, 4, 5)]
        public void PaSkill_CtDagger_OffhandShield_AddsPassiveShieldToParry(TestHeroes testHero, int PaVal, int shieldPaSkill)
        {
            const bool MainHand = true;
            const bool otherIsParry = false;
            const CombatBranch otherHand = CombatBranch.Shield;

            // Arrange
            MockHero(testHero, true, true, false, 0);


            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.LayarielsDagger,
                TestHeroes.Arbosch => HeroArbosch.ArboschsDagger,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, shieldPaSkill);


            // Assert
            Assert.AreEqual(PaVal + shieldPaSkill, result);
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
        [TestCase(TestHeroes.Layariel, 6, 0, -2, Description = "No special ability 'Two-Handed Combat'")]
        [TestCase(TestHeroes.Layariel, 6, 1, -1)]
        [TestCase(TestHeroes.Layariel, 6, 2, 0)]
        [TestCase(TestHeroes.Arbosch, 4, 0, -2, Description = "No special ability 'Two-Handed Combat'")]
        [TestCase(TestHeroes.Arbosch, 4, 1, -1)]
        [TestCase(TestHeroes.Arbosch, 4, 2, 0)]
        public void PaSkill_CtDagger_OffhandParryWeapon_AddsPassiveBonusToParryAnd2HandedPenalty(
            TestHeroes testHero, int PaVal, int SATwoHandedTier, int Penalty4TwohandedCombat)
        {
            const bool MainHand = true;
            const bool otherIsParry = true;
            const CombatBranch otherHand = CombatBranch.Melee;
            const int ParryWeaponBonus = 1;

            // Arrange
            MockHero(testHero, true, true, false, SATwoHandedTier);


            WeaponDTO WeaponData = testHero switch
            {
                TestHeroes.Layariel => HeroWipfelglanz.LayarielsDagger,
                TestHeroes.Arbosch => HeroArbosch.ArboschsDagger,
                _ => throw new NotImplementedException("Hero does not exist")
            };
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, ParryWeaponBonus);


            // Assert
            Assert.AreEqual(PaVal + ParryWeaponBonus + Penalty4TwohandedCombat, result);
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


        // Implicitely tested by calling `AtSkill(...)`
        //public void ComputeAttackVal_StateUnderTest_ExpectedBehavior()


        // Implicitely tested by calling `PaSkill(...)`
        //public void ComputeParryVal_StateUnderTest_ExpectedBehavior()


        [Test]
        [TestCaseSource(nameof(LayarielsWeaponsHitPointBonus))]
        public void HitpointBonus_VaryingPrimaryAbility(WeaponDTO Weapon, int Expected)
        {
            // Arrange
            MockHero(TestHeroes.Layariel, true, true);

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
            MockHero(TestHeroes.Layariel, false, false);
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
