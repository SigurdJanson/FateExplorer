using FateExplorer.CharacterImport;
using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Pages;
using FateExplorer.Shared;
using FateExplorer.Shared.ClientSideStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{


    public class TheHeroViMo : ITheHeroViMo
    {
        protected IGameDataService GameDataService; // injected
        protected AppSettings AppConfig; // injected
        protected IClientSideStorage Storage; // injected storage
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
        public TheHeroViMo(IGameDataService gameData, AppSettings appConfig, IClientSideStorage storage)
        {
            GameDataService = gameData;
            AppConfig = appConfig;
            Storage = storage;
        }

        /// <inheritdoc/>
        public bool HasBorn { get; protected set; }

        /// <inheritdoc/>
        public async Task ReadCharacterFile(byte[] Data)
        {
            CharacterImportOptM characterImportOptM = JsonSerializer.Deserialize<CharacterImportOptM>(new ReadOnlySpan<byte>(Data));

            characterM = new CharacterM(GameDataService, characterImportOptM);
            await InitAttributes();
            HasBorn = true;
            NotifyStateChanged();
        }



        /// <summary>
        /// Sets the effective values after a new character was loaded.
        /// </summary>
        protected async Task InitAttributes()
        {
            if (characterM is null)
                throw new Exception("Interner Programmfehler; keine geladenen Daten.");

            // Retrieve previously saved state (if available)
            string StorageId = $"{GetType()}/{characterM?.Id}";
            HeroStorageDTO StoredItem = await Storage.Retrieve<HeroStorageDTO>(StorageId, null);

            //
            // Create character with default values
            // Set either default value or effective value from storage
            //
            // ABILITIES
            if (AbilityEffValues is null)
                AbilityEffValues = new();
            else
                AbilityEffValues.Clear();
            foreach (var chab in characterM?.Abilities)
                if (StoredItem?.Abilities?.TryGetValue(chab.Key, out int StoredValue) ?? false)
                    AbilityEffValues.Add(chab.Key, StoredValue);
                else
                    AbilityEffValues.Add(chab.Key, chab.Value.Value);

            // COMBAT
            Hands = new(characterM, GameDataService);

            // DODGE
            if (StoredItem?.DodgeTrue is not null)
                DodgeTrueValue = StoredItem.DodgeTrue;
            else
                DodgeTrueValue = characterM.Dodge.Value;

            // Add DodgeMod

            // ENERGIES
            EffEnergy = new();
            foreach (var r in characterM.Energies)
            {
                int Max2Set;
                if (StoredItem?.EffectiveMaxEnergy?.ContainsKey(r.Key) ?? false)
                    Max2Set = StoredItem.EffectiveMaxEnergy[r.Key];
                else
                    Max2Set = r.Value.Max;
                int Val2Set;
                if (StoredItem?.EffectiveEnergy?.ContainsKey(r.Key) ?? false)
                    Val2Set = StoredItem.EffectiveEnergy[r.Key];
                else
                    Val2Set = r.Value.Max;

                var energy = new EnergyViMo(r.Value, r.Key, this)
                {
                    Name = GameDataService.Energies[r.Key].Name,
                    ShortName = GameDataService.Energies[r.Key].ShortName,
                    EffMax = Max2Set,
                    EffectiveValue = Val2Set
                };
                EffEnergy.Add(energy);
            }

            // RESILIENCES
            if (ResilienceEffValues is null)
                ResilienceEffValues = new();
            else
                ResilienceEffValues.Clear();
            foreach (var chre in characterM?.Resiliences)
                if (StoredItem?.EffectiveResilience?.TryGetValue(chre.Key, out int StoredValue) ?? false)
                    ResilienceEffValues.Add(chre.Key, StoredValue);
                else
                    ResilienceEffValues.Add(chre.Key, chre.Value.ComputeValue(AbilityEffValues));

            // BELONGINGS
            EffectiveMoney = StoredItem?.EffectiveMoney ?? 0;

        }


        /// <summary>
        /// Send the current state of effective and true values to storage
        /// </summary>
        protected async void SaveAttributes()
        {
            string StorageId = $"{GetType()}/{characterM?.Id}";
            HeroStorageDTO Data2Store = new()
            {
                Abilities = this.AbilityEffValues,
                DodgeTrue = this.DodgeTrueValue,
                DodgeMod  = this.DodgeEffMod,
                EffectiveEnergy = new(),
                EffectiveMaxEnergy = new(),
                EffectiveResilience = this.ResilienceEffValues,
                EffectiveMoney = this.EffectiveMoney
            };
            foreach (var e in this.EffEnergy)
            {
                Data2Store.EffectiveEnergy.Add(e.Id, e.EffectiveValue);
                Data2Store.EffectiveMaxEnergy.Add(e.Id, e.EffMax);
            }

            await Storage.Store(StorageId, Data2Store);
        }



        public string Name { get => characterM?.Name ?? ""; }
        public string PlaceOfBirth { get => characterM?.PlaceOfBirth ?? ""; }
        public string DateOfBirth { get => characterM?.DateOfBirth ?? ""; }

        public decimal CarriedWeight { get => characterM?.CarriedWeight ?? 0; }


        #region POSESSIONS ------

        /// <summary>
        /// The effective amount of money if it deviates from the (imported) amount; otherwise <c>null</c>.
        /// </summary>
        private decimal? EffectiveMoney = null;

        /// <summary>
        /// The cash money carried by the character.
        /// </summary>
        public decimal Money
        { 
            get => EffectiveMoney is null ? characterM?.Money ?? 0 : EffectiveMoney!.Value;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                if (value == EffectiveMoney) return; // avoid any further processing if uneeded
                if (value == characterM?.Money)
                    EffectiveMoney = null;
                else
                    EffectiveMoney = value;
                SaveAttributes();
            }
        }

        /// <summary>
        /// Get a formatted string of the character's total wealth
        /// </summary>
        /// <returns>A human-readable string</returns>
        public string FormatMoney()
        {
            string Result;
            if (Money >= 100)
            {
                int Doucats = (int)Math.Floor(Money / 10);
                decimal Silvers = Money % 10;
                Result = $"{Doucats} D {Silvers:N2} S";
            }
            else
                Result = $"{Money:N2} S";
            return Result;
        }


        private decimal assetValue = -1;
        /// <summary>
        /// What is the combined value of all assets?
        /// </summary>
        public decimal AssetValue
        {
            get
            {
                if (assetValue < 0)
                {
                    assetValue = 0;
                    foreach (var i in characterM.Belongings.Values)
                        assetValue += i.Price;
                }
                return assetValue;
            }
        }


        public IEnumerable<BelongingViMo> GetBelongings()
        {
            foreach (var i in characterM.Belongings.Values.OrderBy(x => x.Name))
                yield return new BelongingViMo(i);
        }


        /// <inheritdoc/>
        public decimal WhatCanCarry { get => characterM?.WhatCanCarry(AbilityEffValues[AbilityM.STR]) ?? 0; }

        /// <inheritdoc/>
        public decimal WhatCanLift { get => characterM?.WhatCanLift(AbilityEffValues[AbilityM.STR]) ?? 0; }

        #endregion



        /// <inheritdoc/>
        public int Movement { get => characterM.Movement.Value; }


        /// <inheritdoc/>
        public int Initiative
            => characterM.GetInitiative(AbilityEffValues[AbilityM.COU], AbilityEffValues[AbilityM.AGI]);

        /// <inheritdoc/>
        public CharacterAttrDTO GetInitiative()
            => new()
            {
                Id = ChrAttrId.INI,
                Name = ResourceId.IniLabelId,
                EffectiveValue = Initiative,
                Max = 40,
                Min = 1
            };



        #region ABILITIES

        /// <summary>
        /// The effective values of the character's abilities.
        /// </summary>
        Dictionary<string, int> AbilityEffValues { get; set; } // TODO: send to storage as soon as a change of true/effective value is supported

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


        /// <inheritdoc />
        public List<SpecialAbilityDTO> GetSpecialAbilities()
        {
            List<SpecialAbilityDTO> Result = new();
            foreach (var sa in characterM?.SpecialAbilities)
            {
                string Name;
                try
                {
                    Name = GameDataService.SpecialAbilities[sa.Key].Name;
                }
                catch (Exception) { Name = "unknown";  }
                SpecialAbilityDTO item = new()
                {
                    Id = sa.Key,
                    Name = Name,
                    Tier = sa.Value.Tier
                };
                Result.Add(item);
            }

            return Result;
        }


        /// <inheritdoc />
        public List<SpecialAbilityDTO> GetCombatStyleSpecialAbilities(string CombatTecId)
        {
            List<SpecialAbilityDTO> Result = new();
            foreach (var sa in characterM?.SpecialAbilities)
            {
                // skip if special ability does not reference this combat technique
                // (in fact does not reference anything)
                if (sa.Value.Reference is null || sa.Value.Reference.Length == 0)
                    continue;
                // skip if it does not fit the given filter
                if (CombatTecId is not null && !sa.Value.Reference.Contains(CombatTecId)) 
                    continue;

                string Name;
                try
                {
                    Name = GameDataService.SpecialAbilities[sa.Key].Name;
                }
                catch (Exception) { Name = "unknown"; }
                SpecialAbilityDTO item = new()
                {
                    Id = sa.Key,
                    Name = Name,
                    Tier = sa.Value.Tier
                };
                Result.Add(item);
            }
            return Result;
        }


        /// <inheritdoc />
        public List<LanguageDTO> GetLanguages()
        {
            List<LanguageDTO> Result = new();
            if (characterM?.Languages is null) return Result;
            foreach (var sa in characterM?.Languages)
            {
                LanguageDTO item = new()
                {
                    Id = sa.Key,
                    Name = sa.Value.Language.ToString(),
                    Tier = (LanguageAbility)sa.Value.Tier,
                    Language = sa.Value.Language
                };
                Result.Add(item);
            }

            return Result;
        }


        /// <summary>
        /// Returns a list with the characters advantages
        /// </summary>
        /// <returns>List</returns>
        public List<DisAdvantageDTO> GetAdvantages()
        {
            List<DisAdvantageDTO> Result = new();
            foreach (var adv in characterM?.Advantages)
            {
                string Name;
                try
                {
                    Name = GameDataService.DisAdvantages[adv.Key].Name;
                }
                catch (Exception) { Name = "unknown"; }
                DisAdvantageDTO item = new()
                {
                    Id = adv.Key,
                    Name = Name,
                    Tier = adv.Value.Tier
                };
                Result.Add(item);
            }

            return Result;
        }


        /// <summary>
        /// Returns a list with the characters disadvantages
        /// </summary>
        /// <returns>List</returns>
        public List<DisAdvantageDTO> GetDisadvantages()
        {
            List<DisAdvantageDTO> Result = new();
            foreach (var adv in characterM?.Disadvantages)
            {
                string Name;
                try
                {
                    Name = GameDataService.DisAdvantages[adv.Key].Name;
                }
                catch (Exception) { Name = "unknown"; }
                DisAdvantageDTO item = new()
                {
                    Id = adv.Key,
                    Name = Name,
                    Tier = adv.Value.Tier
                };
                Result.Add(item);
            }

            return Result;
        }



        #region SKILLS


        /// <inheritdoc/>
        public List<SkillsDTO> GetSkills(Check.Skill? Domain = null, string NameFilter = "")
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
                    EffectiveValue = s.Value.Value,
                    Max = s.Value.Value,
                    Modifications = s.Value.Modifications,
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
            string[] Favs = AppConfig.MostUsedSkills.ToArray();

            List<SkillsDTO> Result = new();

            foreach (var fav in Favs)
            {
                var s = characterM.Skills.Skills[fav];
                var skill = new SkillsDTO
                {
                    Id = fav,
                    Name = s.Name,
                    Max = s.Value,
                    EffectiveValue = s.Value,
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
                    EffectiveValue = characterM.Skills.Skills[Key].Value,
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
                        EffectiveValue = characterM.Skills.Skills[Key].Value,
                        Min = 0,
                        Domain = characterM.Skills.Skills[Key].Domain
                    });
                    i++;
                }
            }

            return Result;
        }


        /// <inheritdoc/>
        public List<Check.Skill> GetMasteredSkillDomains()
        {
            List<Check.Skill> Result = new();
            foreach (var b in characterM.Skills.MasteredDomains)
                if (b.Value) Result.Add(b.Key);
            return Result;
        }


        /// <inheritdoc />
        public AbilityDTO[] GetSkillAbilities(string skillId)
        {
            AbilityDTO[] Result = new AbilityDTO[3];
            var s = characterM.Skills.Skills[skillId];

            int Index = 0;
            foreach (var AbilityId in s.Abilities)
            {
                var Ability = characterM?.Abilities[AbilityId];
                Result[Index++] = new AbilityDTO()
                {
                    Id = Ability.Id,
                    Name = Ability.Name,
                    ShortName = Ability.ShortName,
                    EffectiveValue = AbilityEffValues[Ability.Id],
                    Max = Ability.Value,
                    Min = 0
                };
            }
            return Result;
        }

        #endregion



        #region COMBAT & DODGE

        public HandsViMo Hands { get; protected set; }



        /// <summary>
        /// A dodge value set by the user overwrites the value from the character sheet
        /// </summary>
        int DodgeTrueValue { get; set; } // TODO: send to storage as soon as a change of true/effective value is supported

        /// <summary>
        /// A temporary modifier of the dodge value
        /// </summary>
        int DodgeEffMod { get; set; } // TODO: send to storage as soon as a change of true/effective value is supported

        /// <inheritdoc />
        public CharacterAttrDTO GetDodge()
        {
            int DodgeVal;
            var dodgeM = characterM.Dodge;
            var Dependencies = dodgeM.DependentAttributes;
            if (Dependencies == null) 
                DodgeVal = DodgeTrueValue;
            else
            {
                // TODO: Make this logic flexible so that it can apply to all calculated values
                bool Same = true;
                foreach (var d in Dependencies)
                    if (characterM.GetAbility(d) != AbilityEffValues[d])
                        Same = false;

                if (Same)
                    DodgeVal = DodgeTrueValue + DodgeEffMod;
                else
                    DodgeVal = DodgeM.ComputeDodge(characterM.GetAbility(Dependencies[0])) + DodgeEffMod; // TODO: this is wrong because the true value is completely being ignored, here
            }

            return new CharacterAttrDTO()
            {
                Id = ChrAttrId.DO,
                Name = "Dodge"/*TODO: Magic string, no l10n*/, 
                EffectiveValue = DodgeVal, Max = dodgeM.Max, Min = dodgeM.Min
            };
        }

        #endregion



        #region ENERGIES

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public List<EnergyViMo> EffEnergy { get; protected set; }


        /// <inheritdoc/>
        public List<EnergyViMo> GetEnergies()
        {
            return EffEnergy;
        }

        /// <inheritdoc/>
        public EnergyViMo OnEnergyChanged(EnergyViMo energy)
        {
            NotifyStateChanged();
            SaveAttributes(); // store new values in client-side storage
            return energy;
        }

        #endregion


        #region RESILIENCES

        /// <summary>
        /// Effective points of this energy
        /// </summary>
        public Dictionary<string, int> ResilienceEffValues { get; protected set; } // TODO: send to storage as soon as a change of true/effective value is supported

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




        #region WEAPONS

        List<WeaponViMo> weapons;
        public List<WeaponViMo> Weapons
        {
            get
            {
                if (weapons is null)
                {
                    weapons = new List<WeaponViMo>();
                    foreach (var w in characterM.Weapons)
                        weapons.Add(new WeaponViMo(w.Value));
                }
                return weapons;
            }
        }


        #endregion
    }
}
