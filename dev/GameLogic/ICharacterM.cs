using System.Collections.Generic;

namespace FateExplorer.GameLogic
{
    public interface ICharacterM
    {
        Dictionary<string, AbilityM> Abilities { get; }

        Dictionary<string, CharacterResourceM> Resources { get; }

        Dictionary<string, ResilienceM> Resiliences { get; }

        Dictionary<string, CombatTechM> CombatTechs { get; }

        CharacterSkillsM Skills { get; }


        int GetAbility(string Id);

        int GetEffectiveAbility(string Id);

        int GetEffectiveResilience(string Id);
    }
}