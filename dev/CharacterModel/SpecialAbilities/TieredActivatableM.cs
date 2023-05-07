using System;

namespace FateExplorer.CharacterModel
{
    /// <summary>
    /// Represents a basic activatable (dis-/advantage or special ability) using tiers (which usually represents some kind of level).
    /// </summary>
    public class TieredActivatableM : IActivatableM
    {
        public TieredActivatableM(string id, int tier, string[] reference)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (tier < 0) throw new ArgumentException("Language allows skill only between 0-4", nameof(tier));

            Id = id;
            Tier = tier;
            Reference = reference;
        }


        /// <inheritdoc/>
        public string Id { get; protected set; }

        /// <inheritdoc/>
        public int Tier { get; protected set; }

        /// <inheritdoc/>
        /// <remarks>Special abilities know by this basic generic special ability class
        /// cannot be interpreted. Specific classes are required for that.</remarks>
        public bool IsRecognized => false;

        /// <inheritdoc/>
        public string[] Reference { get; protected set; }
    }
}
