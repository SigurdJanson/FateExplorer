using FateExplorer.CharacterModel;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;

namespace FateExplorer.RollLogic;



public class BattlegroundM : ICheckContextM
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="useMainHand">true = fight with main hand weapon; false = fight with off-hand weapon.</param>
    /// <param name="mainWeapon">Weapon in the main hand.</param>
    /// <param name="offWeapon">Weapon in the off hand.</param>
    public BattlegroundM(bool useMainHand, WeaponM mainWeapon, WeaponM offWeapon)
    {
        MainWeapon = mainWeapon;
        OffWeapon = offWeapon;
        UseMainHand = useMainHand;
    }


    /*
     * Implement `IStateContainer`
     */
    public event Action OnStateChanged;

    private void NotifyStateChange() => OnStateChanged?.Invoke();


    /*
     * Pass through properties to access the weapons properties needed to determine
     * the effects of the context.
     */
    protected CombatBranch Branch { get => MainWeapon.Branch; }
    protected string WeaponName { get => MainWeapon.Name; }
    protected WeaponsReach WeaponReach { get => (WeaponsReach)MainWeapon.Reach; }
    protected bool IsRanged { get => MainWeapon.IsRanged; }



    /// <summary>
    /// Clear the battleground and remove all modifiers.
    /// </summary>
    public void ResetToDefault()
    {
        // 
        distance = DistanceDefault;
        visibility = ViewDefault;
        water = WaterDefault;
        crampedSpace = CrampedSpaceDefault;
        moving = MovingDefault;
        enemyMoving = EnemyMovingDefault;
        enemyReach = EnemyReachDefault;
        sizeOfEnemy = SizeOfEnemyDefault;
        enemyEvasive = EnemyEvasiveDefault;
        //
        freeModifier = FreeModifierDefault;
        //
        NotifyStateChange();
    }




    /// <summary>
    /// Helper method to set property values of this class. Using this method elicits a state change action
    /// if old and new value are different from each other.
    /// </summary>
    /// <typeparam name="T">Type of the property to set.</typeparam>
    /// <param name="original">The property to be set.</param>
    /// <param name="value">A new value.</param>
    private void SetVal<T>(ref T original, T value)
    {
        if (!original.Equals(value))
        {
            TotalMod = Modifier.Neutral;
            TotalModAction = 0;
            original = value;
            NotifyStateChange();
        }
    }

    /*
     * Battleground properties
     */
    /// <summary>
    /// Weapon in the main ("strong") hand
    /// </summary>
    public WeaponM MainWeapon { get; set; }
    /// <summary>
    /// Weapon in the off-hand
    /// </summary>
    public WeaponM OffWeapon { get; set; }
    /// <summary>
    /// true = fight with main hand weapon; false = fight with off-hand weapon.
    /// </summary>
    public bool UseMainHand { get; set; }

    private const int FreeModifierDefault = 0;
    private const WeaponsRange DistanceDefault = WeaponsRange.Medium;
    private const Vision ViewDefault = Vision.Clear;
    private const UnderWater WaterDefault = UnderWater.Dry;
    private const bool CrampedSpaceDefault = false;
    private const Movement MovingDefault = Movement.None;
    private const Movement EnemyMovingDefault = Movement.None;
    private const bool EnemyEvasiveDefault = false;
    private const WeaponsReach EnemyReachDefault = WeaponsReach.Medium;
    private const EnemySize SizeOfEnemyDefault = EnemySize.Medium;


    private int freeModifier = FreeModifierDefault;
    private WeaponsRange distance = DistanceDefault;
    private Vision visibility = ViewDefault;
    private UnderWater water = WaterDefault;
    private bool crampedSpace = CrampedSpaceDefault;
    private Movement moving = MovingDefault;
    private Movement enemyMoving = EnemyMovingDefault;
    private bool enemyEvasive = EnemyEvasiveDefault;
    private WeaponsReach enemyReach = EnemyReachDefault;
    private EnemySize sizeOfEnemy = SizeOfEnemyDefault;


    /// <summary>
    /// An additional additive modifier to be used in addition to the context itself.
    /// </summary>
    public int FreeModifier { get => freeModifier; set => SetVal(ref freeModifier, value); }
    public WeaponsRange Distance { get => distance; set => SetVal(ref distance, value); }
    public Vision Visibility { get => visibility; set => SetVal(ref visibility, value); }
    public UnderWater Water { get => water; set => SetVal(ref water, value); }
    public bool CrampedSpace { get => crampedSpace; set => SetVal<bool>(ref crampedSpace, value); }
    public Movement Moving { get => moving; set => SetVal(ref moving, value); }
    public Movement EnemyMoving { get => enemyMoving; set => SetVal(ref enemyMoving, value); }
    public bool EnemyEvasive { get => enemyEvasive; set => SetVal(ref enemyEvasive, value); }
    public WeaponsReach EnemyReach { get => enemyReach; set => SetVal(ref enemyReach, value); }
    public EnemySize SizeOfEnemy { get => sizeOfEnemy; set => SetVal(ref sizeOfEnemy, value); }



    public Modifier GetDistanceMod()
    {
        if (!IsRanged) return Modifier.Neutral;

        return Distance switch
        {
            WeaponsRange.Short => new Modifier(+2),
            WeaponsRange.Medium => Modifier.Neutral,
            WeaponsRange.Long => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetVisibilityMod(Check.Combat action)
    {
        // dodging would be same as parry

        if (IsRanged)
            return Visibility switch
            {
                Vision.Clear => Modifier.Neutral,
                Vision.Impaired => new Modifier(-2),
                Vision.ShapesOnly => new Modifier(-4),
                Vision.Barely => new Modifier(-6),
                Vision.NoVision => Modifier.LuckyShot,
                _ => throw new InvalidOperationException()
            };
        else
            return Visibility switch
            {
                Vision.Clear => Modifier.Neutral,
                Vision.Impaired => new Modifier(-1),
                Vision.ShapesOnly => new Modifier(-2),
                Vision.Barely => new Modifier(-3),
                Vision.NoVision =>
                    !IsRanged && action == Check.Combat.Attack ? Modifier.Halve : Modifier.LuckyShot,
                _ => throw new InvalidOperationException()
            };

    }


    public Modifier GetWaterMod() /** OK **/
    {
        // dodging would be affected as well

        if (IsRanged)
            return Water switch
            {
                UnderWater.Dry => Modifier.Neutral,
                UnderWater.KneeDeep => Modifier.Neutral,
                UnderWater.WaistDeep => new Modifier(-2),
                _ => Modifier.Impossible
            };
        else
            return Water switch
            {
                UnderWater.Dry => Modifier.Neutral,
                UnderWater.KneeDeep => new Modifier(-1),
                UnderWater.WaistDeep => new Modifier(-2),
                UnderWater.ChestDeep => new Modifier(-4),
                UnderWater.NeckDeep => new Modifier(-5),
                UnderWater.Submerged => new Modifier(-6),
                _ => throw new InvalidOperationException()
            };
    }


    public Modifier GetCrampedSpaceMod()
    {
        if (!CrampedSpace) return Modifier.Neutral;
        // dodging would not be affected

        if (Branch == CombatBranch.Ranged || Branch == CombatBranch.Unarmed)
            return Modifier.Neutral;
        else if (Branch == CombatBranch.Melee)
            return WeaponReach switch
            {
                WeaponsReach.Short => Modifier.Neutral,
                WeaponsReach.Medium => new Modifier(-4),
                WeaponsReach.Long => new Modifier(-8),
                _ => throw new InvalidOperationException(),
            };
        else // CombatBranch.Shield
            return new Modifier(-2); // TODO: distinguish shield size
    }


    public Modifier GetMovingMod(Check.Combat action)
    {
        if (!IsRanged) return Modifier.Neutral;
        if (action == Check.Combat.Parry) return Modifier.Neutral;
        // dodging would not be affected

        return Moving switch
        {
            Movement.None => Modifier.Neutral,
            Movement.Slow => new Modifier(-2), // slow on foot
            Movement.Fast => new Modifier(-4),
            Movement.GaitWalk => new Modifier(-4),
            Movement.GaitTrot => Modifier.LuckyShot,
            Movement.GaitGallop => new Modifier(-8),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetEnemyMovingMod(Check.Combat action)
    {
        if (!IsRanged) return Modifier.Neutral;
        if (action == Check.Combat.Parry) return Modifier.Neutral;
        // dodging would not be affected

        return EnemyMoving switch
        {
            Movement.None => new Modifier(2),
            Movement.Slow => Modifier.Neutral, // slow on foot
            Movement.Fast => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetEvasiveActionMod(Check.Combat action)
    {
        if (!IsRanged) return Modifier.Neutral;
        if (action == Check.Combat.Parry) return Modifier.Neutral;
        // dodging would not be affected

        return EnemyEvasive switch
        {
            true => new Modifier(-4),
            false => Modifier.Neutral
        };
    }


    public Modifier GetEnemyReachMod()
    {
        // dodging would not be affected

        if (WeaponReach >= EnemyReach) return Modifier.Neutral;

        return (WeaponReach, EnemyReach) switch
        {
            (WeaponsReach.Short, WeaponsReach.Medium) => new Modifier(-2),
            (WeaponsReach.Short, WeaponsReach.Long) => new Modifier(-4),
            (WeaponsReach.Medium, WeaponsReach.Long) => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetSizeOfEnemyMod(Check.Combat action)
    {
        // dodging would not be affected

        if (IsRanged)
        {
            return SizeOfEnemy switch
            {
                EnemySize.Tiny => new Modifier(-8),
                EnemySize.Small => new Modifier(-4),
                EnemySize.Medium => new Modifier(0),
                EnemySize.Large => new Modifier(4),
                EnemySize.Huge => new Modifier(8),
                _ => throw new InvalidOperationException()
            };
        }
        else
        {
            if (Branch == CombatBranch.Melee || Branch == CombatBranch.Unarmed)
            {
                if (action == Check.Combat.Attack && SizeOfEnemy == EnemySize.Tiny)
                    return new Modifier(-4);
                else if (action == Check.Combat.Parry && SizeOfEnemy > EnemySize.Large)
                    return Modifier.Impossible;
                else
                    return Modifier.Neutral;
            }
            if (Branch == CombatBranch.Shield)
            {
                if (action == Check.Combat.Attack && SizeOfEnemy == EnemySize.Tiny)
                    return new Modifier(-4);
                else if (action == Check.Combat.Parry && SizeOfEnemy == EnemySize.Huge)
                    return Modifier.Impossible;
                else
                    return Modifier.Neutral;
            }
        }
        throw new InvalidOperationException();
    }


    /*
     * ICheckContextM
     */
    private Modifier TotalMod { get; set; } = Modifier.Neutral;
    private Check.Combat TotalModAction { get; set; } = 0;

    /// <inheritdoc />
    public Modifier GetTotalMod(int before, Check.Combat action)
    {
        if (!TotalMod.IsNeutral && action == TotalModAction) return TotalMod;

        List<Modifier> Mods = new()
        {
            //
            GetDistanceMod(), GetCrampedSpaceMod(),
            GetEnemyMovingMod(action), GetEvasiveActionMod(action),
            GetEnemyReachMod(),
            GetVisibilityMod(action),
            GetMovingMod(action),
            GetWaterMod(),
            GetSizeOfEnemyMod(action)
        };
        List<Modifier> HalveMods = new(), ForceMods = new();
        int i = 0;
        while (i < Mods.Count)
        {
            if (Mods[i].Operator == Modifier.Op.Halve)
            {
                HalveMods.Add(Mods[i]);
                Mods.RemoveAt(i);
            }
            else if (Mods[i].Operator == Modifier.Op.Force)
            {
                ForceMods.Add(Mods[i]);
                Mods.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        int After = before;
        foreach (var m in Mods)
            After += m;
        After += FreeModifier;

        foreach (var m in HalveMods)
            After += m;
        foreach (var m in ForceMods)
        {
            After += m;
        }

        if (ForceMods.Count > 0)
            TotalMod = new Modifier(After, Modifier.Op.Force);
        else
            TotalMod = new Modifier(After - before, Modifier.Op.Add);
        TotalModAction = action;

        return TotalMod;
    }


    /// <inheritdoc />
    public int ApplyTotalMod(int before, Check.Combat action)
    {
        var mod = GetTotalMod(before, action);
        return before + mod;
    }


    /// <inheritdoc />
    public int ModDelta(int before, Check.Combat action)
    {
        var mod = GetTotalMod(before, action);
        return (before + mod) - before;
    }
}
