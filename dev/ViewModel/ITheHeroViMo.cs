using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{
    public interface ITheHeroViMo
    {
        // SETUP

        /// <value>
        /// Notify registered components when the hero has changed.
        /// </value>
        public event Action OnChange;

        /// <value>
        /// Has character data been read. Then the character has been born.
        /// </value>
        bool HasBorn { get; }

        /// <summary>
        /// Import a character file with the Optolith importer
        /// </summary>
        /// <param name="Data">The json data as byte array</param>
        Task ReadCharacterFile(byte[] Data);

        /// <value>Character's name (read-only)</value>
        string Name { get; }
        /// <value>Character's place of birth (read-only)</value>
        string PlaceOfBirth { get; }
        /// <value>Birthday (read-only)</value>
        string DateOfBirth { get; }
        /// <value>The total  weight of the character's belongings (read-only)</value>
        double CarriedWeight { get; }
        /// <value>The weight the character can carry (for a longer period of time, like in a backpack) (read-only)</value>
        double WhatCanCarry { get; }
        /// <value>The weight the character can lift (for a shorter period) (read-only)</value>
        double WhatCanLift { get; }

        decimal Money { get; set; }

        /// <summary>
        /// Get the character's money as formatted string
        /// </summary>
        /// <returns></returns>
        string FormatMoney();

        /// <summary>
        /// (Effective) Initiative value of the character
        /// </summary>
        int Initiative { get; }

        /// <summary>
        /// Get the 
        /// </summary>
        /// <returns></returns>
        CharacterAttrDTO GetInitiative();

        List<AbilityDTO> GetAbilites();

        /// <summary>
        /// Provide all special abilities mastered by the hero.
        /// </summary>
        /// <returns>List of special abilities (as DTO)</returns>
        List<SpecialAbilityDTO> GetSpecialAbilities();

        /// <summary>
        /// Provide all combat style special abilities mastered by the hero. If 
        /// </summary>
        /// <param name="CombatTecId">Filter: A combat technique (as id string) the special abilities shall fit.</param>
        /// <returns>List of special abilities (as DTO)</returns>
        List<SpecialAbilityDTO> GetCombatStyleSpecialAbilities(string CombatTecId);

        /// <summary>
        /// Provide the languages the hero can speak.
        /// </summary>
        /// <returns>A list of languages (as DTO)</returns>
        List<LanguageDTO> GetLanguages();

        List<DisAdvantageDTO> GetAdvantages();
        List<DisAdvantageDTO> GetDisadvantages();

        //
        /// <summary>
        /// Get the skills from a character.
        /// </summary>
        /// <param name="Domain">Mundane, arcane or karmic skill. Use null for all.</param>
        /// <param name="NameFilter">Return only skills with the filter string in the name.</param>
        /// <returns>
        /// List as DTO for the View. If no skill fits the criterion the list will be empty.
        /// </returns>
        List<SkillsDTO> GetSkills(SkillDomain? Domain = null, string NameFilter = "");

        /// <summary>
        /// Return the skills with the highest skill value
        /// </summary>
        /// <param name="Count">How many top skills? Top 5, top 10?</param>
        /// <param name="IncludeTies">Return exactly 'Count' skills or shall skills with same 
        /// proficiency be added?</param>
        /// <returns>List as DTO for the View</returns>
        List<SkillsDTO> GetBestSkills(uint Count = 4, bool IncludeTies = true);

        /// <summary>
        /// Returns a list with the presumably most used skills, esp. perception.
        /// </summary>
        /// <returns>List as DTO for the View</returns>
        List<SkillsDTO> GetFavoriteSkills();

        /// <summary>
        /// Returns the list of skill domains
        /// </summary>
        /// <returns>LIst of the skill domain enum with at least 1 value (i.e. "Basic")</returns>
        List<SkillDomain> GetMasteredSkillDomains();


        /// <summary>
        /// Returns the 3 abilities required to do a skill check.
        /// </summary>
        /// <param name="skillId">The identifier of the requested skill</param>
        /// <returns>An array with exactly three abilities</returns>
        AbilityDTO[] GetSkillAbilities(string skillId);

        /// <summary>
        /// Returns the effective dodge value
        /// </summary>
        /// <returns>The effective dodge value</returns>
        CharacterAttrDTO GetDodge();


        /// <summary>
        /// Returns toughness and spirit
        /// </summary>
        /// <returns></returns>
        List<ResilienceDTO> GetResiliences();

        /// <summary>
        /// Returns the list of energies () as available for the character
        /// </summary>
        /// <returns></returns>
        List<EnergyViMo> GetEnergies();

        /// <summary>
        /// Updates max and effective value
        /// </summary>
        /// <param name="energy"></param>
        /// <returns></returns>
        EnergyViMo OnEnergyChanged(EnergyViMo energy);

        /// <summary>
        /// The weapons carried by a character
        /// </summary>
        List<WeaponViMo> Weapons { get; }

        ///// <summary>
        ///// The weapon held by the dominant hand; is either a set weapon or it is the bare hands
        ///// </summary>
        //WeaponViMo DominantHandWeapon { get; set; }

        ///// <summary>
        ///// The weapon held by the NON-dominant hand
        ///// </summary>
        //WeaponViMo NondominantHandWeapon { get; set; }

        /// <summary>
        /// Hands carrying weapons
        /// </summary>
        HandsViMo Hands { get; }
    }
}