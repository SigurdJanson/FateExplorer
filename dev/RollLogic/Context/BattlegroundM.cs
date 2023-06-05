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
    public BattlegroundM()
    {}


    /*
     * Implement `IStateContainer`
     */
    public event Action OnStateChanged;

    private void NotifyStateChange() => OnStateChanged?.Invoke();



    /*
     * Modifier calculations
     */

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
            TotalMod = Modifier.Neutral; // reset to allow re-calculation
            TotalModAction = default;
            original = value;
            NotifyStateChange();
        }
    }


    /// <summary>
    /// Gives the user tha option to ignore battleground modifiers
    /// </summary>
    public bool ApplyBattleground { get; set; } = false;


    /*
     * Battleground properties
     */

    private const int FreeModifierDefault = 0;
    private const WeaponsRange DistanceDefault = WeaponsRange.Medium;
    private const Vision ViewDefault = Vision.Clear;
    private const UnderWater WaterDefault = UnderWater.Dry;
    private const bool CrampedSpaceDefault = false;
    private const Movement MovingDefault = Movement.None;
    private const Movement EnemyMovingDefault = Movement.Slow;
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
    /// <summary>
    /// The distance of a target for a character to reach with a ranged Weapon.
    /// </summary>
    public WeaponsRange Distance { get => distance; set => SetVal(ref distance, value); }
    public Vision Visibility { get => visibility; set => SetVal(ref visibility, value); }
    public UnderWater Water { get => water; set => SetVal(ref water, value); }
    public bool CrampedSpace { get => crampedSpace; set => SetVal<bool>(ref crampedSpace, value); }
    public Movement Moving { get => moving; set => SetVal(ref moving, value); }
    public Movement EnemyMoving { get => enemyMoving; set => SetVal(ref enemyMoving, value); }
    public bool EnemyEvasive { get => enemyEvasive; set => SetVal(ref enemyEvasive, value); }
    public WeaponsReach EnemyReach { get => enemyReach; set => SetVal(ref enemyReach, value); }
    public EnemySize SizeOfEnemy { get => sizeOfEnemy; set => SetVal(ref sizeOfEnemy, value); }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public static bool IsDistanceEnabled(IWeaponM weapon) => weapon.IsRanged;
    /// <summary>
    /// Computes the <see cref="Distance"/> modifier between fighter and target.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Modifier GetDistanceMod(Check action, IWeaponM weapon)
    {
        if (!action.IsCombat) return Modifier.Neutral; // DO/INI not affected
        if (!IsDistanceEnabled(weapon)) return Modifier.Neutral;

        return Distance switch
        {
            WeaponsRange.Short => new Modifier(+2),
            WeaponsRange.Medium => Modifier.Neutral,
            WeaponsRange.Long => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetVisibilityMod(Check action, IWeaponM weapon)
    {
        if (!action.IsCombat && !action.Is(Check.Roll.Dodge)) return Modifier.Neutral;

        if (weapon.IsRanged && action.Is(Check.Combat.Attack))
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
                    action.Is(Check.Combat.Attack) ? Modifier.Halve : Modifier.LuckyShot,
                _ => throw new InvalidOperationException()
            };

    }


    public Modifier GetWaterMod(Check action, IWeaponM weapon)
    {
        if (!action.IsCombat) return Modifier.Neutral; // DO/INI not affected

        if (weapon.IsRanged)
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


    public static bool IsCrampedSpaceEnabled(IWeaponM weapon) => !weapon.IsRanged;
    public Modifier GetCrampedSpaceMod(Check action, IWeaponM weapon)
    {
        if (!CrampedSpace) return Modifier.Neutral;
        if (!action.IsCombat) return Modifier.Neutral; // DO/INI/Skills, etc. not affected

        if (weapon.Branch == CombatBranch.Unarmed)
            return Modifier.Neutral;
        else if (weapon.Branch == CombatBranch.Melee)
            return weapon.Reach switch
            {
                WeaponsReach.Short => Modifier.Neutral,
                WeaponsReach.Medium => new Modifier(-4),
                WeaponsReach.Long => new Modifier(-8),
                _ => throw new InvalidOperationException(),
            };
        else // CombatBranch.Shield
            return new Modifier(-2); // TODO: distinguish shield size
    }


    public static bool IsMovingEnabled(IWeaponM weapon) => weapon.IsRanged;
    public Modifier GetMovingMod(Check action, IWeaponM weapon)
    {
        if (!weapon.IsRanged) return Modifier.Neutral;
        if (action.Is(Check.Combat.Parry)) return Modifier.Neutral;
        if (!action.Is(Check.Combat.Attack)) return Modifier.Neutral; // DO/INI not affected

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


    public Modifier GetEnemyMovingMod(Check action, IWeaponM weapon)
    {
        if (!weapon.IsRanged) return Modifier.Neutral;
        if (action.Is(Check.Combat.Parry)) return Modifier.Neutral;
        if (!action.Is(Check.Combat.Attack)) return Modifier.Neutral; // DO/INI not affected

        return EnemyMoving switch
        {
            Movement.None => new Modifier(2),
            Movement.Slow => Modifier.Neutral, // slow on foot
            Movement.Fast => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public static bool IsEnemyEvasiveEnabled(IWeaponM weapon) => weapon.IsRanged;
    public Modifier GetEvasiveActionMod(Check action, IWeaponM weapon)
    {
        if (!weapon.IsRanged) return Modifier.Neutral;
        if (action.Is(Check.Combat.Parry)) return Modifier.Neutral;
        if (!action.Is(Check.Combat.Attack)) return Modifier.Neutral; // DO/INI not affected

        return EnemyEvasive switch
        {
            true => new Modifier(-4),
            false => Modifier.Neutral
        };
    }


    public static bool IsEnemyReachEnabled(IWeaponM weapon) => !weapon.IsRanged;
    public Modifier GetEnemyReachMod(Check action, IWeaponM weapon)
    {
        if (!action.Is(Check.Combat.Attack)) return Modifier.Neutral;
        if (action.Is(Check.Combat.Attack, CombatBranch.Ranged)) return Modifier.Neutral;
        if (weapon.Reach >= EnemyReach) return Modifier.Neutral;

        return (weapon.Reach, EnemyReach) switch
        {
            (WeaponsReach.Short, WeaponsReach.Medium) => new Modifier(-2),
            (WeaponsReach.Short, WeaponsReach.Long) => new Modifier(-4),
            (WeaponsReach.Medium, WeaponsReach.Long) => new Modifier(-2),
            _ => throw new InvalidOperationException()
        };
    }


    public Modifier GetSizeOfEnemyMod(Check action, IWeaponM weapon)
    {
        if (!action.IsCombat) return Modifier.Neutral; // DO/INI not affected

        if (weapon.IsRanged)
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
            if (weapon.Branch == CombatBranch.Melee || weapon.Branch == CombatBranch.Unarmed)
            {
                if (action.Is(Check.Combat.Attack) && SizeOfEnemy == EnemySize.Tiny)
                    return new Modifier(-4);
                else if (action.Is(Check.Combat.Parry) && SizeOfEnemy >= EnemySize.Large)
                    return Modifier.Impossible;
                else
                    return Modifier.Neutral;
            }
            if (weapon.Branch == CombatBranch.Shield)
            {
                if (action.Is(Check.Combat.Attack) && SizeOfEnemy == EnemySize.Tiny)
                    return new Modifier(-4);
                else if (action.Is(Check.Combat.Parry) && SizeOfEnemy >= EnemySize.Huge)
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
    private Check TotalModAction { get; set; } = default;


    /// <inheritdoc />
    public Modifier GetTotalMod(int before, Check action, object asset)
    {
        if (!TotalMod.IsNeutral && action == TotalModAction) return TotalMod;
        WeaponM weapon = asset as WeaponM; //-- ?? throw new ArgumentNullException(nameof(asset));

        int After = before;

        if (ApplyBattleground)
        {
            List<Modifier> Mods = new()
            {
                GetDistanceMod(action, weapon), 
                GetCrampedSpaceMod(action, weapon),
                GetEnemyMovingMod(action, weapon), 
                GetEvasiveActionMod(action, weapon),
                GetEnemyReachMod(action, weapon),
                GetVisibilityMod(action, weapon),
                GetMovingMod(action, weapon),
                GetWaterMod(action, weapon),
                GetSizeOfEnemyMod(action, weapon)
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
                TotalMod = new Modifier(Math.Max(0, After), Modifier.Op.Force);
            else
                TotalMod = new Modifier(After - before, Modifier.Op.Add);
        } 
        else // battground is switched off
        {
            After += FreeModifier;
            TotalMod = new Modifier(After - before, Modifier.Op.Add);
        }

        TotalModAction = action;
        return TotalMod;
    }


    /// <inheritdoc />
    public int ApplyTotalMod(int before, Check action, object asset)
    {
        Modifier mod = GetTotalMod(before, action, asset);
        return before + mod;
    }

    /// <inheritdoc />
    public int[] ApplyTotalMod(int[] before, Check action, object asset = null)
    {
        int[] after = new int[before.Length];
        for (int i = 0; i < before.Length; i++)
            after[i] = before[i] + GetTotalMod(before[i], action, asset);
        return after;
    }

    /// <inheritdoc />
    public int ModDelta(int before, Check action, object asset)
    {
        Modifier mod = GetTotalMod(before, action, asset);
        return (before + mod) - before;
    }
}
