using FateExplorer.RollLogic;
using FateExplorer.Shared;

namespace FateExplorer.ViewModel;


public class BattlegroundViMo
{
    private BattlegroundM Battleground { get; set; }

    public int FreeModifier { 
        get => Battleground.FreeModifier; set => Battleground.FreeModifier = value; 
    }
    public WeaponsRange Distance { 
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
    public Movement Moving => Battleground.Moving;
    public Movement EnemyMoving {
        get => Battleground.EnemyMoving; set => Battleground.EnemyMoving = value;
    }
    public WeaponsReach EnemyReach => Battleground.EnemyReach;
    public EnemySize SizeOfEnemy {
        get => Battleground.SizeOfEnemy; set => Battleground.SizeOfEnemy = value;
    }
    public bool EnemyEvasive {
        get => Battleground.EnemyEvasive; set => Battleground.EnemyEvasive = value;
    }

    public BattlegroundViMo(bool useMainHand, WeaponViMo mainWeapon, WeaponViMo offWeapon)
    {
        Battleground = new(useMainHand, mainWeapon.ToWeaponM(), offWeapon.ToWeaponM());
    }

    public void ResetToDefault() => Battleground.ResetToDefault();

    public void SetMainWeapon(WeaponViMo mainWeapon) => Battleground.MainWeapon = mainWeapon.ToWeaponM();
    public void SetOffWeapon(WeaponViMo offWeapon) => Battleground.OffWeapon = offWeapon.ToWeaponM();
    public void SetHand(bool mainHand) => Battleground.UseMainHand = mainHand;


    public BattlegroundM ToM() => Battleground;
}
