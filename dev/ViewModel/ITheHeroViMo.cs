using FateExplorer.GameData;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{
    public interface ITheHeroViMo
    {
        // SETUP
        void ReadCharacterFile(byte[] Data);

        //
        List<SkillsDTO> GetBestSkills(uint Count = 4, bool IncludeTies = true);
        List<SkillsDTO> GetFavoriteSkills();
        List<SkillsDTO> GetSkills(SkillDomain? Domain = null, string NameFilter = "");
        List<SkillDomain> GetMasteredSkillDomains();
    }
}