using System;

namespace FateExplorer.Shared
{
    public interface ICharacterAttributDTO
    {
        /// <summary>
        /// Unique identifier fot the character attribute
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Name of the attribute
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The character's attribute value
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// The permitted minimum value.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The character's attribute value after adding/removing temporary changes.
        /// Reductions happen during gameplay and are defined by the game master.
        /// Calculated attributes may change due to dependencies.
        /// </summary>
        public int EffectiveValue { get; set; }
    }

    public struct AbilityDTO : ICharacterAttributDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }
    }

    public struct SkillsDTO : ICharacterAttributDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }

        /// <summary>
        /// Specifies what domain the skill is from (mundane, arcane or divine).
        /// </summary>
        public SkillDomain Domain;
    }

    public struct EnergyDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <summary>
        /// Allows users to set their character's energy themselves
        /// </summary>
        public int EffMax { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }

        /// <summary>
        /// Some energies have a number of consequences when reduced 
        /// by certain amounts i.e. crosing thresholds.
        /// </summary>
        public int CrossedThresholds;
    }

    public struct ResilienceDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }
    }

    public struct WeaponDTO : ICharacterAttributDTO
    {
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Max { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Min { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EffectiveValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }


    public struct CombatDTO : ICharacterAttributDTO
    {
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Max { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Min { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EffectiveValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}