using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    }


}
