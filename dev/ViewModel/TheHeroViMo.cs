using FateExplorer.CharacterData;
using FateExplorer.GameData;
using FateExplorer.GameLogic;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FateExplorer.ViewModel
{
    public struct SkillsDTO
    {
        public string Id;
        public string Name;
        public int SkillValue;
    }

    public class TheHeroViMo : ITheHeroViMo
    {
        protected IGameDataService GameDataService;
        protected ICharacterM characterM;

        public TheHeroViMo(IGameDataService gameData)
        {
            GameDataService = gameData;
        }


        public void ReadCharacterFile(byte[] Data) //(MemoryStream reader)
        {
            CharacterImportOptM characterImportOptM = JsonSerializer.Deserialize<CharacterImportOptM>(new ReadOnlySpan<byte>(Data));//(reader);

            characterM = new CharacterM(GameDataService, characterImportOptM);
        }



        /// <summary>
        /// Get the skills from a character.
        /// </summary>
        /// <param name="Domain">Mundane, arcane or karmic skill. Use null for all.</param>
        /// <param name="NameFilter">Return only skills with the filter string in the name.</param>
        /// <returns>
        /// List as DTO for the View. If no skill fits the criterion the list will be empty.
        /// </returns>
        public List<SkillsDTO> GetSkills(SkillDomain? Domain = null, string NameFilter = "")
        {
            List<SkillsDTO> Result = new();

            foreach (var s in characterM.Skills.Skills)
            {
                if (Domain is not null && s.Value.Domain != Domain)
                    continue;
                if (!s.Value.Name.Contains(NameFilter))
                    continue;

                var skill = new SkillsDTO
                {
                    Id = s.Key,
                    Name = s.Value.Name,
                    SkillValue = s.Value.Value
                };
                Result.Add(skill);
            }
            return Result;
        }

        /// <summary>
        /// Return a list with the most used skill
        /// </summary>
        /// <returns>List as DTO for the View</returns>
        public List<SkillsDTO> GetFavoriteSkills()
        {
            // TODO: put into config: "Sinnesschärfe", "Menschenkenntnis", "Heilkunde Wunden"
            string[] Favs = new string[3] { "TAL_10", "TAL_20", "TAL_50" };

            List<SkillsDTO> Result = new();

            foreach (var fav in Favs)
            {
                var s = characterM.Skills.Skills[fav];
                var skill = new SkillsDTO
                {
                    Id = fav,
                    Name = s.Name,
                    SkillValue = s.Value
                };
                Result.Add(skill);
            }
            return Result;
        }

        /// <summary>
        /// Return the skills with the highest skill value
        /// </summary>
        /// <param name="Count">How many top skills? Top 5, top 10?</param>
        /// <param name="IncludeTies">Return exactly 'Count' skills or shall skills with same 
        /// proficiency be added?</param>
        /// <returns>List as DTO for the View</returns>
        public List<SkillsDTO> GetBestSkills(uint Count = 4, bool IncludeTies = true)
        {
            List<SkillsDTO> Result = new();

            // Create a new list sorted by talent value, first
            List<(int Val, string Id)> SkillByValue = new();
            foreach (var s in characterM.Skills.Skills)
                SkillByValue.Add((s.Value.Value, s.Key));
            SkillByValue.Sort(delegate ((int, string) x, (int, string) y)
            {
                return y.Item1.CompareTo(x.Item1);
            });


            // Get the first `Count` elements
            int i;
            for (i = 0; i <= Count; i++)
            {
                string Key = SkillByValue[i].Id;
                Result.Add(new SkillsDTO()
                {
                    Id = Key,
                    Name = characterM.Skills.Skills[Key].Name,
                    SkillValue = characterM.Skills.Skills[Key].Value
                });
            }

            // Look for ties (if required and if there is at least one neglected tie
            if (IncludeTies && SkillByValue[i].Val == SkillByValue[i - 1].Val)
            {
                int TieValue = SkillByValue[i].Val;
                while (SkillByValue[i].Val == TieValue)
                {
                    string Key = SkillByValue[i].Id;
                    Result.Add(new SkillsDTO()
                    {
                        Id = Key,
                        Name = characterM.Skills.Skills[Key].Name,
                        SkillValue = characterM.Skills.Skills[Key].Value
                    });
                    i++;
                }
            }

            return Result;
        }


        public List<SkillDomain> GetMasteredSkillDomains()
        {
            List<SkillDomain> Result = new();
            foreach (var b in characterM.Skills.MasteredDomains)
                if (b.Value) Result.Add(b.Key);
            return Result;
        }
    }
}
