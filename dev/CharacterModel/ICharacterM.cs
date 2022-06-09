using System.Collections.Generic;

namespace FateExplorer.CharacterModel
{
    public interface ICharacterM
    {
        /// <summary>
        /// A unique id of a character (as received by an importer).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The character's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The character's place of birth as string.
        /// </summary>
        string PlaceOfBirth { get; }

        /// <summary>
        /// The character's date of birth as string.
        /// </summary>
        string DateOfBirth { get; }

        /// <summary>
        /// The character's race/species as Id.
        /// </summary>
        string SpeciesId { get; }


        /// <summary>
        /// Compute initiative based on given character abilities.
        /// </summary>
        /// <param name="courage">Character's courage value</param>
        /// <param name="agility">Character's agility value</param>
        /// <returns>Initiative value</returns>
        public int GetInitiative(int courage, int agility);


        /// <summary>
        /// The (imported) initiative value
        /// </summary>
        /// <remarks>Definition: (COU + AGI) / 2 +/– points from dis-/advantages</remarks>
        public int Initiative { get; }



        Dictionary<string, WeaponM> Weapons { get; }


        Dictionary<string, AbilityM> Abilities { get; }

        Dictionary<string, IActivatableM> SpecialAbilities { get; }
        Dictionary<string, LanguageM> Languages { get; }


        Dictionary<string, IActivatableM> Advantages { get; }
        bool HasAdvantage(string Id);
        Dictionary<string, IActivatableM> Disadvantages { get; }
        bool HasDisadvantage(string Id);


        Dictionary<string, CharacterEnergyM> Energies { get; }

        Dictionary<string, ResilienceM> Resiliences { get; }

        Dictionary<string, CombatTechM> CombatTechs { get; }

        DodgeM Dodge { get; }

        CharacterSkillsM Skills { get; }


        /// <summary>
        /// Return the ability identified by the given id.
        /// </summary>
        /// <param name="Id">a string id.</param>
        /// <returns>A numeric value characterising the character's effective ability</returns>
        int GetAbility(string Id);


        /// <summary>
        /// Check if the given special ability is activated for the character
        /// </summary>
        /// <param name="Id">A stringed </param>
        /// <returns></returns>
        bool HasSpecialAbility(string Id);

        /// <summary>
        /// The total weight of the character's belongings
        /// </summary>
        double CarriedWeight { get; }

        /// <summary>
        /// The characters money in silver thalers
        /// </summary>
        decimal Money { get; }


        /// <summary>
        /// The weight a character can carry over longer periods of time without effects of encumbrance
        /// (see VR1, p. 348).
        /// </summary>
        /// <param name="EffectiveStrength">The current strength</param>
        /// <returns>The weight in stone (i.e. kg)</returns>
        double WhatCanCarry(int EffectiveStrength);

        /// <summary>
        /// For brief periods a character may lift something below or equal to this weight
        /// (see VR1, p. 348)..
        /// </summary>
        /// <param name="EffectiveStrength">The current strength</param>
        /// <returns>The weight in stone (i.e. kg)</returns>
        double WhatCanLift(int EffectiveStrength);
    }
}