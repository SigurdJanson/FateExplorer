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

        private CombatTechM MakeCombatTechM(CombatTechDbEntry ct, int Skill, ICharacterM hero)
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

        private static IEnumerable<TestCaseData> LayarielsDagger
        {
            get
            {
                WeaponDTO w = new()
                {
                    Id = "ITEM_9999",
                    Name = "Layariels Dagger",
                    CombatTechId = "CT_3",
                    Improvised = false,
                    AttackMod = 0,
                    ParryMod = 0,
                    DamageThreshold = 14,
                    DamageDieCount = 1,
                    DamageDieSides = 6,
                    DamageBonus = 1
                };
                yield return new TestCaseData(w);
            }
        }

        /// <summary>
        /// The abilities of Layariel Wipfelglanz
        /// </summary>
        private static Dictionary<string, int> AbilityValues
        {
            get 
            {
                Dictionary<string, int> Result = new();
                Result.Add(AbilityM.COU, 11);
                Result.Add(AbilityM.SGC, 10);
                Result.Add(AbilityM.INT, 15);
                Result.Add(AbilityM.CHA, 13);
                Result.Add(AbilityM.DEX, 14);
                Result.Add(AbilityM.AGI, 15);
                Result.Add(AbilityM.CON, 11);
                Result.Add(AbilityM.STR, 11);
                return Result;
            }
        }
    }



    [TestFixture]
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
            var Abs = AbilityValues;
            foreach(var a in Abs)
                mockCharacterM.Setup(x => x.GetAbility(It.Is<string>(s => s == a.Key)))
                    .Returns(a.Value);

            this.mockGameDataM = this.mockRepository.Create<IGameDataService>();
        }


        private WeaponM CreateWeaponM()
        {
            return new WeaponM(this.mockCharacterM.Object);
        }






        [Test, Ignore("nyi")]
        public void AtSkill_DamageThreshold_IncreasedDAmageBy1(
            [ValueSourceAttribute(nameof(LayarielsDagger))] WeaponDTO WeaponData)
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            weaponM.Initialise(WeaponData, mockGameDataM.Object);

            bool MainHand = false;
            CombatBranch otherHand = default;

            // Act
            var result = weaponM.AtSkill(MainHand, otherHand);

            // Assert
            Assert.Fail();
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
            var result = weaponM.PaSkill(MainHand, otherHand, otherPaSkill, otherIsParry);

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
            Dictionary<string,CombatTechM> CombatTecSkill = null;

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
            Dictionary<string, AbilityM> Abilities = null;
            Dictionary<string, CombatTechM> CombatTecSkill = null;

            // Act
            var result = weaponM.ComputeParryVal(Abilities, CombatTecSkill);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }


        [Test, Ignore("nyi")]
        [TestCase()]
        public void HitpointBonus_VaryingPrimaryAbility(int Threshold, string PrimaryAbility)
        {
            // Arrange
            var weaponM = this.CreateWeaponM();
            weaponM.DamageThreshold = Threshold;
            //weaponM.PrimaryAbilityId = PrimaryAbility;
            Dictionary<string, AbilityM> Abilities = null;

            // Act
            var result = weaponM.HitpointBonus(Abilities);

            // Assert
            Assert.Fail();
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
