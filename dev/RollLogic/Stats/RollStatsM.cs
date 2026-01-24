using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FateExplorer.RollLogic.Stats;

public class RollStatsM
{
    /// <summary>
    /// Represents the maximum value for a standard 20-sided die (d20).
    /// </summary>
    protected const int MaxD20 = 20;
    /// <summary>
    /// Represents the maximum quality level (QL) achievable in skill checks.
    /// </summary>
    protected const int MaxQL = 6;


    /// <summary>
    /// Calculates the probabilities of various outcomes for a skill check based on brute-force enumeration of all possible dice rolls.
    /// </summary>
    /// <param name="abilities">An array of exactly three base ability values for the skill check.</param>
    /// <param name="skill">The skill level applied to the check.</param>
    /// <param name="modifier">An advantage or penalty modifier applied to the abilities before the check.</param>
    /// <returns>A list of tuples where each tuple contains an outcome name and its probability.</returns>
    /// <exception cref="ArgumentException">Thrown if any effective ability (ability + modifier) is less than or equal to zero, or if the abilities array does not contain exactly three elements.</exception>
    /// <remarks>
    /// The algorithm uses brute-force enumeration over all 8000 (20^3) possible dice combinations. 
    /// It simulates rolling 3d20 and evaluates each combination against the effective abilities 
    /// (base + modifier) plus skill. 
    /// Outcomes include: Fumble, Fail, Success, Critical, and Quality Levels QL1 through QL6.
    /// Fumbles and Criticals are precomputed as constants based on specific dice conditions 
    /// (e.g., two or more ones for criticals).
    /// </remarks>
    public static (List<string> Names, List<double> Chances) ChancesOfSkill_BF(int[] abilities, int skill, int modifier = 0)
    {
        // Precondition checks
        foreach (int ability in abilities)
        {
            if (ability + modifier < 0)
                throw new ArgumentException("Cannot roll against effective skill < 0");
        }

        const int Rolls = 3;    // Number of dice rolled

        if (abilities.Length != Rolls)
            throw new ArgumentException("3 ability values required");

        // Fumbles and criticals counts based on fixed logic:
        // 1 if all three dice are 20; (3*19) if exactly two dice are 20.
        int Fumbles = 1 + 3 * 19;
        int Criticals = 1 + 3 * 19;

        // Compute effective abilities (base + modifier)
        int[] effectiveAbilities = new int[Rolls];
        for (int i = 0; i < Rolls; i++)
            effectiveAbilities[i] = Math.Min(abilities[i] + modifier, MaxD20);
        int totalEffective = effectiveAbilities.Sum(e => Math.Min(e, MaxD20));

        long success = 0;
        long[] qs = new long[MaxQL]; // Index 0 corresponds to QL1

        if (effectiveAbilities.Any(ea => ea < 1))
        { // "If an EAV ever falls below 1, success is impossible", VR1 p. 21
            Fumbles = 0;
            Criticals = 0;
            success = 0;
            qs = [0, 0, 0, 0, 0, 0];
        }
        else
        {
            // Brute-force enumeration over all possible 3d20 rolls
            for (int d1 = 1; d1 <= MaxD20; d1++)
            {
                for (int d2 = 1; d2 <= MaxD20; d2++)
                {
                    for (int d3 = 1; d3 <= MaxD20; d3++)
                    {
                        int[] roll = [d1, d2, d3];

                        // Compute Check as element-wise maximum between roll and effective abilities
                        int[] check = new int[Rolls];
                        int sumCheck = 0;
                        for (int i = 0; i < Rolls; i++)
                        {
                            check[i] = Math.Max(roll[i], effectiveAbilities[i]);
                            sumCheck += check[i];
                        }

                        // Count number of ones in roll
                        int onesCount = 0;
                        int twentyCount = 0;
                        foreach (int die in roll)
                        {
                            if (die == 1) onesCount++;
                            if (die == 20) twentyCount++;
                        }

                        // Determine success condition
                        bool isCritical = onesCount > 1;
                        bool isFumble = twentyCount > 1;
                        bool isNormalSuccess = sumCheck <= totalEffective + skill;

                        // Determine quality level
                        int currentQS;
                        if (isCritical)
                        {
                            currentQS = SkillPoints2QualityLevels(skill);
                            qs[currentQS - 1]++; // Adjust for 0-based indexing
                        }
                        else if (isNormalSuccess && !isFumble)
                        {
                            success++;
                            int penalty = sumCheck - totalEffective;
                            currentQS = SkillPoints2QualityLevels(skill - penalty);
                            if (currentQS <= MaxQL)
                                qs[currentQS - 1]++; // Adjust for 0-based indexing
                        }
                    }
                }
            } // d1,d2,d3
        } // else valid effective abilities

        double totalOutcomes = Math.Pow(MaxD20, Rolls);
        List<double> result =
            [
                Fumbles / totalOutcomes,
                (totalOutcomes - success - Fumbles - Criticals) / totalOutcomes, // fail
                success / totalOutcomes,
                Criticals / totalOutcomes
            ];
        List<string> names = ["Fumble", "Fail", "Success", "Critical"];

        // Append quality levels
        for (int i = 0; i < MaxQL; i++)
        {
            result.Add(qs[i] / totalOutcomes);
            names.Add($"QL{i + 1}");
        }

        return FormatChances(names, result);
    }





