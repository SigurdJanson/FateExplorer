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
        /// The initiative value (INI)
        /// </summary>
        public InitiativeM Initiative { get; }

        /// <summary>
        /// The (imported) core value for movement (MOV)
        /// </summary>
        /// <remarks>Definition: Race Base Value +/– points from dis-/advantages</remarks>
        MovementM Movement { get; }

        /// <summary>
        /// When a creature suffers damage equal to or greater than its wound threshold,
        /// it must make a check to see if it suffers a wound effect.
        /// </summary>
        /// <remarks>Definition: CON / 2 (rounded up)</remarks>
        WoundThresholdM WoundThreshold { get; }



        Dictionary<string, WeaponM> Weapons { get; }
        Dictionary<string, BelongingM> Belongings { get; }

        /// <summary>
        /// list: courage, sagacity, etc.
        /// </summary>
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
        decimal CarriedWeight { get; }

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
        decimal WhatCanCarry(int EffectiveStrength);

        /// <summary>
        /// For brief periods a character may lift something below or equal to this weight
        /// (see VR1, p. 348)..
        /// </summary>
        /// <param name="EffectiveStrength">The current strength</param>
        /// <returns>The weight in stone (i.e. kg)</returns>
        decimal WhatCanLift(int EffectiveStrength);
    }
}