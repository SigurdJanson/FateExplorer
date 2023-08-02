using FateExplorer.Shared;
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
        public override Check.Skill Domain { get => Check.Skill.Skill; }


        // TODO: no URLs in file
    }


}