    /// <summary>
    /// Calculates the probabilities of various outcomes for a skill check using analytical convolution instead of brute-force enumeration.
    /// </summary>
    /// <param name="abilities">An array of exactly three base ability values for the skill check.</param>
    /// <param name="skill">The skill level applied to the check.</param>
    /// <param name="modifier">An advantage or penalty modifier applied to the abilities before the check.</param>
    /// <returns>A list of tuples where each tuple contains an outcome name and its probability.</returns>
    /// <exception cref="ArgumentException">Thrown if any effective ability (ability + modifier) is less than or equal to zero, or if the abilities array does not contain exactly three elements.</exception>
    /// <remarks>
    /// This method computes the exact same results as the original brute-force R function but uses discrete convolution
    /// to avoid iterating over all 8000 dice combinations. It preserves the original logic for fumbles, criticals, and quality levels.
    /// Note: The original R code treats fumbles and criticals symmetrically (both = 58 outcomes), which is maintained here.
    /// </remarks>
    public static (List<string> Names, List<double> Chances) ChancesOfSkill(int[] abilities, int skill, int modifier)
    {
        const int MaxD = 20;
        const int MaxQS = 6;
        const int Rolls = 3;
        const int TotalOutcomes = MaxD * MaxD * MaxD; // 8000

        // Precondition checks
        ArgumentOutOfRangeException.ThrowIfLessThan(skill, 0);
        if (abilities.Length != Rolls)
            throw new ArgumentException("Exactly 3 ability values required");

        // Fumbles and criticals: fixed at 58 outcomes each, as in original
        long Fumbles = 58;
        long Criticals = 58;

        // Compute effective abilities
        int[] effectiveAbilities = abilities.Select(a => Math.Min(a + modifier, MaxD)).ToArray(); // ++++
        int totalEffective = effectiveAbilities.Sum(); // 


        long success = 0;
        //long fail = 0;
        long[] qs = new long[MaxQS]; // QL1 to QL6
        if (effectiveAbilities.Any(ea => ea <= 0))
        {
            Fumbles = 0;
            Criticals = 0;
            success = 0;
        }
        else
        {
            // Step 1: Build PMF for each adjusted die C_i = max(d_i, E_i) using counts (not probabilities)
            List<int> pmfS = UniformSkillSumDistributionTrunc(effectiveAbilities[0], effectiveAbilities[1], effectiveAbilities[2]);

            // Step 2: Process all possible sums
            for (int sumS = totalEffective; sumS <= Rolls * MaxD; sumS++)
            {
                int count = pmfS[sumS - 1]; // zero-based index correction

                if (sumS <= totalEffective + skill)
                {
                    success += count;
                    // Compute QL for these outcomes (will adjust for criticals later)
                    int penalty = sumS - totalEffective;
                    int currentQS = SkillPoints2QualityLevels(skill - penalty);
                    qs[currentQS - 1] += count; // zero-based index correction
                }
            }

            // Correct for fumbles that ended up in the success category
            // This happens, when the effective skills are so high that even 2 twenties
            // so not cause a failure. Normally, fumbles are separated from the failure category.
            foreach (var eav in effectiveAbilities)
            {
                // Compute the skill remainder when the other two eav's are fumbles
                // `(totalEffective - eav)` is the sum of the other two effective abilities
                // Subtracting `(2 * MaxD20)` accounts for the two dice showing 20 (fumble)
                int skillRemainder = skill - ((2 * MaxD) - (totalEffective - eav));
                if (skillRemainder < 0) continue; // no misclassification possible

                // if remainder >= 0, then fumbles were counted as successes
                int misClassified = Math.Min(eav + skillRemainder, MaxD);

                success -= misClassified;

                // Also correct quality levels starting with the highest remainder
                int m;
                int currentQS = SkillPoints2QualityLevels(skillRemainder);
                qs[currentQS - 1] -= eav; // all rolls <= eav were misclassified with maximum QL
                for (m = misClassified - eav; m > 0; m--)
                {
                    skillRemainder--;
                    currentQS = SkillPoints2QualityLevels(skillRemainder);
                    qs[currentQS - 1] -= 1;
                }
                
                if (m > 0) // Finally, if there's still misclassified outcomes, they must be lowest QL
                {
                    currentQS = SkillPoints2QualityLevels(--skillRemainder);
                    qs[currentQS] -= m;
                }
            }
            // Correct for the triple 20: When all 3 "eav + skillRemainder > 20" with a
            // fumble (on the other two) we subtract 2 too many from the successes.
            if (totalEffective + skill >= Rolls * MaxD)
            {
                success += Rolls - 1;
                int skillRemainder = skill - (3*MaxD - totalEffective);
                int currentQS = SkillPoints2QualityLevels(skillRemainder);
                qs[currentQS - 1] += Rolls - 1;
            }
        }


        // Step 3: Assemble results in the exact same order as the original R function
        List<double> result =
        [
            (double)Fumbles / TotalOutcomes,                       // Fumble
            1.0 - ((double)(success + Fumbles) / TotalOutcomes),   // Fail - `success` still contains criticals
            (double)(success-Criticals) / TotalOutcomes,           // Success
            (double)Criticals / TotalOutcomes                      // Critical
        ];
        List<string> names = ["Fumble", "Fail", "Success", "Critical"];

        for (int i = 0; i < MaxQS; i++)
        {
            result.Add( (double)qs[i] / TotalOutcomes);          // QL1 to QL6
            names.Add($"QL{i + 1}");
        }

        return FormatChances(names, result);
    }



