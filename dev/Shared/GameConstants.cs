

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


/// <summary>
/// The languages a character can speak
/// </summary>
public enum LanguageId
{
    Alaani = 1,
    Angram = 2,
    Asdharia = 3,
    Atak = 4,
    Aureliani = 5,
    Bosparano = 6,
    Fjarningsch = 7,
    Garethi = 8,
    Goblinisch = 9,
    Isdira = 10,
    Mohisch = 11,
    Nujuka = 12,
    Ogrisch = 13,
    Oloarkh = 14,
    Ologhaijan = 15,
    Rabensprache = 16,
    Rogolan = 17,
    Rssahh = 18,
    Ruuz = 19,
    SagaThorwalsch = 20,
    Thorwalsch = 21,
    Trollisch = 22,
    Tulamidya = 23,
    UrTulamidya = 24,
    Zelemja = 25,
    Zhayad = 26,
    Zyklopäisch = 27,
    Dschuku = 49,
    Pardiral = 72
}

public enum LanguageAbility
{
    None = 0, Broken = 1, Basic = 2, WithoutAccent = 3, Native = 4
}
