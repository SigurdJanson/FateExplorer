﻿using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic;

public class HruruzatAttackM : AttackCheckM
{
    /// <inheritdoc />
    public new const string checkTypeId = "DSA5/0/combat/attack/hruruzat";


    public HruruzatAttackM(WeaponM weapon, WeaponM otherWeapon, bool isMainHand, BattlegroundM context, IGameDataService gameData)
        : base(weapon, otherWeapon, isMainHand, context, gameData)
    {
        Name = ResourceId.Hruruzat; // TODO #125: this is a crutch. It should be the already translated string.
    }


    /// <inheritdoc />
    public override string ClassificationLabel
    {
        get
        {
            if (RollList[RollType.Damage] is not null)
            {
                if ((RollList[RollType.Damage] as BestOf2d6).IsDoublet)
                    return ResourceId.DoubletId;
                else
                    return ResourceId.HitPoints;
            }
            else 
                return base.ClassificationLabel;
        }
    }


    /// <inheritdoc />
    /// <remarks>Returns the damage in case of combat attack checks 
    /// or the botch effect</remarks>
    public override string Classification
    {
        get
        {
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
            Success.Update(
                RollList[RollType.Primary], 
                RollList[RollType.Confirm], 
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Attack, CombatTech), Weapon));

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
