using FateExplorer.GameData;
using FateExplorer.RollLogic;
using FateExplorer.Shared;
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

        public bool Ranged { get; protected set; }





        public string Name { get; set; }

        public string CombatTechId { get; set; }

        public CombatBranch Branch { get; protected set; }

        public int DamageDieCount { get; set; }

        public int DamageDieSides { get; set; }

        public int DamageBonus { get; set; }

        public int DamageThreshold { get; set; }

        public int AttackMod { get; set; }

        public int ParryMod { get; set; }

        public int Range { get; set; }

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
        /// <param name="hero"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WeaponM(ICharacterM hero)
        {
            Hero = hero ?? throw new ArgumentNullException(nameof(hero));

            // TODO: get "improvised" from DB
        }



        /// <summary>
        /// Provide the weapon with all the necessary data
        /// </summary>
        public void Initialise(WeaponDTO WeaponData, IGameDataService gameData)
        {
            CombatTechId = WeaponData.CombatTechId ?? CombatTechM.Unarmed;

            CombatTechM combatTech;
            if (Hero.CombatTechs.TryGetValue(CombatTechId, out combatTech))
                PrimaryAbilityId = combatTech.PrimeAbilityId.Split("/");
            else
            {
                string Temp = gameData.WeaponsMelee[WeaponData.Id].PrimeAttrID;
                CombatTechId = gameData.WeaponsMelee[WeaponData.Id].CombatTechID;
                PrimaryAbilityId = Temp.Split("/");
            }
            Ranged = combatTech.IsRanged;

            // Get template weapon from internal db
            string Template = WeaponData.Id;
            WeaponMeleeDbEntry DbWeapon = null;
            try
            {
                DbWeapon = gameData.WeaponsMelee[Template];
            } catch (Exception) { }
            


            Name = WeaponData.Name ?? "unknown";
            DamageDieCount = WeaponData.DamageDieCount;
            //DamageDieCount = WeaponData.DamageDieCount != 0 ? WeaponData.DamageDieCount : (DbWeapon?.DamageDieCount ?? 0)
            DamageDieSides = WeaponData.DamageDieSides;
            DamageThreshold = WeaponData.DamageThreshold;
            DamageBonus = WeaponData.DamageBonus; //TODO: must this be re-calculated???

            AttackMod = WeaponData.AttackMod;
            ParryMod = WeaponData.ParryMod;
            // Range = WeaponData.Range; // TODO currently ignored because anmbiguous for ranged vs melee

            Improvised = WeaponData.Improvised;
            TwoHanded = DbWeapon?.TwoHanded ?? false;
            Branch = gameData.CombatTechs[CombatTechId].WeaponsBranch;

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

            int Parry = technique.ComputeParry(PrimeAbility); // no weapons modifier
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