    /// <summary>
    /// Calculates the probabilities of each possible combat outcome based on the specified skill and modifier values.
    /// </summary>
    /// <param name="skill">The skill value used in the combat check. Must be greater than or equal to 0.</param>
    /// <param name="modifier">An additional modifier applied to the check.</param>
    /// <returns>A tuple containing a list of outcome names and a corresponding list of probabilities. The names are "Fumble",
    /// "Fail", "Success", and "Critical"; the probabilities represent the chance of each outcome, in the same order.</returns>
    public static (List<string> Names, List<double> Chances) ChancesOfCombat(int skill, int modifier)
    {
        double effective = Math.Min(Math.Max(skill + modifier, 0), MaxD20);

        double critical = 1.0 / MaxD20 * effective / MaxD20; // Critical if die is 1 and confirmed with 'success'
        double success = effective / MaxD20 - critical;
        double fumble = 1.0 / 20 * (1 - effective / MaxD20);        // Fumble if die is 20 and *not* avoided with 'success'
        double fail = (1.0 - effective / MaxD20) - fumble;//- success - critical - fumble;

        List<double> result =
        [
            fumble,                 // Fumble
            fail,                   // Fail excl. fumbles
            success,                // Success excl. criticals
            critical                // Critical
        ];
        List<string> names = ["Fumble", "Fail", "Success", "Critical"];

        return FormatChances(names, result);
    }


