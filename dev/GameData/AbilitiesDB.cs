using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace FateExplorer.GameData
{

    public class AbilitiesDB : DataServiceCollectionBase<AbilityDbEntry>
    {
        // inherited
    }


    public class AbilityDbEntry
    {
        [JsonPropertyName("attrID")]
        public string AttrID { get; set; }

        [JsonPropertyName("shortname")]
        public string ShortName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }


}
