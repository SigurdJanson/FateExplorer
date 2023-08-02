using FateExplorer.Shared;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class KarmaSkillsDB : DataServiceCollectionBase<KarmaSkillDbEntry>
    {
        // inherited
    }



    public class KarmaSkillDbEntry : SkillDbEntryBase
    {
        [JsonPropertyName("tradition")]
        public string[] Tradition { get; set; }

        [JsonPropertyName("chanttype")]
        public string Type { get; set; }

        [JsonPropertyName("modagainst")]
        public string ModAgainst { get; set; }

        [JsonIgnore]
        public override Check.Skill Domain { get => Check.Skill.Karma; }
    }


}
