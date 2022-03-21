using System;

namespace FateExplorer.Shared
{
    public static class StringHelpers
    {
        /// <summary>
        /// String extension method that determines the similarity of 2 strings.
        /// </summary>
        /// <param name="individual">First string to match</param>
        /// <param name="target">Second string to match</param>
        /// <returns>The number of leading characters that are exactly equal.</returns>
        public static int Fitness(this string individual, string target)
        {
            if (individual is null || target is null) return 0;

            int sum = 0;
            for (int i = 0; i < Math.Min(individual.Length, target.Length); i++)
                if (individual[i] == target[i])
                    sum++;
                else
                    return sum;
            return sum;
        }
    }
}