    /// <summary>
    /// Calculates the probabilities of achieving various hit point outcomes based on skill, check modifier, 
    /// and dice parameters to get the hit points for an attack with a weapon.
    /// </summary>
    /// <param name="skill">The AT value to roll against</param>
    /// <param name="checkmod">The AT modifier</param>
    /// <param name="hpsides">The faces (sides) of the dice to get the hit points</param>
    /// <param name="hpcount">The number of dice</param>
    /// <param name="hpmod">A modifier to be added to the hit point roll</param>
    /// <returns></returns>
    public static (List<string> Names, List<double> Chances) ChancesOfHitPoints(int skill, int checkmod, int hpsides, int hpcount, int hpmod)
    {
        double TotalOutcomes = (int)Math.Pow(hpsides, hpcount);
        double effective = Math.Min(Math.Max(skill + checkmod, 0), MaxD20);
        int maxHp = (hpcount * hpsides + hpmod) * 2;

        double critical = 1.0 / MaxD20 * effective / MaxD20; // Critical if die is 1 and confirmed with 'success'
        double success = effective / MaxD20 - critical;
        double fumble = critical;        // Fumble if die is 20 and *not* avoided with 'success'
        double fail = 1.0 - success - critical - fumble;

        var sumCount = DiceSumDistribution(hpsides, hpcount, hpmod);
        while (sumCount[0] == 0)
            sumCount.RemoveAt(0); // Remove leading zeroes

        int lowest = hpcount + hpmod; // lowest possible outcome
        List<string> hitPointNames = [.. Enumerable.Range(hpcount+hpmod, maxHp - lowest + 1).Select(n => n.ToString())];
        List<double> hitPointChances = [.. Enumerable.Repeat(0.0, maxHp - lowest + 1)];

        // Add regular successes
        for (int i = 0; i < sumCount.Count; i++)
        {
            hitPointChances[i] = sumCount[i] / TotalOutcomes * success;
        }
        // Add criticals
        for (int i = 0; i < sumCount.Count; i++)
        {
            int index = (i + hpmod + 1) * 2 + (hpcount - 1) - (hpmod + 1);
            hitPointChances[index] += sumCount[i] / TotalOutcomes * critical;
        }
        // TODO

        return FormatChances(hitPointNames, hitPointChances);
    }



    /// <summary>
    /// Computes the probabilities of all possible outcomes when rolling a specified 
    /// number of dice with a given number of sides, including an optional modifier.
    /// </summary>
    /// <param name="sides">Sides (faces) of the dice</param>
    /// <param name="count">The number of dice (all with <paramref name="sides"/> sides)</param>
    /// <param name="modifier">A bonus (> 0) or penalty (< 0), otherwise zero.</param>
    /// <returns></returns>
    public static (List<string> Names, List<double> Chances) ChancesOfDiceRolls(int sides, int count, int modifier)
    {
        double total = Math.Pow(sides, count);

        //List<int> pd = [];
        //for(int i = 0; i < sides; i++) pd.Add(1);

        //List<int> sumCount = [];
        //sumCount = pd;
        //for (int i = 1; i < count; i++)
        //{
        //    sumCount = ConvolveCounts(sumCount, pd);
        //}
        var sumCount = DiceSumDistribution(sides, count, modifier);
        while (sumCount[0] == 0) sumCount.RemoveAt(0); // Remove leading zeroes

        List<double> result= [];
        List<string> names = [];
        for (int i = 0; i < sumCount.Count; i++)
        {
            result.Add( sumCount[i] / total );
            names.Add($"{i + count + modifier}");
        }

        return FormatChances(names, result);
    }



    #region ProbabilityDistributions


    // https://en.wikipedia.org/wiki/Irwin%E2%80%93Hall_distribution
    // https://www.geeksforgeeks.org/dsa/dice-throw-dp-30/
    // https://stats.stackexchange.com/questions/46872/how-to-compute-the-distribution-of-sums-when-rolling-n-dice-with-m-faces
    // https://stats.stackexchange.com/questions/116792/dungeons-dragons-attack-hit-probability-success-percentage

    // https://csharphelper.com/howtos/howto_calculate_n_choose_k.html
    // https://stackoverflow.com/questions/12983731/algorithm-for-calculating-binomial-coefficient



    /// <summary>
    /// Determines the number of events that lead to a sum <paramref name="x"/> with 
    /// <paramref name="count"/> dice with <paramref name="sides"/> sides each.
    /// </summary>
    /// <param name="sides">The sides of the dice. A number > 2.</param>
    /// <param name="count">Number of dice. Number > 0.</param>
    /// <param name="x">The event defined by the sum of faces.</param>
    /// <remarks>
    /// <see href="https://www.geeksforgeeks.org/dsa/dice-throw-dp-30/">See source</see>
    /// </remarks>
    public static int NoOfWays(int sides, int count, int x)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(sides, 2);
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        // Create a 2D dp array with (count+1) rows and (x+1)
        // columns dp[i, j] will store the number of ways to
        // get a sum of 'j' using 'i' dice
        int[,] dp = new int[count + 1, x + 1];

