using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

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
    /// Constructor
    /// </summary>
    /// <param name="skill">The skill to check</param>
    /// <param name="ability">The abilities needed for the check</param>
    /// <param name="modificator">An optional modificator (may be <c>null</c>).</param>
    /// <param name="gameData">Access to the game data base</param>
    public RoutineSkillCheckM(SkillsDTO skill, AbilityDTO[] ability, ICheckModificatorM modificator, IGameDataService gameData) 
        : base(gameData)
    {
        RollAttr = new int[3];
        RollAttrName = new string[3];
        for (int a = 0; a < 3; a++)
        {
            RollAttr[a] = ability[a].EffectiveValue;
            RollAttrName[a] = ability[a].ShortName;
        }
        CheckModificator = modificator ?? new SimpleCheckModificatorM(Modifier.Neutral);
        CheckModificator.OnStateChanged += UpdateAfterModifierChange;

        TargetAttr = skill.EffectiveValue;
        TargetAttrName = skill.Name;

        Name = skill.Name;
        AttributeId = skill.Id;
        Domain = skill.Domain;

        RollList = new();
        GetRoll(RollType.Primary, AutoRoll: true);
    }


    /// <summary>
    /// Update the check assessment after a modificator update
    /// </summary>
    public override void UpdateAfterModifierChange()
    {
        int QualityLevel = RoutineSkillCheck(TargetAttr ?? 0, RollAttr, CheckModificator.Modifier);
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
            CheckModificator.OnStateChanged -= UpdateAfterModifierChange;

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

    public override bool NeedsBotchEffect
    {
        get => false;
    }


    public override int Remainder => RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, CheckModificator.Modifier);

    public override string ClassificationLabel => "QL";

    public override string Classification => RoutineSkillCheck(TargetAttr ?? 0, RollAttr, CheckModificator.Modifier).ToString();

    public override string ClassificationDescr => null;




    /// <inheritdoc/>
    /// <remarks>Not implemented</remarks>
    public override RollSuccess.Level SuccessOfRoll(RollType Which)
    {
        int RemainingSkillPoints = RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, CheckModificator.Modifier);
        return RemainingSkillPoints > 0 ? RollSuccess.Level.Success : RollSuccess.Level.Fail;
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
                int RemainingSkillPoints = RoutineSkillCheckRemainder(TargetAttr ?? 0, RollAttr, CheckModificator.Modifier);
                RollSuccess.Level s = RemainingSkillPoints > 0 ? RollSuccess.Level.Success : RollSuccess.Level.Fail;
                Success.Update(s, s);
            }
        }

        return RollList[Which];
    }
}
