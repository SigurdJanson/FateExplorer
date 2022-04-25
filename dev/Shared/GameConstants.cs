

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


/// <summary>
/// <list type="bullet">
///     <item>
///         <term>Terrible</term>
///         <description>Terrible surroundings (extremely bad weather)</description>
///     </item>
///     <item>
///         <term>Bad</term>
///         <description>Bad surroundings (wet, cold)</description>
///     </item>
///     <item>
///         <term>Poor</term>
///         <description>Poor campsite, failed Survival (Find Campsite) check</description>
///     </item>
///     <item>
///         <term>Default</term>
///         <description>Basic regeneration for 6 hours of sleep</description>
///     </item>
///     <item>
///         <term>Good</term>
///         <description>Good accommodations (single room in an inn)</description>
///     </item>
/// </list>
/// </summary>
public enum RegenerationSite
{
    Terrible = -3, Bad = -2, Poor = -1, Default = 0, Good = 1
}


/// <summary>
/// <list type="bullet">
///     <item>
///         <term>None</term>
///         <description>No interruption during the night</description>
///     </item>
///     <item>
///         <term>Brief</term>
///         <description>Interruption of nightly rest (e.g.dog watch, nighttime disturbance)</description>
///     </item>
///     <item>
///         <term>Prolonged</term>
///         <description>Lengthier interruption of nightly rest(e.g. sentry duty, night ambush)</description>
///     </item >
/// </list>
/// </summary>
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
    Fjarningish = 7,
    Garethi = 8,
    Goblinish = 9,
    Isdira = 10,
    Mohish = 11,
    Nujuka = 12,
    Ogrish = 13,
    Oloarkh = 14,
    Ologhaijan = 15,
    RavenTongue = 16,
    Rogolan = 17,
    Rssahh = 18,
    Ruuz = 19,
    SagaThorwalian = 20,
    Thorwalian = 21,
    Trollish = 22,
    Tulamidya = 23,
    AncientTulamidya = 24,
    Zelemja = 25,
    Zhayad = 26,
    Cyclopean = 27,
    Dschuku = 49,
    Pardiral = 72
}

public enum LanguageAbility
{
    lngNone = 0, lngBroken = 1, lngBasic = 2, lngNoAccent = 3, lngNative = 4
}


public enum MagicProperty
{
    Antimagic = 1,
    Clairvoyance = 2,
    Demonic = 3,
    Elemental = 4,
    Healing = 5,
    Illusion = 6,
    Influence = 7,
    Object = 8,
    Spheres = 9,
    Telekinesis = 10,
    Transformation = 11,
    Temporal = 12
}


public enum MoonPhase
{
    /// <summary>
    /// New moon, Dead Mada
    /// </summary>
    New = 1, 
    SmallChalice,
    Chalice,

    /// <summary>
    /// Half moon (waxing)
    /// </summary>
    LargeChalice,
    ThreeFifths,
    FourFfths,
    
    /// <summary>
    /// Full moon, Wheel
    /// </summary>
    Wheel,
    WaningWheel,
    Waning,

    /// <summary>
    /// Half moon (waning)
    /// </summary>
    LargeHelmet,
    Helmet,
    SmallHelmet
}


public enum Season
{
    Summer = 1, Autumn, Winter, Spring
}