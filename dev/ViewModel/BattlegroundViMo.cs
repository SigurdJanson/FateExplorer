using FateExplorer.CharacterModel;
using FateExplorer.RollLogic;
using FateExplorer.Shared;

namespace FateExplorer.ViewModel;


public class BattlegroundViMo : ICheckContextViMo
{
    private BattlegroundM Battleground { get; set; }

    public int FreeModifier { 
        get => Battleground.FreeModifier; set => Battleground.FreeModifier = value; 
    }
    /// <remarks>
    /// Unlike the other arguments this value is not interpreted as-is. It only stores the set distance.
    /// To get a modifier use the <see cref="DistanceBracket"/>.
    /// </remarks>
    public int Distance { get; set; }
    public WeaponsRange DistanceBracket { 
        get => Battleground.Distance; set => Battleground.Distance = value; 
    }
    public Vision View {
        get => Battleground.Visibility; set => Battleground.Visibility = value;
    }
    public UnderWater Water {
        get => Battleground.Water; set => Battleground.Water = value;
    }
    public bool CrampedSpace {
        get => Battleground.CrampedSpace; set => Battleground.CrampedSpace = value;
    }
    public Movement Moving {
        get => Battleground.Moving; set => Battleground.Moving = value;
    }
    public Movement EnemyMoving {
        get => Battleground.EnemyMoving; set => Battleground.EnemyMoving = value;
    }
    public WeaponsReach EnemyReach {
        get => Battleground.EnemyReach; set => Battleground.EnemyReach = value;
    }
    public EnemySize SizeOfEnemy {
        get => Battleground.SizeOfEnemy; set => Battleground.SizeOfEnemy = value;
    }
    public bool EnemyEvasive {
        get => Battleground.EnemyEvasive; set => Battleground.EnemyEvasive = value;
    }


    public static bool IsDistanceEnabled(WeaponViMo weapon) => BattlegroundM.IsDistanceEnabled(weapon.ToWeaponM());
    public static bool IsCrampedSpaceEnabled(WeaponViMo weapon) => BattlegroundM.IsCrampedSpaceEnabled(weapon.ToWeaponM());
    public static bool IsEnemyReachEnabled(WeaponViMo weapon) => BattlegroundM.IsEnemyReachEnabled(weapon.ToWeaponM());
    public static bool IsEnemyEvasiveEnabled(WeaponViMo weapon) => BattlegroundM.IsEnemyEvasiveEnabled(weapon.ToWeaponM());
    public static bool IsMovingEnabled(WeaponViMo weapon) => BattlegroundM.IsMovingEnabled(weapon.ToWeaponM());

    public BattlegroundViMo()
    {
        Battleground = new();
    }

    public void ResetToDefault() => Battleground.ResetToDefault();

    /// <summary>
    /// Is the battleground currently used or ignored? In an ignored battle ground only the 
    /// free modifier from <see cref="ICheckContext"/> will be used.
    /// </summary>
    public bool Apply { get => Battleground.ApplyBattleground; set => Battleground.ApplyBattleground = value; }


    public int Resolve(HandsViMo Hands, bool mainHand, Check action)
    {
        WeaponM weapon = mainHand ? Hands.MainWeapon.ToWeaponM() : Hands.OffWeapon.ToWeaponM();
        WeaponM other = mainHand ? Hands.OffWeapon.ToWeaponM() : Hands.MainWeapon.ToWeaponM();
        if (action.Is(Check.Combat.Attack))
        {
            return Battleground.ApplyTotalMod(weapon.AtSkill(mainHand, other.Branch), action, weapon);
        }
        else if (action.Is(Check.Combat.Parry))
        {
            return Battleground.ApplyTotalMod(weapon.PaSkill(mainHand, other.Branch, other.IsParry, other.ParryMod), action, weapon);
        }
        else
            return 0;
    }


    public BattlegroundM ToBattlegroundM() => Battleground;

    public ICheckContextM ToM() => Battleground;
}
