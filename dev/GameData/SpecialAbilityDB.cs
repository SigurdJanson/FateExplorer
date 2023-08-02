using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    public enum SpecAbGroups { 
        General =  1,
        ExtendedSkills =  2,
        Skillstiles =  3,
        Commands =  4,
        Combat =  5,
        CombatStiles =  6,
        CombatExtended =  7,
        CombatBrawl =  8,
        FatePoint =  9,
        BlessedGeneral = 51,
        BlessedExtended = 52,
        BlessedTraditions = 53,
        LiturgyStiles = 54,
        Sermons = 55,
        Visions = 56,
        CeremonialObjects = 57,
        MagicGeneral = 21,
        MagicExtended = 22,
        MagicTraditions = 23,
        MagicStiles = 24,
        MagicSigns = 25,
        Traditionsartefakte = 26,
        LycanthropicGifts = 27,
        Homunculus = 28,
        SikaryanPredation = 29,
        PactGifts = 30}


    public class SpecialAbilityDB : DataServiceCollectionBase<SpecialAbilityDbEntry>
    {
        // inherited

        /// <summary>
        /// List containing the data base entries
        /// </summary>
        [JsonPropertyName("groups")]
        public IReadOnlyList<SpecialAbilityDbGroup> GroupNames { get; set; }

    }


    public class SpecialAbilityDbGroup
    {
        [JsonPropertyName("id")]
        public SpecAbGroups Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }


    public class SpecialAbilityDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("group")]
        public SpecAbGroups Group { get; set; }

        [JsonPropertyName("reco")]
        public bool Recognized { get; set; }


        [JsonPropertyName("ref")]
        public string[] Reference { get; set; }
        // TODO: no URLs in file
    }
}
