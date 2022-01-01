using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        // TODO: no URLs in file
    }


}
