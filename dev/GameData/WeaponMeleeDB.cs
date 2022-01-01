using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    public class WeaponMeleeDB : DataServiceCollectionBase<WeaponMeleeDbEntry>
    {
        // inherited

        [JsonPropertyName("range")]
        public int Range { get; set; }
    }

    public class WeaponMeleeDbEntry : WeaponDbEntry
    {
        // Inherited properties 
    }
}
