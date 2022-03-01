using System.Threading.Tasks;

namespace FateExplorer.GameData
{
    public interface IGameDataService
    {
        public AbilitiesDB Abilities { get; }

        public SpecialAbilityDB SpecialAbilities { get; }

        DisAdvantagesDB DisAdvantages { get; }

        public BotchEntry GetSkillBotch(SkillDomain domain, int DiceEyes);

        public BotchEntry GetAttackBotch(CombatBranch technique, int DiceEyes);

        public BotchEntry GetParryBotch(CombatBranch technique, int DiceEyes);

        public BotchEntry GetDodgeBotch(CombatBranch technique, int DiceEyes);


        public CombatTechDB CombatTechs { get; }

        /// <summary>
        /// Check if the item with the given id is a weapon.
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        bool IsWeapon(string TemplateId);

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
