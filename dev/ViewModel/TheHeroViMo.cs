using FateExplorer.CharacterData;
using FateExplorer.GameData;
using FateExplorer.GameLogic;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FateExplorer.ViewModel
{
    public struct AbilityDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int EffectiveValue { get; set; }
    }

    public struct SkillsDTO
    {
        public string Id;
        public string Name;
        public int SkillValue;
    }

    public struct EnergyDTO
    {
        public string Id;
        public string Name;
        public string ShortName;
        public int Max;
        public int Min;
        public int EffectiveValue;
        public int CrossedThresholds;
    }

    public struct ResilienceDTO
    {
        public string Id;
        public string Name;
        public string ShortName;
        public int Max;
        public int EffectiveValue;
    }



    public class TheHeroViMo : ITheHeroViMo
    {
        protected IGameDataService GameDataService;
        protected ICharacterM characterM;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameData"></param>
        public TheHeroViMo(IGameDataService gameData)
        {
            GameDataService = gameData;
        }

        /// <inheritdoc/>
        public bool HasBorn { get; protected set; }

        /// <inheritdoc/>
        public void ReadCharacterFile(byte[] Data)
        {
            CharacterImportOptM characterImportOptM = JsonSerializer.Deserialize<CharacterImportOptM>(new ReadOnlySpan<byte>(Data));//(reader);

            characterM = new CharacterM(GameDataService, characterImportOptM);
            InitAttributes();
            HasBorn = true;
        }

        /// <summary>
        /// Sets the effective values after a new character was loaded.
        /// </summary>
        protected void InitAttributes()
        {
            if (characterM is null) 
                throw new Exception("Interner Programmfehler; keine geladenen Daten.");

            // ABILITIES
            if (AbilityEffValues is null)
                AbilityEffValues = new();
            else
                AbilityEffValues.Clear();
            foreach (var chab in characterM?.Abilities)
                AbilityEffValues.Add(chab.Key, chab.Value.Value);
        }

        public string Name { get => characterM?.Name ?? ""; }
        public string PlaceOfBirth { get => characterM?.PlaceOfBirth ?? ""; }
        public string DateOfBirth { get => characterM?.DateOfBirth ?? ""; }


        #region ABILITIES
        Dictionary<string, int> AbilityEffValues { get; set; }

        public List<AbilityDTO> GetAbilites()
        {
            var Result = new List<AbilityDTO>();

            foreach(var chab in characterM?.Abilities)
            {
                Result.Add(new AbilityDTO()
                {
                    Id = chab.Value.Id,
                    Name = chab.Value.Name,
                    ShortName = chab.Value.ShortName,
                    EffectiveValue = AbilityEffValues[chab.Key],
                    Max = chab.Value.Value,
                    Min = 0
                });
            }

            return Result;
        }
        #endregion



        #region SKILLS
        /// <inheritdoc/>
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

        /// <inheritdoc/>
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


        /// <inheritdoc/>
        public List<SkillDomain> GetMasteredSkillDomains()
        {
            List<SkillDomain> Result = new();
            foreach (var b in characterM.Skills.MasteredDomains)
                if (b.Value) Result.Add(b.Key);
            return Result;
        }
        #endregion



        #region ENERGIES

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public Dictionary<string, int> EnergyValue { get; protected set; }

        /// <inheritdoc/>
        public List<EnergyDTO> GetEnergies()
        {
            List<EnergyDTO> Result = new();

            foreach (var r in characterM.Energies)
            {
                var energy = new EnergyDTO()
                {
                    Id = r.Key,
                    Name = "",
                    ShortName = "",
                    Max = r.Value.Max,
                    Min = r.Value.Min,
                    EffectiveValue = EnergyValue[r.Key],
                    CrossedThresholds = r.Value.CountCrossedThresholds(EnergyValue[r.Key])
                };
                Result.Add(energy);
            }

            return Result;
        }

        /// <inheritdoc/>
        public EnergyDTO ChangeEnergies(EnergyDTO energy)
        {
            var energyM = characterM.Energies[energy.Id];
            EnergyValue[energy.Id] = energy.EffectiveValue;
            energyM.Max = energy.Max;
            energy.CrossedThresholds = energyM.CountCrossedThresholds(energy.EffectiveValue);

            return energy;
        }

        #endregion


        #region RESILIENCES
        public List<ResilienceDTO> GetResiliences()
        {
            List<ResilienceDTO> Result = new();

            foreach (var r in characterM.Resiliences)
            {
                var resilience = new ResilienceDTO()
                {
                    Name = GameDataService.Resiliences[r.Key].Name,
                    ShortName = GameDataService.Resiliences[r.Key].ShortName,
                    Max = r.Value.Value,
                    Id = r.Key,
                    EffectiveValue = r.Value.ComputeValue(AbilityEffValues)
                };
                Result.Add(resilience);
            }

            return Result;
        }
        #endregion
    }
}
