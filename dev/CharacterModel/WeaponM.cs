﻿using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;

namespace FateExplorer.CharacterModel
{
    public class WeaponM
    {
        protected const int DefaultSkillValue = 6;

        protected ICharacterM Hero { get; set; }



        /// <summary>
        /// Idenitifies the primary ability that may grant a special bonus on hit points
        /// </summary>
        public string[] PrimaryAbilityId { get; protected set; }

        /// <summary>
        /// Default ATTACK skill when use in main hand and no two-handed combat
        /// </summary>
        public int BaseAtSkill { get; protected set; }

        /// <summary>
        /// Default PARRY skill when use in main hand and no two-handed combat
        /// </summary>
        public int BasePaSkill { get; protected set; }



        /// <summary>
        /// The attack skill value after taking into account if the character carries a 
        /// second weapon or even uses the non-dominant hand.
        /// </summary>
        /// <param name="MainHand">Is 'this' weapon carried in the main hand?</param>
        /// <param name="otherHand">Combat branch of weapon in other hand.</param>
        /// <returns></returns>
        public int AtSkill(bool MainHand, CombatBranch otherHand)
        {
            int OffHandMod;
            if (Hero.HasAdvantage(ADV.Ambidexterous))
                OffHandMod = 0;
            else if (Branch != CombatBranch.Unarmed)
                OffHandMod = !MainHand ? -4 : 0;
            else
                OffHandMod = 0;


            int TwoHandedCombatTier =  Hero.HasSpecialAbility(SA.TwoHandedCombat) switch
            {
                false => 0,
                true => Hero.SpecialAbilities[SA.TwoHandedCombat].Tier
            };
            int TwoHandMod = otherHand switch
            {
                CombatBranch.Unarmed => 0,
                CombatBranch.Shield => 0,
                CombatBranch.Ranged => -2 + TwoHandedCombatTier, // second weapon
                CombatBranch.Melee => -2 + TwoHandedCombatTier, // second weapon
                _ => 0
            };

            // Return the result which shall not be < 0
            return Math.Max(BaseAtSkill + OffHandMod + TwoHandMod, 0);
        }


        /// <summary>
        /// The parry skill value after taking into account if the character carries a 
        /// second weapon or even uses the non-dominant hand.
        /// </summary>
        /// <param name="MainHand">Is 'this' weapon carried in the main hand?</param>
        /// <param name="otherHand">Combat branch of weapon in other hand.</param>
        /// <param name="otherPaSkill">Parry skill of weapon carried by the other hand.</param>
        /// <param name="otherIsParry">Is the other hand's weapon classified as parry weapon?</param>
        /// <returns></returns>
        public int PaSkill(bool MainHand, CombatBranch otherHand, int otherPaSkill, bool otherIsParry) // TODO: SIMPLE: switch bool and int parry args
        {
            // Determine off-hand penalty
            int OffHandMod;
            if (Hero.HasAdvantage(ADV.Ambidexterous))
                OffHandMod = 0;
            else if (Branch == CombatBranch.Shield)
                OffHandMod = 0; // no off-hand penalty for shields
            else if (Branch == CombatBranch.Unarmed)
                OffHandMod = 0; // no off-hand penalty for unarmed
            else
                OffHandMod = !MainHand ? -4 : 0;


            int TwoHandedCombatTier = Hero.HasSpecialAbility(SA.TwoHandedCombat) switch
            {
                false => 0,
                true => Hero.SpecialAbilities[SA.TwoHandedCombat].Tier
            };
            int TwoHandMod = otherHand switch
            {
                CombatBranch.Unarmed => 0,
                CombatBranch.Shield => 0,
                CombatBranch.Ranged => -2 + TwoHandedCombatTier, // second weapon
                CombatBranch.Melee => -2 + TwoHandedCombatTier, // second weapon
                _ => 0
            };


            // Determine passive bonus of parry weapon or shield
            int ParryMod;
            if (otherIsParry)
                ParryMod = 1;
            else if(otherHand == CombatBranch.Shield)
                ParryMod = otherPaSkill;
            else
                ParryMod = 0;

            // Return the result which shall not be < 0
            return Math.Max(BasePaSkill + OffHandMod + TwoHandMod + ParryMod, 0);
        }



