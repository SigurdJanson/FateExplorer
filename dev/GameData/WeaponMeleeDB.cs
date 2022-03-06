using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    public class WeaponMeleeDB : DataServiceCollectionBase<WeaponMeleeDbEntry>
    {
        // inherited

    }

    public class WeaponMeleeDbEntry : WeaponDbEntry
    {
        // Inherited properties 

        [JsonPropertyName("reach")]
        public int Reach { get; set; }
    }
}
