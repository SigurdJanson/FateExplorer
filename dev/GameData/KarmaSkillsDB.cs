using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public override SkillDomain Domain { get => SkillDomain.Karma; }
    }


}
