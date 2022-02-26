﻿using FateExplorer.CharacterModel;
using FateExplorer.CharacterModel.SpecialAbilities;
using System.Collections.Generic;

namespace FateExplorer.CharacterImport
{
    public interface ICharacterImporter
    {
        string GetName();

        string GetPlaceOfBirth();

        string GetDateOfBirth();

        /// <summary>
        /// Return the identifier of the character's species
        /// </summary>
        /// <returns>A string in the format "R_{0}".</returns>
        string GetSpeciesId();


        /// <summary>
        /// Get the extra energy points the character has traded throughout their lives
        /// to get more points additional to the base formula.
        /// </summary>
        /// <param name="energyClass"></param>
        /// <returns></returns>
        int GetAddedEnergy(CharacterEnergyClass energyClass);


        /// <summary>
        /// Does the character have the advantage "Spellcaster" and - thus - has 
        /// a supply of arcane energy?
        /// </summary>
        /// <returns>true/false</returns>
        bool IsSpellcaster();

        /// <summary>
        /// Does the character have the advantage "Blessed" and - thus - has a 
        /// supply of karma energy?
        /// </summary>
        /// <returns>true/false</returns>
        bool IsBlessed();


        int CountAbilities();
        IEnumerable<KeyValuePair<string, int>> GetAbilities();


        int CountTalentSkills();
        IEnumerable<KeyValuePair<string, int>> GetTalentSkills();
        int GetTalentSkill(string Id);

        int CountArcaneSkills();
        IEnumerable<KeyValuePair<string, int>> GetArcaneSkills();

        int CountKarmaSkills();
        IEnumerable<KeyValuePair<string, int>> GetKarmaSkills();


        // SPECIAL ABILITIES

        /// <summary>
        /// Returns the ids of the character's special abilities
        /// </summary>
        /// <returns>List of id strings of all special abilities</returns>
        Dictionary<string, ISpecialAbilityM> GetSpecialAbilities();

        /// <summary>
        /// Returns the collection of languages spoken by the character.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, LanguageM> GetLanguages();

        /// <summary>
        /// Returns the ids of the character's advantages
        /// </summary>
        /// <returns>List of id strings</returns>
        Dictionary<string, ISpecialAbilityM> GetAdvantages();

        /// <summary>
        /// Returns the ids of the character's disadvantages
        /// </summary>
        /// <returns>List of id strings</returns>
        Dictionary<string, ISpecialAbilityM> GetDisadvantages();



        // PROPERTY / BELONGINGS

        double TotalWeightOfBelongings();

        IEnumerable<KeyValuePair<string, string>> GetWeapons();

    }
}
