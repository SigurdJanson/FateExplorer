using FateExplorer.CharacterImport;
using FateExplorer.GameData;
using System.Collections.Generic;



namespace FateExplorer.CharacterModel
{
    /// <summary>
    /// Holds the strings to identify special abilities
    /// </summary>
    public static class SA
    {
        public const string Writing = "SA_27";
        public const string Language = "SA_29";
        public const string TwoHandedCombat = "SA_42";
    }

    public static class ADV
    {
        public const string Ambidexterous = "ADV_5";
    }


    /// <summary>
    /// 
    /// </summary>
    public class CharacterM : ICharacterM
    {

        /// <summary>
        /// Constructor using an Optoloith import
        /// </summary>
        /// <param name="gameData">Access to the data bases describing basic DSA5</param>
        /// <param name="characterImportOptM">Importer</param>
        public CharacterM(IGameDataService gameData, CharacterImportOptM characterImportOptM)
        {
            Name = characterImportOptM.GetName();
            PlaceOfBirth = characterImportOptM.GetPlaceOfBirth();
            DateOfBirth = characterImportOptM.GetDateOfBirth();
            SpeciesId = characterImportOptM.GetSpeciesId();

            // ABILITIES
            Abilities = new();
            foreach (var AbImport in characterImportOptM.GetAbilities())
            {
                string AbilityName = gameData.Abilities[AbImport.Key].Name;
                string AbilityShortName = gameData.Abilities[AbImport.Key].ShortName;
                AbilityM ab = new(AbImport.Key, AbilityName, AbilityShortName, AbImport.Value);
                Abilities.Add(AbImport.Key, ab);
            }

            // SPECIAL ABILITIES
            SpecialAbilities = characterImportOptM.GetSpecialAbilities();
            Languages = characterImportOptM.GetLanguages();

            // DIS-ADVANTAGES
            Advantages = characterImportOptM.GetAdvantages();
            Disadvantages = characterImportOptM.GetDisadvantages();

            // SKILLS
            Skills = new(this, characterImportOptM, gameData);

            // COMBAT TECHNIQUES
            CombatTechs = new();
            foreach (var CtImport in characterImportOptM.GetCombatSkills())
            {
                CombatTechM ct = new(gameData.CombatTechs[CtImport.Key], CtImport.Value, this);
                CombatTechs.Add(CtImport.Key, ct);
            }
            foreach (var ct in gameData.CombatTechs.Data)
            {
                if (!CombatTechs.ContainsKey(ct.Id))
                {
                    CombatTechM newCt = new(ct, CombatTechM.DefaultSkillValue, this);
                    CombatTechs.Add(ct.Id, newCt);
                }
            }

            // DODGE
            Dodge = new DodgeM(this);

            // ENERGIES
            Energies = new();
            foreach (var energy in gameData.Energies.Data)
            {
                CharacterEnergyM energyM = null;
                CharacterEnergyClass _Class;
                int ExtraEnergy;
                switch (energy.Id)
                {
                    case "LP":
                        _Class = CharacterEnergyClass.LP;
                        ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                        energyM = new CharacterHealth(energy, _Class, ExtraEnergy, this);
                        break;
                    case "AE":
                        if (!characterImportOptM.IsSpellcaster()) break;
                        _Class = CharacterEnergyClass.AE;
                        ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                        energyM = new CharacterAstralEnergy(energy, _Class, ExtraEnergy, this);
                        break;
                    case "KP":
                        if (!characterImportOptM.IsBlessed()) break;
                        _Class = CharacterEnergyClass.KP;
                        ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                        energyM = new CharacterKarma(energy, _Class, ExtraEnergy, this);
                        break;
                }
                if (energyM is not null)
                    Energies.Add(energy.Id, energyM);
            }

            // RESILIENCES
            Resiliences = new();
            foreach (var Res in gameData.Resiliences.Data)
            {
                Resiliences.Add(Res.Id, new ResilienceM(Res, this));
            }

            // BELONGINGS
            CarriedWeight = characterImportOptM.TotalWeightOfBelongings();
            Money = characterImportOptM.TotalMoney();

            Weapons = new Dictionary<string, WeaponM>();
            foreach (var w in characterImportOptM.GetWeaponsDetails(gameData.WeaponsMelee, gameData.WeaponsRanged, gameData.CombatTechs))
            {
                WeaponM weaponM = new (this);
                weaponM.Initialise(w, gameData);
                Weapons.Add(w.Id, weaponM);
            }
        }



        public string Name { get; protected set; }

        public string PlaceOfBirth { get; protected set; }

        public string DateOfBirth { get; protected set; }

        /// <inheritdoc />
        public string SpeciesId { get; protected set; }

        /// <inheritdoc />
        public double CarriedWeight { get; protected set; }

        /// <inheritdoc />
        public double Money { get; protected set; }

        /// <inheritdoc />
        public double WhatCanCarry(int EffectiveStrength)
        {
            return EffectiveStrength * 2;
        }

        /// <inheritdoc />
        public double WhatCanLift(int EffectiveStrength)
        {
            return EffectiveStrength * 10;
        }


        /// <inheritdoc />
        public int GetInitiative(int courage, int agility)
        {
            return (courage + agility + 1) / 2;
        }

        /// <inheritdoc />
        public int Initiative
            => GetInitiative(Abilities[AbilityM.COU].Value, Abilities[AbilityM.AGI].Value);


        public Dictionary<string, WeaponM> Weapons { get; protected set; }


        public Dictionary<string, AbilityM> Abilities { get; set; }

        public int GetAbility(string Id) => Abilities[Id].Value;

        public Dictionary<string, IActivatableM> SpecialAbilities { get; protected set; }

        public bool HasSpecialAbility(string Id) => SpecialAbilities?.ContainsKey(Id) ?? false;

        public Dictionary<string, LanguageM> Languages { get; protected set; }


        public Dictionary<string, IActivatableM> Advantages { get; protected set; }
        public bool HasAdvantage(string Id) => Advantages?.ContainsKey(Id) ?? false;

        public Dictionary<string, IActivatableM> Disadvantages { get; protected set; }
        public bool HasDisadvantage(string Id) => Disadvantages?.ContainsKey(Id) ?? false;


        public Dictionary<string, CharacterEnergyM> Energies { get; set; }

        public Dictionary<string, ResilienceM> Resiliences { get; set; }


        public Dictionary<string, CombatTechM> CombatTechs { get; set; }

        public DodgeM Dodge { get; set; }

        public CharacterSkillsM Skills { get; set; }
    }
}
