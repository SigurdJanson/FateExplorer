using System.Collections.Generic;

namespace FateExplorer.CharacterData
{
    public interface ICharacterImporter
    {
        string GetName();

        string GetPlaceOfBirth();

        string GetDateOfBirth();


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

    }
}
