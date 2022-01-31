using System.Threading.Tasks;

namespace FateExplorer.GameData
{
    public interface IGameDataService
    {
        public AbilitiesDB Abilities { get; }


        public BotchEntry GetSkillBotch(SkillDomain domain, int DiceEyes);

        public BotchEntry GetAttackBotch(CombatBranch technique, int DiceEyes);

        public BotchEntry GetParryBotch(CombatBranch technique, int DiceEyes);


        public CombatTechDB CombatTechs { get; }

        public WeaponMeleeDB WeaponsMelee { get; }

        public WeaponRangedDB WeaponsRanged { get; }

        public SkillsDB Skills { get; }

        public ArcaneSkillsDB ArcaneSkills { get; }

        public KarmaSkillsDB KarmaSkills { get; }

        public ResiliencesDB Resiliences { get; }

        public EnergiesDB Energies { get; }

        public Task InitializeGameDataAsync();
    }
}
