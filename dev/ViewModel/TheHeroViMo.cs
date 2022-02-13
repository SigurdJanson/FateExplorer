using FateExplorer.CharacterData;
using FateExplorer.GameData;
using FateExplorer.CharacterModel;
using FateExplorer.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FateExplorer.ViewModel
{


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
            CharacterImportOptM characterImportOptM = JsonSerializer.Deserialize<CharacterImportOptM>(new ReadOnlySpan<byte>(Data));

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

            // COMBAT
            Hands = new(characterM, GameDataService);

            // DODGE
            DodgeTrueValue = characterM.Dodge.Value;

            // ENERGIES
            EffEnergy = new();
            foreach (var r in characterM.Energies)
            {
                var energy = new EnergyViMo(r.Value, r.Key, this)
                {
                    Name = GameDataService.Energies[r.Key].Name,
                    ShortName = GameDataService.Energies[r.Key].ShortName,
                    EffMax = r.Value.Max,
                    EffectiveValue = r.Value.Max
                };
                EffEnergy.Add(energy);
            }

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

        public double CarriedWeight { get => characterM?.CarriedWeight ?? 0; }

        public double WhatCanCarry { get => characterM?.WhatCanCarry(AbilityEffValues[AbilityM.STR]) ?? 0; }

        public double WhatCanLift { get => characterM?.WhatCanLift(AbilityEffValues[AbilityM.STR]) ?? 0; }


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



        #region COMBAT & DODGE

        public HandsViMo Hands { get; protected set; }

        //protected WeaponViMo dominantHandWeapon;
        //protected WeaponViMo nondominantHandWeapon;

        ///// <summary>
        ///// Create a "bare hands" weapon for unarmed combat
        ///// </summary>
        ///// <returns>A weapon</returns>
        //protected WeaponViMo GetBareHandsAsWeapon()
        //{
        //    WeaponUnarmedM weapon = new(characterM);
        //    weapon.Initialise(GameDataService);
        //    return new WeaponViMo(weapon);
        //}

        ///// <inheritdoc/>
        //public WeaponViMo DominantHandWeapon 
        //{
        //    get 
        //    { 
        //        if (dominantHandWeapon is null)
        //            dominantHandWeapon = GetBareHandsAsWeapon();
        //        return dominantHandWeapon; 
        //    }
        //    set { dominantHandWeapon = value ?? GetBareHandsAsWeapon(); }
        //}


        ///// <inheritdoc/>
        //public WeaponViMo NondominantHandWeapon
        //{ 
        //    get 
        //    {
        //        if (nondominantHandWeapon is null)
        //            nondominantHandWeapon = GetBareHandsAsWeapon();
        //        return nondominantHandWeapon; 
        //    }
        //    set { nondominantHandWeapon = value ?? GetBareHandsAsWeapon(); }
        //}


        ///// <summary>
        ///// Empties the characters hand by removing the current weapon and "replacing" it with 
        ///// bare hands.
        ///// </summary>
        ///// <param name="Dominant">Remove the weapon from which hand? 
        ///// true is the dominant hand; false the non-domoinant.</param>
        //public void RemoveWeapon(bool Dominant)
        //{
        //    if (Dominant)
        //        DominantHandWeapon = null;
        //    else
        //        NondominantHandWeapon = null;
        //}

        ///// <summary>
        ///// Tells if the character's hands are empty and s/he would be fighting with bare hands.
        ///// </summary>
        ///// <param name="Dominant"></param>
        ///// <returns></returns>
        //public bool IsBareHand(bool Dominant)
        //{
        //    if (Dominant) //TODO: comparison is always false
        //        return DominantHandWeapon.GetType() == typeof(WeaponUnarmedM);
        //    else
        //        return NondominantHandWeapon.GetType() == typeof(WeaponUnarmedM);
        //}




        /// <summary>
        /// A basic dodge value set by the user overwrites the value from the charcter sheet
        /// </summary>
        int DodgeTrueValue { get; set; }

        /// <summary>
        /// A temporary modifier of the dodge value
        /// </summary>
        int DodgeEffMod { get; set; }

        /// <inheritdoc />
        public DodgeDTO GetDodge()
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
                    DodgeVal = DodgeM.ComputeDodge(characterM.GetAbility(Dependencies[0])) + DodgeEffMod;
            }

            return new DodgeDTO()
            {
                Id = "DO"/*TODO*/,
                Name = "Dodge"/*TODO*/, 
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
