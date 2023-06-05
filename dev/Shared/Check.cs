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

    public enum Roll { Ability = 1, Regenerate = 2, Dodge = 4, Initiative = 8, 
        Any = Ability | Regenerate | Dodge | Initiative };

    [Flags]
    public enum Skill { 
        Skill = 1 << 12, // starts with 2^12 to avoid value overlap
        Arcane = Spell | Ritual, Spell = Skill*2, Ritual = Skill*4, 
        Karma = Chant | Ceremony, Chant = Skill*8, Ceremony = Skill*16,
        Any = Skill | Arcane | Karma
    };

    [Flags]
    public enum Combat { 
        Attack = 1 << 24, // starts with 2^24 to avoid value overlap
        Parry = Attack * 2,
        Any = Attack | Parry
    };

    private readonly int RollType { get; init; }


    /// <summary>
    /// A path-like string to uniquely identify a roll incl. (dis-) advantages and special abilities.
    /// </summary>
    public string Id { get; init; }

    public static implicit operator string(Check c) => c.Id;

    /// <summary>
    /// Is the check a general <see cref="Roll"/>? Does not support <see cref="Check.Roll.Any"/>.
    /// </summary>
    /// <param name="what">The value to check for</param>
    /// <returns><c>true</c> if the instance represents <c>what</c>.</returns>
    public bool Is(Roll what) => ((int)what & RollType) == (int)what;
    /// <summary>
    /// Is the check a <see cref="Skill"/> check?  Does not support <see cref="Check.Skill.Any"/>.
    /// </summary>
    /// <param name="what">The value to check for</param>
    /// <returns><c>true</c> if the instance represents <c>what</c>.</returns>
    public bool Is(Skill what) => ((int)what & RollType) == (int)what;
    /// <summary>
    /// Is the check a <see cref="Combat"/> check? If the check is <c>Combat.Any</c> it will return
    /// <c>true</c> for <c>Combat.Attack</c> or <c>Combat.Parry</c>. But the other way round is not true.
    /// <c>check.Is(Check.Combat.Any)</c> will only return <c>true</c> if the check is initialised as <c>Any</c>,
    /// too.
    /// </summary>
    /// <param name="what">The value to check for</param>
    /// <returns><c>true</c> if the instance represents <c>what</c>.</returns>
    public bool Is(Combat what) => ((int)what & RollType) == (int)what;
    public bool Is(Combat what, CombatBranch whatBranch)
    {
        return Is(what) & (Is(whatBranch) | whatBranch == CombatBranch.Unspecififed);
    }
    public bool Is(CombatBranch what) => (ComputeBranch(what) & RollType) != 0;

    public bool IsRoll => (RollType & (int)Roll.Any) > 0;
    public bool IsSkill => (RollType & (int)Skill.Any) > 0;
    public bool IsCombat => (RollType & (int)Combat.Any) > 0;

    private static int ComputeRoll(Roll value) => (int)value;
    private static int ComputeRoll(Skill value) => (int)value;
    private static int ComputeRoll(Combat value, CombatBranch branch) => 
        (int)value | ComputeBranch(branch);
    private static int ComputeBranch(CombatBranch branch) => branch switch
    {
        CombatBranch.Unarmed => (int)Combat.Parry * 2,
        CombatBranch.Melee => (int)Combat.Parry * 4,
        CombatBranch.Ranged => (int)Combat.Parry * 8,
        CombatBranch.Shield => (int)Combat.Parry * 16,
        _ => 0
    };


    /// <summary>
    /// Constructor for basic roll checks
    /// </summary>
    /// <param name="roll">A basic type of check</param>
    /// <remarks>For skill and combat checks, use the designated constructors.</remarks>
    public Check(Roll roll)
    {
        Id = roll switch
        {
            Roll.Ability => ChrAttrId.AbilityBaseId,
            Roll.Regenerate => ChrAttrId.Regenerate,
            Roll.Dodge => ChrAttrId.DO,
            Roll.Initiative => ChrAttrId.INI,
            _ => throw new ArgumentException("Unknown roll", nameof(roll)),
        };
        RollType = ComputeRoll(roll);
     }


    /// <summary>
    /// Constructor for skill checks
    /// </summary>
    public Check(Check.Skill skill, bool isRoutine = false)
    {
        string SkillId = skill switch
        {
            Check.Skill.Skill => ChrAttrId.Skill,
            Check.Skill.Arcane or Check.Skill.Spell or Check.Skill.Ritual => ChrAttrId.Spell,
            Check.Skill.Karma or Check.Skill.Chant or Check.Skill.Ceremony => ChrAttrId.Liturgy,
            _ => throw new ArgumentException("Unknown skill", nameof(skill)),
        };
        if (isRoutine)
            Id = $"{ChrAttrId.Routine}{Sep}{SkillId}"; // "RC/TAL"
        else
            Id = SkillId; // "TAL"

        RollType = ComputeRoll(skill);
    }


    /// <summary>
    /// Constructor for combat checks
    /// </summary>
    public Check(
        Combat action, string combatTechId, CombatBranch branch = CombatBranch.Unspecififed, string combatStyle = null)
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

        RollType = ComputeRoll(action, branch);
    }


    
}