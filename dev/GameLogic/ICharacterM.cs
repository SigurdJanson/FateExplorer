using System.Collections.Generic;

namespace FateExplorer.GameLogic
{
    public interface ICharacterM
    {
        string Name { get; }

        string PlaceOfBirth { get; }

        string DateOfBirth { get; }

        Dictionary<string, AbilityM> Abilities { get; }

        Dictionary<string, CharacterEnergyM> Energies { get; }

        Dictionary<string, ResilienceM> Resiliences { get; }

        Dictionary<string, CombatTechM> CombatTechs { get; }

        CharacterSkillsM Skills { get; }


        int GetAbility(string Id);

    }
}