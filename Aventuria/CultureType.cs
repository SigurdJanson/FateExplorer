
enum TypeOfSpecies
{
    Human = 100, Mixed = 600, NonHuman = 700
}

enum CultureType
{
    Andergastan = 101, Aranian = 111, Bornlander = 121, Cyclopeans = 131, Fjarninger = 141, 
    Horasian = 151, Maraskan = 161, Mhanadistani = 171, Middenrealmer = 181, ForestPeople = 191, 
    Utulu = 201, Nivese = 211, Norbards = 221, NorthernAventurian = 231, Nostrian = 241,
    Novadi = 251, SouthernAventurian = 261, Svellter = 271, Thorwaler = 281,
    Elves = 701, Dwarves = 711, Orcs = 721, Goblins = 731, Achaz = 741, Grolm = 751, 
    Ogre = 771, Cyclopes = 781, Troll = 791, Yeti = 801
}

enum DereSubCulture
{
    Albernian = CultureType.Middenrealmer + 1, Koshan,
    Gjalsks, 
    Arania = 210, Ferkina, Mhanadistan, Novadi, /*Tulamidya,*/ Zahori, // Tulamidyan
    Nivese = 410, Norbard, TrollPeaks, /* Nuanaä-Lie */ // NorthernNatives
    Utulu = 510, /*Tocamuyac, Waldmenschen*/ // SouthernNatives
    Amazons = 610, Maraskan = 620, Southerners = 630,  /* Bukanier */ // Mixed
    HighNorth = 998, // non-native humans in the high north
    Shadowlands = 999, SvelltValley
}


enum SubSoutherners
{
    Kemi = DereSubCulture.Southerners+1, Selem, ColonialHarbours, SouthCityStates /*Thalusia, Al'Anfa*/
}


enum SubMiddenrealm
{
    Almada = CultureType.Middenrealmer + 1, Albernia, Garetia, Griffonsford, Kosh, Northmarches, 
    Perricum, Ravenmarches, RommilysianMarches, Sunmark, Tobrien, Warunk, Weiden, Windhag
}

enum SubElves
{
    GladeElves = CultureType.Elves+1, SteppeElves, Highelves, Shakagra, Woodelves, Firnelves
}

enum SubDwarves
{
    HillDwarves = CultureType.Dwarves+1, ForgeDwarves, OreDwarves, DiamondDwarves, DeepDwarves, WildDwarves
}

enum SubOrcs
{
    Orcland = CultureType.Orcs+1, SvelltValley, Yurach /* Svellt valley Orcs are the occupying force of Orcs there */
}
enum SubGoblin
{
    FestumGhetto = CultureType.Goblins+1, GoblinGangs, TribalGoblins
}
enum SubAchaz
{
    TribalAchaz = CultureType.Achaz+1, ArchaicAchaz
}
