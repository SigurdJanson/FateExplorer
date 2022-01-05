﻿using FateExplorer.CharacterData;
using FateExplorer.GameData;
using System.Collections.Generic;



namespace FateExplorer.GameLogic
{
    public class CharacterM : ICharacterM
    {
        public CharacterM(IGameDataService gameData, CharacterImportOptM characterImportOptM)
        {
            Name = characterImportOptM.GetName();
            PlaceOfBirth = characterImportOptM.GetPlaceOfBirth();
            DateOfBirth = characterImportOptM.GetDateOfBirth();

            // ABILITIES
            Abilities = new();
            foreach(var AbImport in characterImportOptM.GetAbilities())
            {
                string AbilityName = gameData.Abilities[AbImport.Key].Name;
                string AbilityShortName = gameData.Abilities[AbImport.Key].ShortName;
                AbilityM ab = new(AbImport.Key, AbilityName, AbilityShortName, AbImport.Value);
                Abilities.Add(AbImport.Key, ab);
            }

            // SKILLS
            Skills = new(this, characterImportOptM, gameData);

            // ENERGIES
            Energies = new();
            foreach(var energy in gameData.Energies.Data)
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
                        energyM = new CharacterEnergyM(energy, _Class, ExtraEnergy, this);
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
        }



        public string Name { get; protected set; }

        public string PlaceOfBirth { get; protected set; }

        public string DateOfBirth { get; protected set; }


        public Dictionary<string, AbilityM> Abilities { get; set; }

        public Dictionary<string, CharacterEnergyM> Energies { get; set; }

        public Dictionary<string, ResilienceM> Resiliences { get; set; }

        public Dictionary<string, CombatTechM> CombatTechs{ get; set; }

        public CharacterSkillsM Skills { get; set; }

        public int GetAbility(string Id) => Abilities[Id].Value;
    }
}
