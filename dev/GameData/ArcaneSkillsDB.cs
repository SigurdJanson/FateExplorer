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

        [JsonPropertyName("property")]
        public MagicProperty Property { get; set; }

        [JsonIgnore]
        public override SkillDomain Domain { get => SkillDomain.Arcane; }
    }


}
