using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace vmCode_UnitTests.CharacterModel
{
    public partial class WeaponMTests
    {
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

    }



    [TestFixture]
    [TestOf(typeof(WeaponM))]
    public partial class WeaponMTests
    {
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






        [Test, Description("Layariels primary ability increase dagger hit points")]
        public void AT_SingleWeapon_MainHand_DamageIncreasedBy1(
            [ValueSource(typeof(HeroWipfelglanz), nameof(HeroWipfelglanz.LayarielsDagger))] WeaponDTO WeaponData)
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            //bool MainHand = true;
            //CombatBranch otherHand = CombatBranch.Unarmed;

            // Act
            //var result = weaponM.AtSkill(MainHand, otherHand);

            // Assert
            Assert.AreEqual(1, weaponM.DamageDieCount);
            Assert.AreEqual(6, weaponM.DamageDieSides);
            Assert.AreEqual(HeroWipfelglanz.LayarielsDagger.DamageBonus+1, weaponM.DamageBonus);
            this.mockRepository.VerifyAll();
        }


        [Test, Ignore("nyi")]
        public void PaSkill_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            bool MainHand = false;
            CombatBranch otherHand = default;
            int otherPaSkill = 0;
            bool otherIsParry = false;

            // Act
            var result = weaponM.PaSkill(MainHand, otherHand, otherIsParry, otherPaSkill);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }


        [Test, Ignore("nyi")]
        public void Initialise_StateUnderTest_ExpectedBehavior(WeaponDTO WeaponData)
        {
            // Arrange
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
            //Dictionary<string, AbilityM> Abilities = null;
            //Dictionary<string, CombatTechM> CombatTecSkill = MakeCombatTechDict(10, mockCharacterM.Object);
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
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(Weapon, mockGameDataM.Object);
            // TODO: mock combat techs

            // Act
            var result = weaponM.HitpointBonus(HeroWipfelglanz.Abilities);

            // Assert
            Assert.AreEqual(Expected, result);
            this.mockRepository.VerifyAll();
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
