using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    public class WeaponMeleeDB : DataServiceCollectionBase<WeaponMeleeDbEntry>
    {
        // inherited

        [JsonPropertyName("range")]
        public int Reach { get; set; }
    }

    public class WeaponMeleeDbEntry : WeaponDbEntry
    {
        // Inherited properties 
    }
}
