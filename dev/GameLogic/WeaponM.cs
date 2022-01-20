using FateExplorer.RollLogic;
using System;
using System.Collections.Generic;

namespace FateExplorer.GameLogic
{
    public class WeaponM
    {
        protected const int DefaultSkillValue = 6;

        protected ICharacterM Hero { get; set; }




        public string[] PrimaryAbilityId { get; protected set; }

        public int AtSkill { get; protected set; }

        public int PaSkill { get; protected set; }




        public string Name { get; protected set; }

        public string CombatTechId { get; protected set; }

        public int DamageThreshold { get; protected set; }

        public int DamageDieCount { get; protected set; }

        public int DamageDieSides { get; protected set; }

        public int DamageBonus { get; protected set; }

        public int AttackMod { get; protected set; }

        public int ParryMod { get; protected set; }

        public int Range { get; protected set; }

        public int Weight { get; protected set; }

        public bool Improvised { get; protected set; }

        public bool Ranged { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hero"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WeaponM(ICharacterM hero)
        {
            Hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }


        /// <summary>
        /// 
        /// </summary>
        public void Initialise()
        {
            CombatTechM combatTech;
            if (Hero.CombatTechs.TryGetValue(CombatTechId, out combatTech))
                PrimaryAbilityId = combatTech.PrimeAbilityId.Split("/");
            else
                PrimaryAbilityId = Array.Empty<string>();

            AtSkill = ComputeAttackVal(Hero.Abilities, Hero.CombatTechs);
            PaSkill = ComputeParryVal(Hero.Abilities, Hero.CombatTechs);
        }


        /// <summary>
        /// Compute effective attack value to roll against based on DSA5 rules. 
        /// Base values for AT and PA depend on: courage, character skill, weapon modifiers,
        /// and enhancements through the primary attribute.
        /// </summary>
        /// <param name="Abilities">A data frame containing the characters abilities</param>
        /// <param name="CombatTecSkill">Skill value(s) of combat techniques</param>
        /// <returns></returns>
        public int ComputeAttackVal(Dictionary<string, AbilityM> Abilities, Dictionary<string, CombatTechM> CombatTecSkill)
        {
            int PrimeAbility;

            // Get value for primary ability/attribute
            PrimeAbility = Abilities[PrimaryAbilityId[0]]?.Value ?? 0;
            if (PrimaryAbilityId.Length > 1) // more than 1 primary attribute
            {   
                for (int i = 1; i < PrimaryAbilityId.Length; i++)
                    PrimeAbility = Math.Max(PrimeAbility, Abilities[PrimaryAbilityId[i]].Value);
            }

            // Ability "Courage" is default for attack
            int Courage = Abilities[AbilityM.COU].Value;

            CombatTechM technique = CombatTecSkill[CombatTechId];

            int Attack = technique.ComputeAttack(Courage); // no weapons modifier
            Attack += AttackMod; // attack modifier of the weapon
            //- does not apply here but on damage Attack += Math.Max(PrimeAbility - DamageThreshold, 0); // if primary attribute > damage threshold ...

            return Attack;
        }


        /// <summary>
        /// Compute effective parry value to roll against based on DSA5 rules. 
        /// Base values for AT and PA depend on: courage, character skill, weapon modifiers,
        /// and enhancements through the primary attribute.
        /// </summary>
        /// <param name="Abilities">A data frame containing the characters abilities</param>
        /// <param name="CombatTecSkill">Skill value(s) of combat techniques</param>
        /// <returns></returns>
        public int ComputeParryVal(Dictionary<string, AbilityM> Abilities, Dictionary<string, CombatTechM> CombatTecSkill)
        {
            int PrimeAbility;

            // Get value for primary ability/attribute
            PrimeAbility = Abilities[PrimaryAbilityId[0]]?.Value ?? 0;
            if (PrimaryAbilityId.Length > 1) // more than 1 primary attribute
            {
                for (int i = 1; i < PrimaryAbilityId.Length; i++)
                    PrimeAbility = Math.Max(PrimeAbility, Abilities[PrimaryAbilityId[i]].Value);
            }

            CombatTechM technique = CombatTecSkill[CombatTechId];

            int Parry = technique.ComputeAttack(PrimeAbility); // no weapons modifier
            Parry += ParryMod; // attack modifier of the weapon
            Parry += Math.Max(PrimeAbility - DamageThreshold, 0); // if primary attribute > damage threshold ...

            return Parry;
        }


        ////' CalcDamage
        ////' Computes the hit point formula of the weapon. It takes the
        ////' character's abilities into account to get the bonus right.
        ////' @details The damage is determined by three components: [N]d[DP] + [Bonus]
        ////' @param CharAbs The character's abilities
        ////' @return Invisible returns `self`
        //public IRollM GetDamageRoll() 
        //{

        //    return null;
        //}


        /// <summary>
        /// Get possible hit point bonus depending on character's abilities.
        /// </summary>
        /// <param name="Abilities">A collection of abilities</param>
        /// <returns>A numeric value indicating the extra bonus that must be added to the
        /// weapons hit points.</returns>
        public int HitpointBonus(Dictionary<string, AbilityM> Abilities) 
        {
            if (DamageThreshold == 0) return 0;

            // Get value for primary ability/attribute
            int PrimeAbility = Abilities[PrimaryAbilityId[0]]?.Value ?? 0;
            if (PrimaryAbilityId.Length > 1) // more than 1 primary attribute
            {
                for (int i = 1; i < PrimaryAbilityId.Length; i++)
                    PrimeAbility = Math.Max(PrimeAbility, Abilities[PrimaryAbilityId[i]].Value);
            }

            int Bonus = Math.Max(PrimeAbility - DamageThreshold, 0);

            return Bonus;
        }

    }
}
