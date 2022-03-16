using FateExplorer.GameData;
using FateExplorer.Shared;

namespace FateExplorer.CharacterModel
{
    public class WeaponUnarmedM : WeaponM
    {
        public WeaponUnarmedM(ICharacterM hero) : base(hero)
        {}

        /// <summary>
        /// Provide the weapon with all the necessary data
        /// </summary>
        /// <param name="gameData"></param>
        /// <remarks>Do not use the inherited method 
        /// <see cref="WeaponM.Initialise(WeaponDTO, IGameDataService)"/> for 
        /// unarmed combat</remarks>
        public void Initialise(IGameDataService gameData)
        {
            WeaponDTO weaponDTO = new()
            {
                Id = "Unarmed", Name = "Brawling", 
                CombatTechId = CombatTechM.Unarmed,
                AttackMod = 0, ParryMod = 0,
                DamageBonus = 0,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageThreshold = 21,
                PrimaryAbilityId = gameData.CombatTechs[CombatTechM.Unarmed].PrimeAttrID.Split('/'),
                Range = null,
                Reach = 1,
                IsRanged = false,
                IsTwohanded = false,
                IsImprovised = false,
                IsParry = false,
                Branch = CombatBranch.Unarmed
            };
            base.Initialise(weaponDTO, gameData);
        }

    }
}
