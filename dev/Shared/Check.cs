using System;

namespace FateExplorer.Shared;



// What value does this add?
// * No hard-coding of "/" separator characters in various places of the code.
// * Explicit contract for the developer without the need to think how check id strings are created.

/// <summary>
/// A full path to identify a roll check. 
/// It is used to tell the roll handler which roll needs to be done.
/// </summary>
public readonly struct Check
{
    private const char Sep = '/';

    public enum Roll { Ability, Regenerate, Dodge, Initiative };
    public enum Combat { Attack, Parry };
    public enum Skill { Skill, Spell, Liturgy };

    /// <summary>
    /// 
    /// </summary>
    public string Id { get; init; }

    public static implicit operator string(Check c) => c.Id;

    /// <summary>
    /// Constructor for basic roll checks
    /// </summary>
    public Check(Roll checkType)
    {
        Id = checkType switch
        {
            Roll.Ability => ChrAttrId.AbilityBaseId,
            Roll.Regenerate => ChrAttrId.Regenerate,
            Roll.Dodge => ChrAttrId.DO,
            Roll.Initiative => ChrAttrId.INI,
            _ => throw new ArgumentException("Unknown roll", nameof(checkType)),
        };
     }


    /// <summary>
    /// Constructor for skill checks
    /// </summary>
    public Check(Skill skill, bool isRoutine = false)
    {
        string SkillId = skill switch
        {
            Skill.Skill => ChrAttrId.Skill,
            Skill.Spell => ChrAttrId.Spell,
            Skill.Liturgy => ChrAttrId.Liturgy,
            _ => throw new ArgumentException("Unknown skill", nameof(skill)),
        };
        if (isRoutine)
            Id = $"{ChrAttrId.Routine}{Sep}{SkillId}"; // "RC/TAL"
        else
            Id = SkillId; // "TAL"
    }


    /// <summary>
    /// Constructor for combat checks
    /// </summary>
    public Check(Combat action, string combatTechId, string combatStyle = null)
    {
        if (!combatTechId.StartsWith(ChrAttrId.CombatTecBaseId))
            throw new ArgumentException("Combat technique seems invalid", nameof(combatTechId));

        string Action = (action == Combat.Attack) ? ChrAttrId.AT : ChrAttrId.PA;

        if (string.IsNullOrWhiteSpace(combatStyle))
            Id = $"{combatTechId}{Sep}{Action}"; // CT_9/AT
        else
            Id = $"{combatTechId}{Sep}{Action}{Sep}{combatStyle}"; // CT_9/AT/SA_186
    }

}