        public string Name { get; set; }

        public string CombatTechId { get; set; }

        public CombatBranch Branch { get; protected set; }

        public int DamageDieCount { get; set; }

        public int DamageDieSides { get; set; }

        public int DamageBonus { get; set; }

        public int DamageThreshold { get; set; }

        public int AttackMod { get; set; }

        public int ParryMod { get; set; }

        public int Reach { get; set; }

        public int[] Range { get; set; }

        public bool Ranged { get; protected set; }

        public bool Improvised { get; set; }

        public bool TwoHanded { get; set; }

        public bool CanParry
        {
            get
            {
                CombatTechM combatTech;
                if (Hero.CombatTechs.TryGetValue(CombatTechId, out combatTech))
                    return combatTech.CanParry;
                else
                    throw new Exception($"Unknown combat technique {CombatTechId}");
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hero">The character carrying this weapon</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WeaponM(ICharacterM hero)
        {
            Hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }



        /// <summary>
        /// Provide the weapon with all the necessary data
        /// </summary>
        public void Initialise(WeaponDTO WeaponData, IGameDataService gameData)
        {
            Name = WeaponData.Name;
            CombatTechId = WeaponData.CombatTechId;
            PrimaryAbilityId = (string[])WeaponData.PrimaryAbilityId.Clone();

            DamageDieCount = WeaponData.DamageDieCount;
            DamageDieSides = WeaponData.DamageDieSides;
            DamageThreshold = WeaponData.DamageThreshold;
            DamageBonus = WeaponData.DamageBonus + HitpointBonus(Hero.Abilities);

            Improvised = WeaponData.Improvised;
            TwoHanded = WeaponData.Twohanded;
            Branch = WeaponData.Branch;
            Ranged = WeaponData.Ranged;

            AttackMod = WeaponData.AttackMod;
            ParryMod = WeaponData.ParryMod;
            if (Ranged)
                Range = (int[])WeaponData.Range.Clone();
            else
                Reach = WeaponData.Reach;

            BaseAtSkill = ComputeAttackVal(Hero.Abilities, Hero.CombatTechs[CombatTechId]);
            BasePaSkill = ComputeParryVal(Hero.Abilities, Hero.CombatTechs[CombatTechId]);
        }


        /// <summary>
        /// Compute effective attack value to roll against based on DSA5 rules. 
        /// Base values for AT and PA depend on: courage, character skill, weapon modifiers,
        /// and enhancements through the primary attribute.
        /// </summary>
        /// <param name="Abilities">A data frame containing the characters abilities</param>
        /// <param name="CombatTecSkill">Skill value(s) of combat techniques</param>
        /// <returns></returns>
        public int ComputeAttackVal(Dictionary<string, AbilityM> Abilities, CombatTechM CombatTecSkill)
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

            //-CombatTechM technique = CombatTecSkill[CombatTechId];

            int Attack = CombatTecSkill.ComputeAttack(Courage); // no weapons modifier
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
        public int ComputeParryVal(Dictionary<string, AbilityM> Abilities, CombatTechM CombatTecSkill)
        {
            int PrimeAbility;

            // Get value for primary ability/attribute
            PrimeAbility = Abilities[PrimaryAbilityId[0]]?.Value ?? 0;
            if (PrimaryAbilityId.Length > 1) // more than 1 primary attribute
            {
                for (int i = 1; i < PrimaryAbilityId.Length; i++)
                    PrimeAbility = Math.Max(PrimeAbility, Abilities[PrimaryAbilityId[i]].Value);
            }

            //-CombatTechM technique = CombatTecSkill[CombatTechId];

            int Parry = CombatTecSkill.ComputeParry(PrimeAbility); // no weapons modifier
            Parry += ParryMod; // attack modifier of the weapon
            Parry += Math.Max(PrimeAbility - DamageThreshold, 0); // if primary attribute > damage threshold ...

            return Parry;
        }



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
