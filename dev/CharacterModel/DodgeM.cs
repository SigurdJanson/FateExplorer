﻿using FateExplorer.Shared;

namespace FateExplorer.CharacterModel
{
    /// <summary>
    /// Represents the competence to dodge
    /// </summary>
    public class DodgeM : DerivedValue
    {
        //--private readonly ICharacterM Hero;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hero">The character of this dodge competence</param>
        public DodgeM(ICharacterM hero) : base(ComputeDodge(hero.GetAbility(AbilityM.AGI)))
        {
            //--Hero = hero;
            Min = 0;
            Max = 20;
            DependencyId = new string[1] { AbilityM.AGI };
        }

        /// <summary>
        /// Computes a valid dodge value from a given effect
        /// </summary>
        /// <param name="EffectiveAgility">The agility of the character</param>
        /// <returns>An effective dodge value</returns>
        public static int ComputeDodge(int EffectiveAgility)
            => EffectiveAgility / 2 + EffectiveAgility % 2;


        ///// <summary>
        ///// The imported dodge value
        ///// </summary>
        //public int Value { get; protected set; }
    }
}
