using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class SkillsDB : DataServiceCollectionBase<SkillDbEntry>
    {
        // inherited
    }



    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class SkillDbEntry : SkillDbEntryBase
    {
        // Inherited properties

        [JsonIgnore]
        public override SkillDomain Domain { get => SkillDomain.Basic; }


        // TODO: no URLs in file
    }


}
