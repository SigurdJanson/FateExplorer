namespace FateExplorer.Shared;



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
/// The hardness of the ground to determine fall damage
/// </summary>
public enum GroundCondition { Water = -4, Hay, Mud, Snow, Soil, Stone, Gravel, Grit, Boulders }


/// <summary>
/// A category of a roll in the rolling sequence
/// </summary>
public enum RollType { Primary = 0, Confirm = 1, Damage = 2, Botch = 3, BotchDamage = 4 }

/// <summary>
/// Types of armed conflicts
/// </summary>
public enum CombatBranch
{
    Unspecififed = 0, Unarmed = 1, Melee = 2, Ranged = 3, Shield = 4
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


#region Combat

public enum WeaponsReach
{
    /// <summary>
    /// Anything up to a short sword incl. brawling weapons
    /// </summary>
    Short = 1,
    /// <summary>
    /// Most swords, axes, hammers, etc. usually incl. two-handed versions
    /// </summary>
    Medium,
    /// <summary>
    /// Polearms, lances, long staffs
    /// </summary>
    Long
}

/// <summary>
/// Long range weapons
/// </summary>
public enum WeaponsRange
{
    /// <summary>
    /// Short-range
    /// </summary>
    Short = 1,
    /// <summary>
    /// Medium-range
    /// </summary>
    Medium,
    /// <summary>
    /// Long-range
    /// </summary>
    Long
}


/// <summary>
/// Shield sizes
/// </summary>
public enum ShieldSize
{
    /// <summary>
    /// Small shield
    /// </summary>
    Small = 1,
    /// <summary>
    /// Medium-sized shield
    /// </summary>
    Medium,
    /// <summary>
    /// Large shield
    /// </summary>
    Large
}


public enum EnemySize {
    /// <summary>
    /// rat, toad, sparrow
    /// </summary>
    Tiny,
    /// <summary>
    /// fawn, sheep, goat
    /// </summary>
    Small,
    /// <summary>
    /// human, dwarf, donkey
    /// </summary>
    Medium,
    /// <summary>
    /// ogre, troll, cow
    /// </summary>
    Large,
    /// <summary>
    /// dragon, giant, elephant
    /// </summary>
    Huge
}

public enum Movement
{
    /// <summary>
    /// Stationary (mounted or on foot)
    /// </summary>
    None = 0,
    /// <summary>
    /// Walking; character (either target or attacker) is moving slowly (4 yards or less in its last action)
    /// </summary>
    Slow,
    /// <summary>
    /// Running; character (either target or attacker) is moving quickly (5 yards or more in its last action)
    /// </summary>
    Fast,
    /// <summary>
    /// Mount is moving at a walk
    /// </summary>
    GaitWalk,
    /// <summary>
    /// Mount is moving at a trot
    /// </summary>
    GaitTrot,
    /// <summary>
    /// Mount is moving at a gallop
    /// </summary>
    GaitGallop
}

public enum Vision
{
    /// <summary>
    /// Sight clear and undisturbed
    /// </summary>
    Clear = 0,
    /// <summary>
    /// Vision slightly impaired. Sparse leaves, morning mist
    /// </summary>
    Impaired,
    /// <summary>
    /// Target’s shape can be seen. Fog, moonlight
    /// </summary>
    ShapesOnly,
    /// <summary>
    /// Target’s shape can be roughly seen. Dense fog, starlight
    /// </summary>
    Barely,
    /// <summary>
    /// Target cannot be seen. Thick smoke, complete darkness
    /// </summary>
    NoVision
}


public enum UnderWater
{
    /// <summary>
    /// Standing on dry ground
    /// </summary>
    Dry = 0,
    /// <summary>
    /// Standing knee-deep in water or swamp
    /// </summary>
    KneeDeep,
    /// <summary>
    /// Standing in water from the waist up
    /// </summary>
    WaistDeep,
    /// <summary>
    /// Standing in water from the belly up
    /// </summary>
    ChestDeep,
    /// <summary>
    /// Standing in water with only the head above the surface
    /// </summary>
    NeckDeep,
    /// <summary>
    /// Under water
    /// </summary>
    Submerged
}

#endregion




#region Calendar 

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

#endregion


#region Objects
public enum GroupId
{
    iMelee = 1, iRangedWeapon = 2, iAmmunition = 3, iArmor = 4, iWeaponAccessories = 5,
    iClothing = 6, iTravelNTools = 7, iLights = 8, iBandagesNRemedies = 9, iContainers = 10,
    iRopesNChains = 11, iBurglarTools = 12, iTools = 13, iOrientation = 14, iJewellery = 15,
    iFinestoneNGems = 16, iStationary = 17, iBooks = 18, iMagicalArtefacts = 19,
    iAlchimica = 20, iPoisons = 21, iHerbs = 22, iMusicalInstruments = 23, iStimulantsNLuxury = 24, iAnimal = 25,
    iAnimalSupplies = 26, iTransport = 27, iSacredTools = 28, iCeremonialObjects = 29
};

/// <summary>
/// Quality level for products and services (e.g. inns, hotels, etc.)
/// </summary>
public enum QualityLevel
{
    /// <summary>Lowest quality level.</summary>
    Lowest = 1,
    Low = 2,
    /// <summary>The default (median)  quality level without modifications.</summary>
    Normal = 3,
    Good = 4,
    Excellent = 5,
    /// <summary>Highest quality level.</summary>
    Luxurious = 6
}

/// <summary>
/// Price level for services, inns, hotels, etc. Prices range from -50% to +400% around
/// the normal price.
/// </summary>
public enum PriceLevel
{
    /// <summary>Lowest price level.</summary>
    VeryCheap = 1,
    Cheap,
    /// <summary>The default (median) price level without modifications.</summary>
    Normal,
    Expensive,
    VeryExpensive,
    /// <summary>Highest price level.</summary>
    Horrendous
}

#endregion