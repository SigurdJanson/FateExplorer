using System;

namespace FateExplorer.CharacterModel.SpecialAbilities
{
    public class SpecialAbilitySwitchM : ISpecialAbilityM
    {
        public SpecialAbilitySwitchM(string id, int tier)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (tier < 0 || tier > 4) throw new ArgumentException("Language allows skill only between 0-4", nameof(tier));

            Id = id;
            Tier = tier;
        }


        /// <inheritdoc/>
        public string Id { get; protected set; }

        /// <inheritdoc/>
        public int Tier { get; protected set; }

        /// <inheritdoc/>
        /// <remarks>Special abilities know by this basic generic special ability class
        /// cannot be interpreted. Specific classes are required for that.</remarks>
        public bool IsRecognized => false;
    }
}
