
namespace FateExplorer.WPA.RollLogic
{
    /// <summary>
    /// An interface for various simple RNG implementation
    /// </summary>
    public interface IRandomNG
    {
        /// <summary>
        /// Set a new seed for the random number generator.
        /// </summary>
        /// <param name="seed">A number to initialize a pseudorandom number generator. It does not need to be random.</param>
        void RandomInit(uint seed);

        /// <summary>
        /// Output random integer in the interval min <= x <= max.
        /// </summary>
        /// <param name="min">Lowest value allowed</param>
        /// <param name="max">Maximum allowed value</param>
        /// <returns>An integer random number</returns>
        int IRandom(int min, int max);

        /// <summary>
        /// Output random float number in the interval 0 <= x < 1.
        /// </summary>
        /// <returns>An double random number</returns>
        double Random();

        /// <summary>
        /// Generate 32 random bits.
        /// </summary>
        /// <returns>A random 32 bits</returns>
        uint BRandom();
    }
}
