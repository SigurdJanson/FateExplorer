using System.Collections.Generic;

namespace FateExplorer.CharacterData
{
    public interface ICharacterImporter
    {
        string GetName();

        string GetPlaceOfBirth();

        string GetDateOfBirth();



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
