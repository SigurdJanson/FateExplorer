using System;

namespace FateExplorer.Shared;




/// <summary>
/// A full path to identify a roll check: the check id. This class is responsible for
/// creating valid check id's that fit those in "rollresolver.json".
/// That way, it is used to tell the roll handler which roll needs to be done, i.e.
/// which roll check class it has to choose.
/// </summary>
/// What value does this add?
/// * No hard-coding of "/" separator characters in various places of the code.
/// * Explicit contract for the developer without the need to think how check id strings are created.
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
    /// <param name="checkType">A basic type of check</param>
    /// <remarks>For skill and combat checks, use the designated constructors.</remarks>
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
    public Check(SkillDomain skill, bool isRoutine = false)
    {
        string SkillId = skill switch
        {
            SkillDomain.Basic => ChrAttrId.Skill,
            SkillDomain.Arcane => ChrAttrId.Spell,
            SkillDomain.Karma => ChrAttrId.Liturgy,
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
        if (!combatStyle?.StartsWith(ChrAttrId.SpecialAbilityBaseId) ?? false)
            throw new ArgumentException("Combat style seems invalid", nameof(combatStyle));

        string Action = (action == Combat.Attack) ? ChrAttrId.AT : ChrAttrId.PA;

        if (string.IsNullOrWhiteSpace(combatStyle))
            Id = $"{combatTechId}{Sep}{Action}"; // CT_9/AT
        else
            Id = $"{combatTechId}{Sep}{Action}{Sep}{combatStyle}"; // CT_9/AT/SA_186
    }

}