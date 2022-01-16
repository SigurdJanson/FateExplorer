namespace FateExplorer.GameLogic
{
    /// <summary>
    /// Represents the competence to dodge
    /// </summary>
    public class DodgeM : IDerivedAttributeM
    {
        private ICharacterM Hero;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hero">The character of this dodge competence</param>
        public DodgeM(ICharacterM hero)
        {
            Hero = hero;
            Value = ComputeDodge(Hero.GetAbility(AbilityM.AGI));
        }

        /// <summary>
        /// Computes a valid dodge value from a given effect
        /// </summary>
        /// <param name="EffectiveAgility">The agility of the character</param>
        /// <returns>An effective dodge value</returns>
        public static int ComputeDodge(int EffectiveAgility)
            => EffectiveAgility / 2 + EffectiveAgility % 2;

        /// <summary>
        /// The basic dodge value
        /// </summary>
        public int Value { get; protected set; }


        /// <inheritdoc />
        /// <remarks>Implements <see cref="IDerivedAttributeM"/></remarks>
        public string[] DependentAttributes => new string[1] { AbilityM.AGI };
    }
}
