using FateExplorer.Shared;

namespace FateExplorer.CharacterModel;

public interface IWeaponM
{
    int AttackMod { get; set; }
    int BaseAtSkill { get; }
    int BasePaSkill { get; }
    CombatBranch Branch { get; }
    bool CanParry { get; }
    string CombatTechId { get; set; }
    int DamageBonus { get; set; }
    int DamageDieCount { get; set; }
    int DamageDieSides { get; set; }
    int DamageThreshold { get; set; }
    bool IsImprovised { get; set; }
    bool IsParry { get; set; }
    bool IsRanged { get; }
    bool IsTwoHanded { get; set; }
    int LoadTime { get; set; }
    string Name { get; set; }
    int ParryMod { get; set; }
    string[] PrimaryAbilityId { get; }
    int[] Range { get; set; }
    WeaponsReach Reach { get; set; }
    ShieldSize Shield { get; set; }

    int AtSkill(bool MainHand, CombatBranch otherHand);
    int PaSkill(bool MainHand, CombatBranch otherHand, bool otherIsParry, int otherPaMod);
}