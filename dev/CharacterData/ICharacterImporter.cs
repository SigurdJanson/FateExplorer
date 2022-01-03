using System.Collections.Generic;

namespace FateExplorer.CharacterData
{
    public interface ICharacterImporter
    {
        int CountAbilities();
        IEnumerable<KeyValuePair<string, int>> GetAbilities();


        int CountTalentSkills();
        IEnumerable<KeyValuePair<string, int>> GetTalentSkills();

        int CountArcaneSkills();
        IEnumerable<KeyValuePair<string, int>> GetArcaneSkills();

        int CountKarmaSkills();
        IEnumerable<KeyValuePair<string, int>> GetKarmaSkills();

    }
}
