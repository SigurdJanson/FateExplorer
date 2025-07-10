using System;

namespace FateExplorer.CharacterModel;

/// <summary>
/// Represents a basic activatable (dis-/advantage or special ability) using tiers 
/// (which usually represents some kind of level).
/// </summary>
public class TieredActivatableM : IActivatableM
{

    public TieredActivatableM(string id, int tier, string[] reference, bool recognized = false)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));
        if (tier < 0) throw new ArgumentException("Error in file: a tier of a special ability cannot be negative", nameof(tier));

        Id = id;
        Tier = tier;
        Reference = reference ?? Array.Empty<string>();
        IsRecognized = recognized;
    }

    /// <inheritdoc/>
    public string Id { get; set; }

    /// <inheritdoc/>
    public int Tier { get; set; }

    /// <inheritdoc/>
    /// <remarks>Special abilities know by this basic generic special ability class
    /// cannot be interpreted. Specific classes are required for that.</remarks>
    public virtual bool IsRecognized { get; set; }

    /// <inheritdoc/>
    public string[] Reference { get; set; }

    /// <summary>
    /// No effect unless this class is derived.
    /// </summary>
    public virtual void Apply(CharacterM character)
    {}
}
