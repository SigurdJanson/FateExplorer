﻿using System;

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

    public struct WeaponDTO
    {
        /// <summary>
        /// An object id (for the optolith it's the template id).
        /// </summary>
        public string Id { get; set; }

        public string CombatTechId { get; set; }

        public string Name { get; set; }

        public int AttackMod { get; set; }
        public int ParryMod { get; set; }

        public int DamageBonus { get; set; }

        public int DamageDieCount { get; set; }

        public int DamageDieSides { get; set; }

        public int DamageThreshold { get; set; }

        public bool Improvised { get; set; }
    }


    public struct CombatDTO : ICharacterAttributDTO
    {
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Max { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Min { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EffectiveValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }


    public struct DodgeDTO : ICharacterAttributDTO
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
    }
}