using FateExplorer.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.GameData
{
    public interface IGameDataService
    {
        public AbilitiesDB Abilities { get; }

        public CombatTechDB CombatTechs { get; }

        public WeaponMeleeDB WeaponsMelee { get; }

        public WeaponRangedDB WeaponsRanged { get; }

        public SkillsDB Skills { get; }

        public ArcaneSkillsDB ArcaneSkills { get; }

        public KarmaSkillsDB KarmaSkills { get; }

        public ResiliencesDB Resiliences { get; }

        public Task InitializeGameDataAsync();
    }
}
