using FateExplorer.CharacterImport;
using FateExplorer.CharacterModel.DisAdvantages;
using FateExplorer.CharacterModel.SpecialAbilities;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;



namespace FateExplorer.CharacterModel
{

    /// <summary>
    /// 
    /// </summary>
    public class CharacterM : ICharacterM
    {

        /// <summary>
        /// Constructor using an Optolith import
        /// </summary>
        /// <param name="gameData">Access to the data bases describing basic DSA5</param>
        /// <param name="characterImportOptM">Importer</param>
        public CharacterM(IGameDataService gameData, ICharacterImporter characterImportOptM)
        {
            try
            {
                Id = characterImportOptM.GetIdentifier();
                Name = characterImportOptM.GetName();
                PlaceOfBirth = characterImportOptM.GetPlaceOfBirth();
                DateOfBirth = characterImportOptM.GetDateOfBirth();
                SpeciesId = characterImportOptM.GetSpeciesId();
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Specification); }

            // ABILITIES
            try
            {
                Abilities = new();
                foreach (var AbImport in characterImportOptM.GetAbilities())
                {
                    string AbilityName = gameData.Abilities[AbImport.Key].Name;
                    string AbilityShortName = gameData.Abilities[AbImport.Key].ShortName;
                    AbilityM ab = new(AbImport.Key, AbilityName, AbilityShortName, AbImport.Value);
                    Abilities.Add(AbImport.Key, ab);
                }
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Attribute); }


            // SPECIAL ABILITIES
            try
            {
                SpecialAbilities = new SpecialAbilityConverter(gameData, characterImportOptM).GetAbilities();
                Languages = characterImportOptM.GetLanguages();
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.SpecialAbility); }


            // DIS-ADVANTAGES
            try
            {
                var DisAdv = new DisAdvantagesConverter(gameData, characterImportOptM).GetDisAdvantages();
                Advantages = DisAdv.Item1;
                Disadvantages = DisAdv.Item2;
                //Advantages = characterImportOptM.GetAdvantages();
                //Disadvantages = characterImportOptM.GetDisadvantages();
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.DisAdvantage); }


            // SKILLS
            try
            {
                Skills = new(this, characterImportOptM, gameData);
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Skills); }


            // COMBAT TECHNIQUES
            try
            {
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
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.CombatTechnique); }


            // DODGE
            try
            {
                Dodge = new DodgeM(this)
                {
                    Name = ResourceId.DodgeLabelId // TODO #125: this is a crutch. It should be the already translated string.
                };
            }
            catch (Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Attribute); }


            // ENERGIES
            try
            {
                Energies = new();
                foreach (var energy in gameData.Energies.Data)
                {
                    CharacterEnergyM energyM = null;
                    CharacterEnergyClass _Class;
                    int ExtraEnergy;
                    switch (energy.Id)
                    {
                        case ChrAttrId.LP:
                            _Class = CharacterEnergyClass.LP;
                            ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                            energyM = new CharacterHealth(energy, _Class, ExtraEnergy, this);
                            break;
                        case ChrAttrId.AE:
                            if (!characterImportOptM.IsSpellcaster()) break;
                            _Class = CharacterEnergyClass.AE;
                            ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                            energyM = new CharacterAstralEnergy(energy, _Class, ExtraEnergy, this);
                            break;
                        case ChrAttrId.KP:
                            if (!characterImportOptM.IsBlessed()) break;
                            _Class = CharacterEnergyClass.KP;
                            ExtraEnergy = characterImportOptM.GetAddedEnergy(_Class);
                            energyM = new CharacterKarma(energy, _Class, ExtraEnergy, this);
                            break;
                    }
                    if (energyM is not null)
                        Energies.Add(energy.Id, energyM);
                }
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Energy); }


            // RESILIENCES
            try
            {
                Resiliences = new();
                foreach (var Res in gameData.Resiliences.Data)
                {
                    Resiliences.Add(Res.Id, new ResilienceM(Res, this));
                }
            }
            catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.Attribute); }


            // MOVEMENT
            Movement = new MovementM(characterImportOptM.GetMovementBaseVal(), this);


            // BELONGINGS
            try
            {
                CarriedWeight = characterImportOptM.TotalWeightOfBelongings();
                Money = characterImportOptM.TotalMoney();

                Weapons = new Dictionary<string, WeaponM>();
                foreach (var w in characterImportOptM.GetWeaponsDetails(gameData.WeaponsMelee, gameData.WeaponsRanged, gameData.CombatTechs))
                {
                    WeaponM weaponM = new(this);
                    weaponM.Initialise(w, gameData);
                    Weapons.Add(w.Id, weaponM);
                }
            }
            catch (System.Exception e) { throw new ChrImportException("In the weapons", e, ChrImportException.Property.Belonging); }

            try
            {
                Belongings = new Dictionary<string, BelongingM>();
                foreach (var i in characterImportOptM.GetBelongings())
                {
                    Belongings.Add(i.Value.Id, i.Value);
                }
            }
            catch (System.Exception e) { throw new ChrImportException("In other belongings", e, ChrImportException.Property.Belonging); }

            // Apply all activatables that affect character attributes, like nimble -> MOV+1, etc.
            foreach (var sa in SpecialAbilities.Values)
                sa.Apply(this);
            foreach (var adv in Advantages.Values)
                adv.Apply(this);
            foreach (var disadv in Disadvantages.Values)
                disadv.Apply(this);
        }


        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string Name { get; protected set; }

        /// <inheritdoc />
        public string PlaceOfBirth { get; protected set; }

        /// <inheritdoc />
        public string DateOfBirth { get; protected set; }

        /// <inheritdoc />
        public string SpeciesId { get; protected set; }

        /// <inheritdoc />
        public decimal CarriedWeight { get; protected set; }

        /// <inheritdoc />
        public decimal Money { get; protected set; }

        /// <inheritdoc />
        public decimal WhatCanCarry(int EffectiveStrength)
        {
            return EffectiveStrength * 2;
        }

        /// <inheritdoc />
        public decimal WhatCanLift(int EffectiveStrength)
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

        /// <inheritdoc />
        public MovementM Movement { get; }


        /// <inheritdoc />
        public int WoundThreshold => (Abilities[AbilityM.CON].Value + 1) / 2;

        public Dictionary<string, WeaponM> Weapons { get; protected set; }
        public Dictionary<string, BelongingM> Belongings{ get; protected set; }


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
