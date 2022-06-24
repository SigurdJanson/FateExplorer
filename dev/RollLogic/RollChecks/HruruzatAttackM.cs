using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using System;

namespace FateExplorer.RollLogic;

public class HruruzatAttackM : AttackCheckM
{
    /// <inheritdoc />
    public new const string checkTypeId = "DSA5/0/combat/attack/hruruzat";


    public HruruzatAttackM(WeaponM weapon, bool mainHand, WeaponM otherWeapon, ICheckModifierM modifier, IGameDataService gameData) 
        : base(weapon, mainHand, otherWeapon, modifier, gameData)
    {
    }



    /// <inheritdoc />
    /// <remarks>Returns the damage in case of combat attack checks 
    /// or the botch effect</remarks>
    public override string Classification
    {
        get
        {
            if (RollList[RollType.Damage] is not null)
            {
                string Doublet = (RollList[RollType.Damage] as BestOf2d6).IsDoublet ? " (Pasch)" : "";
                return $"{base.Classification}{Doublet}";
            }
            else
                return base.Classification;
        }
    }


    /// <summary>
    /// Roll the dice for the selected roll.
    /// </summary>
    /// <param name="Which">The desired roll</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private IRollM ThrowCup(RollType Which)
    {
        IRollM roll = Which switch
        {
            RollType.Primary => new D20Roll(),
            RollType.Confirm => NeedsConfirmation ? new D20Roll() : null,
            RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
            RollType.Damage => NeedsDamage ? new BestOf2d6() : null,
            _ => throw new ArgumentException("Combat rolls only support primary and confirmation rolls")
        };
        RollList[Which] = roll;

        if (Which == RollType.Primary || Which == RollType.Confirm)
            Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], CheckModifier.Apply(RollAttr[0]));

        return roll;
    }



    /// <inheritdoc />
    public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
    {
        if (AutoRoll && RollList[Which] is null)
            RollList[Which] = ThrowCup(Which);

        return RollList[Which];
    }
}
