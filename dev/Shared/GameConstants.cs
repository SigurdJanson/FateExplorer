

public enum SkillDomain
{
    Basic = 0, Arcane = 1, Karma = 2
}

public enum ResilienceTypes
{
    TOU = 1, // Toughness/Zähigkeit
    SPI = 2  // Spirit/Seelenkraft
}

public enum CharacterEnergyClass
{
    LP = 1, AE = 2, KP = 3
}

public enum RegenerationSite
{
    Terrible = -3, Bad = -2, Poor = -1, Default = 0, Good = 1
}

public enum RegenerationDisturbance
{
    None = 0, Brief = -1, Prolonged = -2
}


/// <summary>
/// A category of a roll
/// </summary>
public enum RollType { Primary = 0, Confirm = 1, Damage = 2, Botch = 3, BotchDamage = 4 }

/// <summary>
/// Types of armed conflicts
/// </summary>
public enum CombatBranch
{
    Unarmed = 1, Melee = 2, Ranged = 3, Shield = 4
}

