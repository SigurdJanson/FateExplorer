using FateExplorer.CharacterModel;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;

namespace FateExplorer.RollLogic.Tests;


[TestFixture()]
public class BattlegroundMTests
{
    private MockRepository mockRepository;
    private Mock<ICharacterM> hero;
    private Mock<IWeaponM> mainWeapon;
    private Mock<IWeaponM> offWeapon;

    BattlegroundM bg;

    /*
     * Weapon Tech Ids
     */
    const string BluntWeapon = "CT_5";
    const string Sword = "CT_12";
    const string Crossbow = "CT_1";
    const string Blowpipe = "CT_18";
    const string ShieldTech = "CT_10";


    [SetUp] 
    public void SetUp() 
    {
        this.mockRepository = new(MockBehavior.Strict);

        hero = mockRepository.Create<ICharacterM>();
        mainWeapon = mockRepository.Create<IWeaponM>(); // hero.Object
        offWeapon = mockRepository.Create<IWeaponM>(); // hero.Object
        bg = new BattlegroundM();
    }


    #region Distance Modifiers =======================

    [Test, Description("Close combat, Attacking only")]
    [TestCase(CombatBranch.Unarmed, "CT_9")]
    [TestCase(CombatBranch.Melee, "CT_3")]
    [TestCase(CombatBranch.Melee, "CT_13")]
    [TestCase(CombatBranch.Shield, "CT_10")]
    public void GetDistanceModTest_CloseCombat_NeutralMod(CombatBranch branch, string TechId)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);

        // Act
        Modifier result = bg.GetDistanceMod(new Check(Check.Combat.Attack, TechId, branch), mainWeapon.Object);

        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }



    [Test, Description("Ranged combat, Attacking only")]
    public void GetDistanceModTest_RangedCombat_NeutralMod(
        [Values(CombatBranch.Ranged)]CombatBranch Branch, 
        [Values("CT_1", "CT_2", "CT_14")] string TechId, 
        [Values] WeaponsRange Distance)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.Distance = Distance;

        // Act
        Modifier result = bg.GetDistanceMod(new Check(Check.Combat.Attack, TechId, Branch), mainWeapon.Object);

        // Assert
        switch (Distance)
        {
            case WeaponsRange.Short: Assert.That(result, Is.EqualTo(new Modifier(2))); break;
            case WeaponsRange.Medium: Assert.That(result, Is.EqualTo(new Modifier(0))); break;
            case WeaponsRange.Long: Assert.That(result, Is.EqualTo(new Modifier(-2))); break;
        }
        mockRepository.VerifyAll();
    }



    [Test()]
    public void GetDistanceModTest_Dodge_ReturnsNeutralMod()
    {
        // Arrange
        bg.Distance = WeaponsRange.Long;
        // Act
        Modifier result = bg.GetDistanceMod(new Check(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    [Test()]
    public void GetDistanceModTest_Initiative_ReturnsNeutralMod()
    {
        // Arrange
        bg.Distance = WeaponsRange.Long;
        // Act
        Modifier result = bg.GetDistanceMod(new Check(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Visibility Modifier =============================

    [Test()]
    [TestCase(BluntWeapon, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(BluntWeapon, Vision.ShapesOnly, Modifier.Op.Add, -2)]
    [TestCase(BluntWeapon, Vision.NoVision, Modifier.Op.Halve, 2)]
    [TestCase(Sword, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(Sword, Vision.ShapesOnly, Modifier.Op.Add, -2)]
    [TestCase(Sword, Vision.NoVision, Modifier.Op.Halve, 2)]
    public void GetVisibilityMod_AttackClose_ReturnsCorrectMod(string TechId, Vision Visibility, Modifier.Op ExpectedOp, int ExpectedValue)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        bg.Visibility = Visibility;

        // Act
        Modifier result = bg.GetVisibilityMod(new Check(Check.Combat.Attack, TechId), mainWeapon.Object);

        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedValue, ExpectedOp)));
        mockRepository.VerifyAll();
    }


    [Test()]
    [TestCase(BluntWeapon, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(BluntWeapon, Vision.Barely, Modifier.Op.Add, -3)]
    [TestCase(BluntWeapon, Vision.NoVision, Modifier.Op.Force, 1)]
    [TestCase(Sword, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(Sword, Vision.ShapesOnly, Modifier.Op.Add, -2)]
    [TestCase(Sword, Vision.NoVision, Modifier.Op.Force, 1)]
    public void GetVisibilityMod_ParryClose_ReturnsCorrectMod(string TechId, Vision Visibility, Modifier.Op ExpectedOp, int ExpectedValue)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.IsRanged).Returns(false); // not called when action is a parry
        bg.Visibility = Visibility;

        // Act
        Modifier result = bg.GetVisibilityMod(new Check(Check.Combat.Parry, TechId), mainWeapon.Object);

        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedValue, ExpectedOp)));
        mockRepository.VerifyAll();
    }


    [Test()]
    [TestCase(Blowpipe, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(Blowpipe, Vision.ShapesOnly, Modifier.Op.Add, -4)]
    [TestCase(Blowpipe, Vision.NoVision, Modifier.Op.Force, 1)]
    [TestCase(Crossbow, Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(Crossbow, Vision.Barely, Modifier.Op.Add, -6)]
    [TestCase(Crossbow, Vision.NoVision, Modifier.Op.Force, 1)]
    public void GetVisibilityMod_AttackRanged_ReturnsCorrectMod(string TechId, Vision Visibility, Modifier.Op ExpectedOp, int ExpectedValue)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.Visibility = Visibility;

        // Act
        Modifier result = bg.GetVisibilityMod(new Check(Check.Combat.Attack, TechId), mainWeapon.Object);

        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedValue, ExpectedOp)));
        mockRepository.VerifyAll();
    }

    [Test()]
    [TestCase(Vision.Clear, Modifier.Op.Add, 0)]
    [TestCase(Vision.ShapesOnly, Modifier.Op.Add, -2)]
    [TestCase(Vision.NoVision, Modifier.Op.Force, 1)]
    public void GetVisibilityModTest_Dodge_ReturnsCorrectMod(Vision Visibility, Modifier.Op ExpectedOp, int ExpectedValue)
    {
        // Arrange
        bg.Visibility = Visibility;
        // Act
        Modifier result = bg.GetVisibilityMod(new Check(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedValue, ExpectedOp)));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetVisibilityModTest_Ini_ReturnsNeutralMod([Values]Vision Visibility)
    {
        // Arrange
        bg.Visibility = Visibility;
        // Act
        Modifier result = bg.GetVisibilityMod(new Check(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Water Modificator ================================

    [Test(), Combinatorial()]
    public void GetWaterMod_CloseRangeAttack_ReturnCorrectMod([Values] UnderWater Depth, [Values] Check.Combat Action)
    {
        int[] Expected = new int[] { 0, -1, -2, -4, -5, -6 };
        int ExpectedMod = Expected[(int)Depth];
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        bg.Water = Depth;
        // Act
        Modifier result = bg.GetWaterMod(new Check(Action, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedMod)));
        mockRepository.VerifyAll();
    }

    [Test(), Combinatorial()]
    public void GetWaterMod_Dodge_ReturnCorrectMod([Values] UnderWater Depth, [Values] bool Ranged)
    {
        int[] Expected = new int[] { 0, -1, -2, -4, -5, -6 };
        int ExpectedMod = Expected[(int)Depth];
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(Ranged);
        bg.Water = Depth;
        // Act
        Modifier result = bg.GetWaterMod(new Check(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedMod)));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetWaterMod_RangedAttack_LowWater_ReturnCorrectMod(
        [Values(UnderWater.Dry, UnderWater.KneeDeep, UnderWater.WaistDeep)] UnderWater Depth)
    {
        int[] Expected = new int[] { 0, 0, -2 };
        int ExpectedMod = Expected[(int)Depth];
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.Water = Depth;
        // Act
        Modifier result = bg.GetWaterMod(new Check(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedMod)));
        mockRepository.VerifyAll();
    }

    [Test(), Combinatorial()]
    public void GetWaterMod_RangedAttack_HighWater_ReturnCorrectMod(
    [Values(UnderWater.ChestDeep, UnderWater.NeckDeep, UnderWater.Submerged)] UnderWater Depth)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.Water = Depth;
        // Act
        Modifier result = bg.GetWaterMod(new Check(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Impossible));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Cramped Space Modifier =================================

    [Test()]
    public void GetCrampedSpace_NotCramped_ReturnNeutralMod([Values] CombatBranch Branch, [Values] Check.Combat Action)
    {
        // Arrange
        bg.CrampedSpace = false;
        Check TestCheck = new(Action, "CT not relevant");

        // Act
        Modifier result = bg.GetCrampedSpaceMod(TestCheck, mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetCrampedSpace_Unarmed_Cramped_ReturnNeutralMod([Values] Check.Combat Action)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.Branch).Returns(CombatBranch.Unarmed);
        bg.CrampedSpace = true;
        Check TestCheck = new(Action, "CT not relevant");

        // Act
        Modifier result = bg.GetCrampedSpaceMod(TestCheck, mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetCrampedSpace_Melee_Cramped_ReturnCorrectMod([Values] Check.Combat Action, [Values] WeaponsReach Reach)
    {
        int[] Expected = new int[] { -999, 0, -4, -8 };
        int ExpectedModValue = Expected[(int)Reach];
        // Arrange
        mainWeapon.SetupGet(w => w.Branch).Returns(CombatBranch.Melee);
        mainWeapon.SetupGet(w => w.Reach).Returns(Reach);
        bg.CrampedSpace = true;

        // Act
        Modifier result = bg.GetCrampedSpaceMod(new(Action, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedModValue)));
        mockRepository.VerifyAll();
    }


    [Test()]
    public void GetCrampedSpace_ShieldAttack_Cramped_ReturnNeutralMod([Values] ShieldSize shieldSize)
    {
        int[] Expected = new int[] { -999, -2, -4, -6 };
        int ExpectedModValue = Expected[(int)shieldSize];
        // Arrange
        mainWeapon.SetupGet(w => w.Branch).Returns(CombatBranch.Shield);
        mainWeapon.SetupGet(w => w.Shield).Returns(shieldSize);
        bg.CrampedSpace = true;

        // Act
        Modifier result = bg.GetCrampedSpaceMod(new(Check.Combat.Attack, ShieldTech), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedModValue)));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetCrampedSpace_ShieldParry_Cramped_ReturnNeutralMod([Values] ShieldSize shieldSize)
    {
        int[] Expected = new int[] { -999, -2, -3, -4 };
        int ExpectedModValue = Expected[(int)shieldSize];
        // Arrange
        mainWeapon.SetupGet(w => w.Branch).Returns(CombatBranch.Shield);
        mainWeapon.SetupGet(w => w.Shield).Returns(shieldSize);
        bg.CrampedSpace = true;

        // Act
        Modifier result = bg.GetCrampedSpaceMod(new(Check.Combat.Parry, ShieldTech), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(ExpectedModValue)));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Moving Hero Modifier =================================

    [Test()]
    public void GetMovingMod_RangedAttack_ReturnCorrectMod([Values] Movement Move)
    {
        int[] Expected = new[] { 0, -2, -4, -4, 1, -8 };
        Modifier.Op[] ExpectedOp = new Modifier.Op[] { Modifier.Op.Add, Modifier.Op.Add, Modifier.Op.Add, Modifier.Op.Add, Modifier.Op.Force, Modifier.Op.Add };
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.Moving = Move;

        // Act
        Modifier result = bg.GetMovingMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(Expected[(int)Move], ExpectedOp[(int)Move])));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetMovingMod_RangedParry_ReturnNeutralMod([Values] Movement Move)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.IsRanged).Returns(true); // checked AFTER checking the Check, hence, not relevant
        bg.Moving = Move;

        // Act
        Modifier result = bg.GetMovingMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }


    [Test()]
    public void GetMovingMod_CloseCombatAttack_ReturnNeutralMod([Values] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        bg.Moving = Move;

        // Act
        Modifier result = bg.GetMovingMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetMovingMod_Dodge_ReturnNeutralMod([Random(3)] Movement Move)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.IsRanged).Returns(true); // checked AFTER checking the Check, hence, not relevant
        bg.Moving = Move;
        // Act
        Modifier result = bg.GetMovingMod(new(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    [Test()]
    public void GetMovingMod_Initiative_ReturnNeutralMod([Random(3)] Movement Move)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.IsRanged).Returns(true); // checked AFTER checking the Check, hence, not relevant
        bg.Moving = Move;
        // Act
        Modifier result = bg.GetMovingMod(new(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Moving Enemy Modifier =================================

    [Test()]
    public void GetEnemyMovingMod_WalkingSpeed_RangedAttack_ReturnCorrectMod(
        [Values(Movement.None, Movement.Slow, Movement.Fast)] Movement Move)
    {
        int[] Expected = new[] { 2, 0, -2 };
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyMoving = Move;

        // Act
        Modifier result = bg.GetEnemyMovingMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(Expected[(int)Move])));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyMovingMod_WalkingSpeed_RangedAttack_ThrowsException(
    [Values(Movement.GaitGallop, Movement.GaitTrot, Movement.GaitWalk)] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyMoving = Move;
        // Act
        // Assert
        Assert.That(
            () => bg.GetEnemyMovingMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object), 
            Throws.Exception.TypeOf<InvalidOperationException>());
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyMovingMod_Parry_ReturnNeutralMod([Values] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyMoving = Move;

        // Act
        Modifier result = bg.GetEnemyMovingMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test(), Description("Movement here is not used; any kind of movement yields a neutral modifier.")]
    public void GetEnemyMovingMod_CloseCombatAttack_ReturnNeutralMod([Random(3)] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        bg.EnemyMoving = Move;

        // Act
        Modifier result = bg.GetEnemyMovingMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyMovingMod_Dodge_ReturnNeutralMod([Random(3)] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyMoving = Move;
        // Act
        Modifier result = bg.GetEnemyMovingMod(new(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyMovingMod_Initiative_ReturnNeutralMod([Random(3)] Movement Move)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyMoving = Move;
        // Act
        Modifier result = bg.GetEnemyMovingMod(new(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Enemy Evasive Action Modifier ==================

    [Test()]
    public void GetEvasiveActionMod_RangedAttack_ReturnCorrectMod([Values] bool Evasive)
    {
        Modifier Expected = Evasive ? new Modifier(-4) : Modifier.Neutral;
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyEvasive = Evasive;
        // Act
        Modifier result = bg.GetEvasiveActionMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Expected));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEvasiveActionMod_Parry_ReturnNeutralMod([Values] bool Evasive)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyEvasive = Evasive;

        // Act
        Modifier result = bg.GetEvasiveActionMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEvasiveActionMod_CloseCombatAttack_ReturnNeutralMod([Values] bool Evasive)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        bg.EnemyEvasive = Evasive;

        // Act
        Modifier result = bg.GetEvasiveActionMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEvasiveActionMod_Dodge_ReturnNeutralMod([Values] bool Evasive)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyEvasive = Evasive;
        // Act
        Modifier result = bg.GetEvasiveActionMod(new(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    [Test()]
    public void GetEvasiveActionMod_Initiative_ReturnNeutralMod([Values] bool Evasive)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.EnemyEvasive = Evasive;
        // Act
        Modifier result = bg.GetEvasiveActionMod(new(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Reach Difference Mod ==========================

    [Test()]
    public void GetEnemyReachMod_CloseAttack_HeroWithLongerReach_ReturnNeutralMod(
        [Values(WeaponsReach.Short, WeaponsReach.Medium)] WeaponsReach Enemy)
    {
        // Arrange
        WeaponsReach Hero = Enemy + 1; 
        Assume.That(Hero, Is.LessThanOrEqualTo(WeaponsReach.Long));

        mainWeapon.SetupGet(w => w.Reach).Returns(Hero);
        bg.EnemyReach = Enemy;
        // Act
        Modifier result = bg.GetEnemyReachMod(new(Check.Combat.Attack, "CT not relevant", CombatBranch.Melee), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyReachMod_CloseAttack_EnemyWithLongerReach_ReturnNegativeMod(
    [Values(WeaponsReach.Long, WeaponsReach.Medium)] WeaponsReach Enemy)
    {
        // Arrange
        WeaponsReach Hero = Enemy - 1;
        Assume.That(Hero, Is.GreaterThanOrEqualTo(WeaponsReach.Short));

        mainWeapon.SetupGet(w => w.Reach).Returns(Hero);
        bg.EnemyReach = Enemy;
        // Act
        Modifier result = bg.GetEnemyReachMod(new(Check.Combat.Attack, "CT not relevant", CombatBranch.Melee), mainWeapon.Object);
        // Assert
        Assert.That((int)result, Is.LessThan((int)Modifier.Neutral));
        Assert.That(result.Operator, Is.EqualTo(Modifier.Op.Add));
        mockRepository.VerifyAll();
    }

    [Test()]
    public void GetEnemyReachMod_Dodge_ReturnNeutralMod([Values] WeaponsReach Hero, [Values] WeaponsReach Enemy)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.Reach).Returns(Hero); // checked later in method
        bg.EnemyReach = Enemy;
        // Act
        Modifier result = bg.GetEnemyReachMod(new(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    [Test()]
    public void GetEnemyReachMod_Initiative_ReturnNeutralMod([Values] WeaponsReach Hero, [Values] WeaponsReach Enemy)
    {
        // Arrange
        //mainWeapon.SetupGet(w => w.Reach).Returns(Hero); // checked later in method
        bg.EnemyReach = Enemy;
        // Act
        Modifier result = bg.GetEnemyReachMod(new(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    #endregion



    #region Enemy Size Mod =========================

    [Test()]
    public void GetEnemySizeMod_RangedAttack_ReturnCorrectMod([Values] EnemySize Size)
    {
        int Expected = ((int)Size - 2) * 4;
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(Expected)));
        mockRepository.VerifyAll();
    }

    [Test()] // Close combat attack against tiny -4
    public void GetEnemySizeMod_CloseAttackAgainstTiny_ReturnMinus4(
        [Values(EnemySize.Tiny)] EnemySize Size, 
        [Values(CombatBranch.Shield, CombatBranch.Melee, CombatBranch.Unarmed)] CombatBranch Branch)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        mainWeapon.SetupGet(w => w.Branch).Returns(Branch);
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Combat.Attack, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(new Modifier(-4)));
        mockRepository.VerifyAll();
    }

    [Test()] // Melee & Unarmed Parry against > large is impossible
    public void GetEnemySizeMod_MeleeANdUnarmedParryAgainstTiny_Impossible(
        [Values(EnemySize.Large, EnemySize.Huge)] EnemySize Size,
        [Values(CombatBranch.Melee, CombatBranch.Unarmed)] CombatBranch Branch)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        mainWeapon.SetupGet(w => w.Branch).Returns(Branch);
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Impossible));
        mockRepository.VerifyAll();
    }

    [Test()] // Shield Parry against > large is possible
    public void GetEnemySizeMod_ShieldParryAgainstTiny_ReturnNeutral(
    [Values(EnemySize.Large)] EnemySize Size,
    [Values(CombatBranch.Shield)] CombatBranch Branch)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        mainWeapon.SetupGet(w => w.Branch).Returns(Branch);
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    [Test()] // Shield Parry against > huge is impossible
    public void GetEnemySizeMod_ShieldParryAgainstTiny_Impossible(
        [Values(EnemySize.Huge)] EnemySize Size,
        [Values(CombatBranch.Shield)] CombatBranch Branch)
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(false);
        mainWeapon.SetupGet(w => w.Branch).Returns(Branch);
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Combat.Parry, "CT not relevant"), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Impossible));
        mockRepository.VerifyAll();
    }


    [Test()]
    public void GetEnemySizeMod_Dodge_ReturnNeutralMod([Values] EnemySize Size)
    {
        // Arrange
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Roll.Dodge), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }
    [Test()]
    public void GetEnemySizeMod_Initiative_ReturnNeutralMod([Values] EnemySize Size)
    {
        // Arrange
        bg.SizeOfEnemy = Size;
        // Act
        Modifier result = bg.GetSizeOfEnemyMod(new(Check.Roll.Initiative), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Neutral));
        mockRepository.VerifyAll();
    }

    #endregion



    #region Total Mod Calculation =====================

    [Test()]
    public void GetTotalMod_ForcedOverAddition_ReturnsForced()
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);

        bg.SizeOfEnemy = EnemySize.Small; // AT - 4
        bg.Water = UnderWater.ChestDeep;  // force to impossible
        bg.ApplyBattleground = true;

        // Act
        Modifier result = bg.GetTotalMod(10, new Check(Check.Combat.Attack, "CT not relevant", CombatBranch.Ranged), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Impossible));
        mockRepository.VerifyAll();
    }


    [Test(), Description("Visibility allows lucky shot but because of cramped space it is still impossible")]
    public void GetTotalMod_ForcedAndSubstract_ReturnsSmaller()
    {
        // Arrange
        mainWeapon.SetupGet(w => w.IsRanged).Returns(true);
        mainWeapon.SetupGet(w => w.Reach).Returns(WeaponsReach.Long);
        mainWeapon.SetupGet(w => w.Branch).Returns(CombatBranch.Melee);

        bg.CrampedSpace = true; // AT - 8 for a long weapon
        bg.Visibility = Vision.NoVision;   // force to lucky shot
        bg.ApplyBattleground = true;

        // Act
        Modifier result = bg.GetTotalMod(8, new Check(Check.Combat.Parry, "CT not relevant", CombatBranch.Melee), mainWeapon.Object);
        // Assert
        Assert.That(result, Is.EqualTo(Modifier.Impossible));
        mockRepository.VerifyAll();
    }

    #endregion
}