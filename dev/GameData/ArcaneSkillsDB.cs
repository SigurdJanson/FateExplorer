using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class ArcaneSkillsDB : DataServiceCollectionBase<ArcaneSkillDbEntry>
    {
        // inherited
    }



    public class ArcaneSkillDbEntry : SkillDbEntryBase
    {
        [JsonPropertyName("modagainst")]
        public string ModAgainst { get; set; }

        [JsonPropertyName("category")] // TODO: most entries in file have not category
        public string Category { get; set; }

        [JsonIgnore]
        public override SkillDomain Domain { get => SkillDomain.Arcane; }
    }


}
