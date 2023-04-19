using FateExplorer.RollLogic;
using FateExplorer.Shared;

namespace FateExplorer.ViewModel;


public class BattlegroundViMo
{
    private BattlegroundM Battleground { get; set; }

    public int FreeModifier => Battleground.FreeModifier;
    public WeaponsRange Distance => Battleground.Distance;
    public Vision View => Battleground.Visibility;
    public UnderWater Water => Battleground.Water;
    public bool CrampedSpace => Battleground.CrampedSpace;
    public Movement Moving => Battleground.Moving;
    public Movement EnemyMoving => Battleground.EnemyMoving;
    public WeaponsReach EnemyReach => Battleground.EnemyReach;
    public EnemySize SizeOfEnemy => Battleground.SizeOfEnemy;
    public bool EnemyEvasive => Battleground.EnemyEvasive;

    public BattlegroundViMo(bool useMainHand, WeaponViMo mainWeapon, WeaponViMo offWeapon)
    {
        Battleground = new(useMainHand, mainWeapon.ToWeaponM(), offWeapon.ToWeaponM());
    }

    public void ResetToDefault() => Battleground.ResetToDefault();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Value"></param>
    /// <returns></returns>
    public int ModifyAttack(int Value)
    {
        return Battleground.ApplyTotalMod(Value, Check.Combat.Attack);
    }


    public int ModifyParry(int Value)
    {
        return Battleground.ApplyTotalMod(Value, Check.Combat.Parry);
    }
    public int ModifyDodge(int Value)
    {
        return Value - 299;
    }


    public BattlegroundM ToM() => Battleground;
}
