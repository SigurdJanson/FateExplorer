
enum DereCultureArea
{
    Middenrealmish = 100, Tulamidyan = 200, Thorwalian = 300, NorthernNatives = 400, SouthernNatives = 500, Mixed = 600, NonHuman = 700
}

enum DereCulture
{
    Albernia = 110, Bornland, CyclopeanIslnads, HorasEmpire, Middenrealm = 150, Andergast = 170, Nostria = 180, // Middenrealmish
    Fjarnings = 310, Gjalsks, Thorwal, // Thorwalian
    Arania = 210, Ferkina, Mhanadistan, Novadi, /*Tulamidya,*/ Zahori, // Tulamidyan
    Nivese = 410, Norbard, TrollPeaks, /* Nuanaä-Lie */ // NorthernNatives
    Utulu = 510, /*Tocamuyac, Waldmenschen*/ // SouthernNatives
    Amazons = 610, Maraskan = 620, Southerners = 630,  /* Bukanier */ // Mixed
    Cyclops = 710, Achaz = 720, Dwarve = 730, Elf = 740, Goblin = 750, Grolm = 760, Orc = 770, Ogre, Troll, Yeti, // Non-human
    HighNorth = 998, // non-native humans in the high north
    Shadowlands = 999, SvelltValley
}


enum SubSoutherners
{
    Kemi = DereCulture.Southerners+1, Selem, ColonialHarbours, SouthCityStates /*Thalusia, Al'Anfa*/
}


enum SubMiddenrealm
{
    Almada = DereCulture.Middenrealm+1, Albernia, Garetia, Griffonsford, Kosh, Northmarches, 
    Perricum, Ravenmarches, RommilysianMarches, Sunmark, Tobrien, Warunk, Weiden, Windhag
}

enum SubElves
{
    GladeElves = DereCulture.Elf+1, SteppeElves, Highelves, Shakagra, Woodelves, Firnelves
}

enum SubDwarves
{
    HillDwarves = DereCulture.Dwarve+1, ForgeDwarves, OreDwarves, DiamondDwarves, DeepDwarves, WildDwarves
}

enum SubOrcs
{
    Orcland = DereCulture.Orc+1, SvelltValley, Yurach /* Svellt valley Orcs are the occupying force of Orcs there */
}
enum SubGoblin
{
    FestumGhetto = DereCulture.Goblin+1, GoblinGangs, TribalGoblins
}
enum SubAchaz
{
    TribalAchaz = DereCulture.Achaz+1, ArchaicAchaz
}

// Al’Anfa und der Tiefe Süden
// Freie Städte des Nordens und des Dominium Donnerbach.
// Namen der Nordprovinzen
// Bühnen - &Künstlernamen
// Südmeer & Bukanier
// Waldmenschenstämme
// The aquatic beings include the nixies and water nymphs, the toad-headed krakons, the zilits, which take the shape of newts, and the fish-eyed risso of the Southern Seas
// whose bodies are adorned with shining scales. The blue mar (and their cousins, the black mar) live near the Iron Edge and in the depths of the glacial sea and seem to be
// relatives of the risso but little else is known of them.
