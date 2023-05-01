using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using static FateExplorer.Shared.Check;

namespace FateExplorer.RollLogic;

// Ignores RollList
public class RoutineSkillCheckM : CheckBaseM
{
    /// <inheritdoc />
    public new const string checkTypeId = "DSA5/0/skill/routine";

    /// <summary>
    /// The domain of the skill
    /// </summary>
    public Check.Skill Domain { get; protected set; }


    /// <inheritdoc />
    /// <remarks>In this context it is the skill value.</remarks>
    public override int? TargetAttr { get; protected set; }

    /// <inheritdoc />
    /// <remarks>In this context it is the skill.</remarks>
    public override string TargetAttrName { get; protected set; }


    /// <inheritdoc />
    /// <remarks>In this context the ability values</remarks>
    public override int[] RollAttr { get; protected set; }

    /// <inheritdoc />
    /// <remarks>In this context the ability values</remarks>
    public override string[] RollAttrName { get; protected set; }

    /// <summary>
    /// Currently active modifier that applies to the check
    /// </summary>
    protected Modifier EffectiveMod { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="skill">The skill to check</param>
    /// <param name="ability">The abilities needed for the check</param>
    /// <param name="modificator">An optional modificator (may be <c>null</c>).</param>
    /// <param name="gameData">Access to the game data base</param>
    public RoutineSkillCheckM(SkillsDTO skill, AbilityDTO[] ability, BaseContextM context, IGameDataService gameData) 
        : base(context, gameData)
    {
        RollAttr = new int[3];
        RollAttrName = new string[3];
        for (int a = 0; a < 3; a++)
        {
            RollAttr[a] = ability[a].EffectiveValue;
            RollAttrName[a] = ability[a].ShortName;
        }
        //Context = context; Already assigned through base
        Context.OnStateChanged += UpdateAfterModifierChange;

        TargetAttr = skill.EffectiveValue;
        TargetAttrName = skill.Name;

        Name = skill.Name;
        AttributeId = skill.Id;
        Domain = skill.Domain;
        EffectiveMod = Context.GetTotalMod(RollAttr[0], new Check(Domain, true), null);

        RollList = new();
        GetRoll(RollType.Primary, AutoRoll: true);
    }


    /// <summary>
    /// Update the check assessment after a modificator update
    /// </summary>
    public override void UpdateAfterModifierChange()
    {
        EffectiveMod = Context.GetTotalMod(RollAttr[0], new Check(Domain, true), null);
        if (EffectiveMod.Operator != Modifier.Op.Add) throw new NotImplementedException(); // to be safe

        int QualityLevel = RoutineSkillCheck(TargetAttr ?? 0, RollAttr, EffectiveMod);
        RollSuccess.Level Level = QualityLevel > 0 ? RollSuccess.Level.Success : RollSuccess.Level.Fail;

        Success.Update(Level, Level);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposedStatus)
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            // release unmanaged resources
            Context.OnStateChanged -= UpdateAfterModifierChange;

            if (disposedStatus) {/*Released managed resources*/}
        }
    }



    /// <summary>
    /// Return the quality level of a skill.
    /// </summary>
    /// <param name="Skill">The skill to be used</param>
    /// <param name="Abilities">The abilities needed to check that skill</param>
    /// <param name="Mod">The checks modificator</param>
    /// <returns>
    /// Quality level achieved by the routine check. A zero indicates that the skill 
    /// does not support a routine check for the given modificator.
    /// </returns>
    public static int RoutineSkillCheck(int SkillValue, int[] Abilities, Modifier Mod)
    {
        int RemainingSkillPoints = RoutineSkillCheckRemainder(SkillValue, Abilities, Mod);
        return SkillCheckM.ComputeSkillQuality(RemainingSkillPoints);
    }



    /// <summary>
    /// Return the remaining skill points of a routine check.
    /// </summary>
    /// <param name="Skill">The skill to be used</param>
    /// <param name="Abilities">The abilities needed to check that skill</param>
    /// <param name="Mod">The checks modificator</param>
    /// <returns>
    /// The remaining skill points.
    /// </returns>
    public static int RoutineSkillCheckRemainder(int SkillValue, int[] Abilities, Modifier Mod)
    {
        foreach (var Ability in Abilities)
            if (Ability < 13)
                return 0;

        bool SufficientSkill = (SkillValue > 0) && (SkillValue >= (-Mod.Delta(SkillValue) + 4) * 3 - 2);
        if (!SufficientSkill)
            return 0;
        else
            return (SkillValue + 1) / 2; // correct rounding
    }


    /// <inheritdoc />
    public override RollSuccess Success { get; protected set; }




    // ROLLS  ////////////////////////

    /// <inheritdoc/>
    public override bool NeedsBotchEffect => false;

    /// <inheritdoc/>
    public override int Remainder => RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, EffectiveMod);

    /// <inheritdoc/>
    /// <remarks>Not needed here</remarks>
    public override int ModDelta
    {
        get
        {
            EffectiveMod = Context.GetTotalMod(RollAttr[0], new Check(Domain, true), null);
            return (int)EffectiveMod;
        }
    }


    public override string ClassificationLabel => "QL";

    public override string Classification => RoutineSkillCheck(TargetAttr ?? 0, RollAttr, EffectiveMod).ToString();

    public override string ClassificationDescr => null;




    /// <inheritdoc/>
    /// <remarks>Not implemented</remarks>
    public override RollSuccess.Level SuccessOfRoll(RollType Which)
    {
        int RemainingSkillPoints = RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, EffectiveMod);
        return RemainingSkillPoints > 0 ? RollSuccess.Level.Success : RollSuccess.Level.Fail;
    }


    /// <inheritdoc/>
    public override Modifier RollModifier(RollType Which)
    {
        return Which switch
        {
            RollType.Primary => EffectiveMod,
            _ => throw new NotImplementedException()
        };
    }


    /// <inheritdoc/>
    /// <remarks>Not implemented</remarks>
    public override int[] RollRemainder(RollType Which)
    {
        if (Which != RollType.Primary)
            throw new ArgumentException("Skill rolls only support remainders for primary and confirmation rolls", nameof(Which));

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    /// <remarks>Not implemented</remarks>
    public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
    {
        if (AutoRoll && RollList[Which] is null)
        {
            IRollM Roll = new SkillRoll();
            Roll.OpenRoll[0] = 0;
            Roll.OpenRoll[1] = 0;
            Roll.OpenRoll[2] = 0;
            RollList[Which] = Roll;

            if (Which == RollType.Primary)
            {
                // NOTE: skill rolls cannot use the logic of RollSuccess which is made for a simple 1d20
                int RemainingSkillPoints = RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, EffectiveMod);
                RollSuccess.Level s = RemainingSkillPoints > 0 ? RollSuccess.Level.Success : RollSuccess.Level.Fail;
                Success.Update(s, s);
            }
        }

        return RollList[Which];
    }
}
