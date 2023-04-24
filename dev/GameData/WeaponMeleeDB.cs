using FateExplorer.Shared;
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
        public WeaponsReach Reach { get; set; }


        [JsonPropertyName("isparry")]
        public bool Parry { get; set; }

    }
}
