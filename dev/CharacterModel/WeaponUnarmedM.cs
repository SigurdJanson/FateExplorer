using FateExplorer.GameData;
using FateExplorer.Shared;
using System.Diagnostics.CodeAnalysis;

namespace FateExplorer.CharacterModel
{
    public class WeaponUnarmedM : WeaponM
    {
        [SetsRequiredMembers]
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
            var Unarmed = gameData.WeaponsMelee["WEAPONLESS"];
            WeaponDTO weaponDTO = new()
            {
                Id = Unarmed.Id, Name = Unarmed.Name, 
                CombatTechId = Unarmed.CombatTechID,
                AttackMod = Unarmed.AtMod, ParryMod = Unarmed.PaMod,
                DamageBonus = Unarmed.Bonus,
                DamageDieCount = Unarmed.DamageDieCount(),
                DamageDieSides = Unarmed.DamageDieSides(),
                DamageThreshold = Unarmed.Threshold,
                PrimaryAbilityId = gameData.CombatTechs[CombatTechM.Unarmed].PrimeAttrID.Split('/'),
                Range = null,
                Reach = Unarmed.Reach,
                IsRanged = !Unarmed.CloseRange,
                IsTwohanded = Unarmed.TwoHanded,
                IsImprovised = Unarmed.Improvised,
                IsParry = Unarmed.Parry,
                Branch = gameData.CombatTechs[CombatTechM.Unarmed].WeaponsBranch
            };
            base.Initialise(weaponDTO, gameData);
        }

    }
}
