using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.ViewModel;

/// <summary>
/// This class may hold the effective values of a character to be stored in a client-side storage.
/// </summary>
public class HeroStorageDTO
{
    /// <inheritdoc cref="TheHeroViMo.AbilityEffValues"/>
    [JsonPropertyName("ab")]
    public Dictionary<string, int> Abilities { get; set; }

    /// <inheritdoc cref="TheHeroViMo.DodgeTrueValue"/>
    [JsonPropertyName("doTrue")]
    public int DodgeTrue { get; set; }
    /// <inheritdoc cref="TheHeroViMo.DodgeEffMod"/>
    [JsonPropertyName("doMod")]
    public int DodgeMod { get; set; }

    /// <inheritdoc cref="EnergyViMo.EffectiveValue"/>
    [JsonPropertyName("energy")]
    public Dictionary<string, int> EffectiveEnergy { get; set; }
    /// <inheritdoc cref="EnergyViMo.EffMax"/>
    [JsonPropertyName("energyMax")]
    public Dictionary<string, int> EffectiveMaxEnergy { get; set; }

    /// <inheritdoc cref="TheHeroViMo.ResilienceEffValues"/>
    [JsonPropertyName("resilience")]
    public Dictionary<string, int> EffectiveResilience { get; set; }

    /// <inheritdoc cref="TheHeroViMo.EffectiveMoney"/>
    [JsonPropertyName("money")]
    public decimal? EffectiveMoney { get; set; }
}