        // Base case: There is 1 way to get a sum of 0 with 0 dice
        dp[0, 0] = 1;

        // Loop through each die (i) from 1 to count
        for (int i = 1; i <= count; i++)
        {
            // Loop through each sum (j) from 1 to x
            for (int j = 1; j <= x; j++)
            {
                // Loop through all possible dice values (k)
                // from 1 to sides and if the sum j - k is valid
                // (non-negative), add the number of ways
                // from dp[i-1, j-k]
                for (int k = 1; k <= sides && j - k >= 0; k++)
                {
                    dp[i, j] += dp[i - 1, j - k];
                }
            }
        }

        // The sumCount will be in dp[count, x], which contains
        // the number of ways to get sum 'x' using 'count' dice
        return dp[count, x];
    }


    /// <summary>
    /// Generates the distribution of sums for rolling a specified number of dice with a 
    /// given number of sides.
    /// </summary>
    /// <param name="sides">The sides of the dice</param>
    /// <param name="count">The number of dice</param>
    /// <param name="modifier">An additive modifier. A bonus > 0, a penalty < 0, or none = 0.</param>
    /// <returns></returns>
    public static List<int> DiceSumDistribution(int sides, int count, int modifier)
    {
        List<int> pd = [];
        for (int i = modifier; i < sides + modifier; i++) pd.Add(1);

        List<int> sumCount = [];
        sumCount = pd;
        for (int i = 1; i < count; i++)
        {
            sumCount = ConvolveCounts(sumCount, pd);
        }
        return sumCount;
    }


    /// <summary>
    /// Computes the distribution of sums for three uniformly distributed d20 rolls 
    /// using brute-force enumeration.
    /// </summary>
    /// <returns>A list counting the occurrencies of each pip sum. Pip sums range from 1 to 60 
    /// (projected to a zero-based list with indices 0-59).</returns>
    public static List<int> UniformD20SumDistributionBF()
    {
        var result = new List<int>(new int[60]);
        for (int a = 1; a <= 20; a++)
        {
            for (int b = 1; b <= 20; b++)
            {
                for (int c = 1; c <= 20; c++)
                {
                    int sum = a + b + c;
                    result[sum-1] += 1;
                }
            }
        }
        return result;
    }



    /// <summary>
    /// Returns the precomputed distribution of sums for three uniformly distributed d20 rolls.
    /// </summary>
    /// <returns>A list counting the occurrencies of each pip sum. Pip sums range from 1 to 60 
    /// (projected to a zero-based list with indices 0-59).</returns>
    public static List<int> UniformD20SumDistribution()
    {
        List<int> mem = [0, 0, 1, 3, 6, 10, 15, 21, 28,
            36, 45, 55, 66, 78, 91, 105, 120, 136, 153,
            171, 190, 210, 228, 244, 258, 270, 280, 288, 294,
            298, 300, 300, 298, 294, 288, 280, 270, 258, 244,
            228, 210, 190, 171, 153, 136, 120, 105, 91, 78,
            66, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1];
        return mem;
    }




    /// <summary>
    /// Computes the distribution of critical outcomes for three uniformly distributed d20 skills.
    /// 
    /// </summary>
    /// <param name="lowera">Lowest possible value for the first d20 (equals the effective ability)</param>
    /// <param name="lowerb">Lowest possible value for a second d20 (equals the effective ability)</param>
    /// <param name="lowerc">Lowest possible value for the third d20 (equals the effective ability)</param>
    /// <returns>
    /// A list counting the occurrencies of each pip sum. Pip sums range from 1 to 60 
    /// (projected to a zero-based list with indices 0-59).
    /// </returns>
    public static List<int> UniformSkillCriticalDistributionTrunc(uint lowera = 0, uint lowerb = 0, uint lowerc = 0)
    {
        Debug.Assert(lowera >= 1); Debug.Assert(lowerb >= 1); Debug.Assert(lowerc >= 1);
        Debug.Assert(lowera <= MaxD20); Debug.Assert(lowerb <= MaxD20); Debug.Assert(lowerc <= MaxD20);
        // Preparations
        int lowerSum = (int)(lowera + lowerb + lowerc);

        List<int> result = [.. Enumerable.Repeat(0, 3 * MaxD20)];

        // Sort lower bounds ascending: lowera >= lowerb >= lowerc
        if (lowera < lowerb) (lowera, lowerb) = (lowerb, lowera);
        if (lowera < lowerc) (lowera, lowerc) = (lowerc, lowera);
        if (lowerb < lowerc) (lowerb, lowerc) = (lowerc, lowerb);

        //

        // 
        int index = lowerSum - 1;
        result[index] = lowerSum - 2; // minus triple 1 // beware the -1 index shift
        
        for (int i = 0; i < MaxD20 - (int)lowera; i++)
        {
            result[++index] += 3;
        }
        for (int i = 0; i < (int)lowera - (int)lowerb; i++)
        {
            result[++index] += 2;
        }
        for (int i = 0; i < (int)lowerb - (int)lowerc; i++)
        {
            result[++index] += 1;
        }
        // the remaining values are zero and have been initialized as such -> done
        return result;
    }
    /// <summary>
    /// Calculates the distribution of possible sums for three uniformly distributed attributes, 
    /// with each skill ranging from 1 to 20. This distribution differs from the 
    /// D20SumDistribution in 2 things. First, each skill is truncated by the skill's value.
    /// And second, if two or more skills roll a 1, the sum is set to the sum of the lower bounds
    /// irregardless of the value of the 3rd skill.
    /// </summary>
    /// <remarks>The returned list has a length of 61, corresponding to possible sums from 0 to 60. Sums less
    /// than the specified lower bound are not possible and will have a value of 0. This method can be used to analyze
    /// the frequency distribution of total skill values under a uniform random assignment, with a minimum enforced
    /// sum.</remarks>
    /// <param name="lower">The minimum value to which each sum is truncated. All sums less than this value are increased to match this
    /// lower bound.</param>
    /// <returns>A list where each index represents a possible sum, and the value at each index is the number of combinations of
    /// skills that sumCount in that sum after truncation. Indices with a value of 0 represent sums that are not possible.</returns>
    public static List<int> UniformSkillSumDistributionTrunc(int lowera, int lowerb, int lowerc)
    {
        lowera = Math.Min(lowera, MaxD20);
        lowerb = Math.Min(lowerb, MaxD20);
        lowerc = Math.Min(lowerc, MaxD20);

        var result = new List<int>(new int[3 * MaxD20]);

        for (int a = 1; a <= MaxD20; a++)
        {
            for (int b = 1; b <= MaxD20; b++)
            {
                for (int c = 1; c <= MaxD20; c++)
                {
                    int sum;
                    if ((a == 1 && b == 1) || (b == 1 && c == 1) || (a == 1 && c == 1))
                        sum = lowera + lowerb + lowerc;
                    else
                        sum = Math.Max(a, lowera) + Math.Max(b, lowerb) + Math.Max(c, lowerc);
                    result[sum - 1] += 1;
                }
            }
        }
        return result;
    }



    #endregion ProbabilityDistributions



    #region AverageAndMedian

    /// <summary>
    /// Computes the average result of rolling a specified number of dice with a given number of sides, 
    /// including an optional modifier.
    /// </summary>
    /// <param name="sides">The sides (faces) of all <paramref name="count"/> dice. The 3 in <c>3d6+1</c>.</param>
    /// <param name="count">The number of dice. The 6 in <c>3d6+1</c>.</param>
    /// <param name="modifier">An additive modifier. The 1 in <c>3d6+1</c>.</param>
    /// <returns></returns>
    /// <remarks>Formula: n × (h + 1) / 2 + m with n = count, h = sides, m = modifier.</remarks>
    public static double DiceSumAverage(int sides, int count, int modifier)
    {
        return count * (sides + 1) / 2.0 + modifier;
    }


    /// <summary>
    /// Calculates the mean value from a discrete probability distribution.
    /// </summary>
    /// <param name="values">A list of discrete numeric events</param>
    /// <param name="probabilities">A list of probabilities in the same order as <paramref name="values"/>.</param>
    /// <returns>The arithmetic mean of the distribution.</returns>
    /// <remarks>This method does not validate the probabily distribution.</remarks>
    public static double MeanFromDistribution(List<int> values, List<double> probabilities)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Count, probabilities.Count,
            "Internal error: Values and probabilities must have the same length.");

        double sum = 0;
        for (int i = 0; i < probabilities.Count; i++)
            sum += probabilities[i] * values[i];

        return sum;
    }


    /// <summary>
    /// Determines the median value from a discrete probability distribution.
    /// </summary>
    /// <param name="values">A list of discrete numeric events.</param>
    /// <param name="probabilities">A list of probabilities in the same order as <paramref name="values"/>.</param>
    /// <param name="interpolate">If <codetrue</param> the median is interpolated between two values if necessary.</param>
    /// <returns>The median of the distribution.</returns>
    /// <remarks>This method does not validate the probabily distribution.</remarks>
    public static double MedianFromDistribution(List<int> values, List<double> probabilities, bool interpolate = false)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(values.Count, probabilities.Count, 
            "Internal error: Values and probabilities must have the same length.");
        const double MedianQuantile = 0.5;

        double cumsum = 0;
        int i;
        for (i = 0; i < probabilities.Count; i++)
        {
            cumsum += probabilities[i];
            if (cumsum >= MedianQuantile)
                break;
        }
        double median_value = values[i];

        // Optional interpolation
        if (i > 0 && interpolate && !IsExactlyHalfway(median_value))
        {
            median_value = values[i - 1] + (MedianQuantile - probabilities[i - 1]) / (probabilities[i] - probabilities[i - 1]) * (median_value - values[i - 1]);
        }
        return median_value;
    }

    #endregion AverageAndMedian



    #region Helpers

    /// <summary>
    /// Converts (remaining) skill points into quality levels (QL).
    /// </summary>
    /// <param name="skillPoints">Skill points remaing after a skill check</param>
    /// <returns>A quality level, i..e an integer between 1 and 6.</returns>
    /// <remarks>This method does *not* determine failed checks.</remarks>
    public static int SkillPoints2QualityLevels(int skillPoints)
    {
        // Treat negative as zero
        skillPoints = skillPoints < 0 ? 0 : skillPoints;

        // Ceiling of (skillPoints / 3) using integer math
        int currentQS = (skillPoints + 2) / 3;

        // Clamp between 1 and MaxQL (inclusive)
        if (currentQS < 1) 
            currentQS = 1;
        else if (currentQS > MaxQL) 
            currentQS = MaxQL;

        return currentQS;
    }



    /// <summary>
    /// Combines two probability mass functions (PMFs) by convolution.
    /// </summary>
    /// <param name="pmfA"></param>
    /// <param name="pmfB"></param>
    /// <returns></returns>
    public static List<int> ConvolveCounts(List<int> pmfA, List<int> pmfB)
    {
        var result = new List<int>(new int[pmfA.Count + pmfB.Count - 1]); // +2 to avoid index issues
        for (int a = 0; a < pmfA.Count; a++)
        {
            for (int b = 0; b < pmfB.Count; b++)
            {
                int sum = a + b;
                int prob = pmfA[a] * pmfB[b];
                result[sum] = result[sum] + prob;
            }
        }
        return result;
    }



    /// <summary>
    /// Brings the outputs of the chance calculations into a standard format.
    /// </summary>
    /// <param name="names">A list of names (length is equal to <paramref name="probs"/>.</param>
    /// <param name="probs">A list of probabilities.</param>
    /// <returns></returns>
    public static (List<string> names, List<double> probs) FormatChances(List<string> names, List<double> probs)
    {
        return (names, probs);
    }


    /// <summary>
    /// Determines whether the specified value is exactly halfway between two consecutive integers. 
    /// It supports a specified tolerance to account for floating-point precision issues.
    /// </summary>
    /// <remarks>This method is useful for detecting values that are equidistant from two integers, such as
    /// when rounding or handling floating-point precision.</remarks>
    /// <param name="value">The value to evaluate for being halfway between two integers.</param>
    /// <param name="tolerance">The maximum allowed difference from the exact halfway point. 
    /// Must be a non-negative number. The default is 1e-8.</param>
    /// <returns><c>true</c> if the value is within the specified tolerance of the midpoint 
    /// between two consecutive integers; otherwise, <c>false</c>.</returns>
    public static bool IsExactlyHalfway(double value, double tolerance = 1e-8)
    {
        // Calculate the midpoint
        double midpoint = (Math.Ceiling(value) + Math.Floor(value)) / 2.0;

        // Compare with tolerance
        return Math.Abs(value - midpoint) < tolerance;
    }


    #endregion Helpers


}
