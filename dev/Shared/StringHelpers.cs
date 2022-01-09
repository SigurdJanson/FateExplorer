using System;

namespace FateExplorer.Shared
{
    public static class StringHelpers
    {
        public static int Fitness(this string individual, string target)
        {
            int sum = 0;
            for (int i = 0; i < Math.Min(individual.Length, target.Length); i++)
                if (individual[i] == target[i]) sum++;
            return sum;
        }
    }
}
