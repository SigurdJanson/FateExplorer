using FateExplorer.CharacterData;
using FateExplorer.GameData;
using FateExplorer.GameLogic;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FateExplorer.ViewModel
{
    public interface ICharacterAttributDTO
    {
        /// <summary>
        /// Unique identifier fot the character attribute
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Name of the attribute
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The character's attribute value
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// The permitted minimum value.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The character's attribute value after adding/removing temporary changes.
        /// Reductions happen during gameplay and are defined by the game master.
        /// Calculated attributes may change due to dependencies.
        /// </summary>
        public int EffectiveValue { get; set; }
    }

    public struct AbilityDTO : ICharacterAttributDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }
    }

    public struct SkillsDTO : ICharacterAttributDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }

        /// <summary>
        /// Specifies what domain the skill is from (mundane, arcane or divine).
        /// </summary>
        public SkillDomain Domain;
    }

    public struct EnergyDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <summary>
        /// Allows users to set their character's energy themselves
        /// </summary>
        public int EffMax { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }

        /// <summary>
        /// Some energies have a number of consequences when reduced 
        /// by certain amounts i.e. crosing thresholds.
        /// </summary>
        public int CrossedThresholds;
    }

    public struct ResilienceDTO
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <summary>
        /// An abbreviation of the name (e.g. COU for courage as common in roleplay systems).
        /// </summary>
        public string ShortName { get; set; }

        /// <inheritdoc />
        public int Max { get; set; }

        /// <inheritdoc />
        public int Min { get; set; }

        /// <inheritdoc />
        public int EffectiveValue { get; set; }
    }



    public class TheHeroViMo : ITheHeroViMo
    {
        protected IGameDataService GameDataService; // injected
        protected IConfiguration AppConfig; // injected
        protected ICharacterM characterM;

        /// <summary>
        /// 
        /// </summary>
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();  // this method hides the OnChange to simplify it



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameData"></param>
        public TheHeroViMo(IGameDataService gameData, IConfiguration appConfig)
        {
            GameDataService = gameData;
            AppConfig = appConfig;
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
            NotifyStateChanged();
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

            // ENERGIES
            if (EnergyEffValues is null)
                EnergyEffValues = new();
            else
                EnergyEffValues.Clear();
            foreach (var chen in characterM?.Energies)
                EnergyEffValues.Add(chen.Key, chen.Value.Max);

            if (EnergyEffMax is null)
                EnergyEffMax = new();
            else
                EnergyEffMax.Clear();
            foreach (var chen in characterM?.Energies)
                EnergyEffMax.Add(chen.Key, chen.Value.Max);

            // RESILIENCES
            if (ResilienceEffValues is null)
                ResilienceEffValues = new();
            else
                ResilienceEffValues.Clear();
            foreach (var chre in characterM?.Resiliences)
                ResilienceEffValues.Add(chre.Key, chre.Value.ComputeValue(AbilityEffValues));
        }

        public string Name { get => characterM?.Name ?? ""; }
        public string PlaceOfBirth { get => characterM?.PlaceOfBirth ?? ""; }
        public string DateOfBirth { get => characterM?.DateOfBirth ?? ""; }


        #region ABILITIES
        Dictionary<string, int> AbilityEffValues { get; set; }

        public List<AbilityDTO> GetAbilites()
        {
            var Result = new List<AbilityDTO>();

            foreach (var chab in characterM?.Abilities)
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

        // TODO: implement effective values

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
                    Min = 0,
                    EffectiveValue = s.Value.Value, // TODO: effective skill value
                    Max = s.Value.Value,
                    Domain = s.Value.Domain
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
            string[] Favs = AppConfig.GetSection("MostUsedSkills").Get<string[]>();

            List<SkillsDTO> Result = new();

            foreach (var fav in Favs)
            {
                var s = characterM.Skills.Skills[fav];
                var skill = new SkillsDTO
                {
                    Id = fav,
                    Name = s.Name,
                    Max = s.Value,
                    EffectiveValue = s.Value, // TODO: effective skill value
                    Min = 0,
                    Domain = s.Domain
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
            for (i = 0; i < Count; i++)
            {
                string Key = SkillByValue[i].Id;
                Result.Add(new SkillsDTO()
                {
                    Id = Key,
                    Name = characterM.Skills.Skills[Key].Name,
                    Max = characterM.Skills.Skills[Key].Value,
                    EffectiveValue = characterM.Skills.Skills[Key].Value, // TODO: effective skill value
                    Min = 0,
                    Domain = characterM.Skills.Skills[Key].Domain
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
                        Max = characterM.Skills.Skills[Key].Value,
                        EffectiveValue = characterM.Skills.Skills[Key].Value, // TODO: effective skill value
                        Min = 0,
                        Domain = characterM.Skills.Skills[Key].Domain
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


        public List<AbilityDTO> GetSkillAbilities(SkillsDTO skill)
        {
            List<AbilityDTO> Result = new();
            var s = characterM.Skills.Skills[skill.Id];
            foreach (var AbilityId in s.Abilities)
            {
                var Ability = characterM?.Abilities[AbilityId];
                Result.Add(new AbilityDTO()
                {
                    Id = Ability.Id,
                    Name = Ability.Name,
                    ShortName = Ability.ShortName,
                    EffectiveValue = AbilityEffValues[Ability.Id],
                    Max = Ability.Value,
                    Min = 0
                });
            }
            return Result;
        }

        #endregion



        #region ENERGIES

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public Dictionary<string, int> EnergyEffValues { get; protected set; }

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public Dictionary<string, int> EnergyEffMax { get; protected set; }

        /// <inheritdoc/>
        public List<EnergyDTO> GetEnergies()
        {
            List<EnergyDTO> Result = new();

            foreach (var r in characterM.Energies)
            {
                var energy = new EnergyDTO()
                {
                    Id = r.Key,
                    Name = GameDataService.Energies[r.Key].Name,
                    ShortName = GameDataService.Energies[r.Key].ShortName,
                    Max = r.Value.Max,
                    EffMax = EnergyEffMax[r.Key],
                    Min = r.Value.Min,
                    EffectiveValue = EnergyEffValues[r.Key],
                    CrossedThresholds = r.Value.CountCrossedThresholds(EnergyEffValues[r.Key])
                };
                Result.Add(energy);
            }

            return Result;
        }

        /// <inheritdoc/>
        public EnergyDTO ChangeEnergies(EnergyDTO energy)
        {
            var energyM = characterM.Energies[energy.Id];
            EnergyEffValues[energy.Id] = energy.EffectiveValue;
            EnergyEffMax[energy.Id] = energy.EffMax;
            energyM.Max = energy.Max;
            energy.CrossedThresholds = energyM.CountCrossedThresholds(energy.EffectiveValue);

            NotifyStateChanged();
            return energy;
        }

        #endregion


        #region RESILIENCES

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public Dictionary<string, int> ResilienceEffValues { get; protected set; }

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
