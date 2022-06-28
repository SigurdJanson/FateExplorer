using FateExplorer.RollLogic;
using NUnit.Framework;
using System;

namespace UnitTests.RollLogic
{
    [TestFixture]
    public class RandomMersenneTests
    {

        private static void Fill<T>(T[] arrayOrList, T value)
        {
            for (int i = arrayOrList.Length - 1; i >= 0; i--)
                arrayOrList[i] = value;
        }


        private static double Exp(double x)
        {
            if (x < -40.0) // ACM update remark (8)
                return 0.0;
            else
                return Math.Exp(x);
        }


        public static double ChiFromFreqs(int[] observed, double[] expected)
        {
            double sum = 0.0;
            for (int i = 0; i < observed.Length; ++i)
            {
                sum += ((observed[i] - expected[i]) * (observed[i] - expected[i])) / expected[i];
            }
            return sum;
        }


        /// <summary>
        /// Calculate probability of a given chi² distributed value.
        /// </summary>
        /// <param name="x">A computed chi-square value</param>
        /// <param name="df">Degrees of freedom</param>
        /// <returns>Probability that x occurred by chance given the null hypothesis</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <remarks>
        /// Taken from the <see href="https://docs.microsoft.com/en-us/archive/msdn-magazine/2017/march/test-run-chi-squared-goodness-of-fit-using-csharp">
        /// MSDN Magazine (2017)</see>. Implements ACM 299.</remarks>
        public static double ChiSquarePval(double x, int df)
        {
            if (x == 0.0) return 1.0;
            if (x < 0.0)
                throw new ArgumentOutOfRangeException(nameof(x), "Chi² cannot be less than zero");
            if (df < 1)
                throw new ArgumentOutOfRangeException(nameof(df), "More than one degree of freedom required");
            double a; // = 0.0; // 299 variable names
            double y = 0.0;
            double s; // = 0.0;
            double z; // = 0.0;
            double ee; // = 0.0; // change from e
            double c;
            bool even; // Is df even?

            a = 0.5 * x;
            if (df % 2 == 0) even = true; else even = false;
            if (df > 1) y = Exp(-a); // ACM update remark (4)
            if (even == true) s = y;
            else s = 2.0 * Gauss(-Math.Sqrt(x));
            if (df > 2)
            {
                x = 0.5 * (df - 1.0);
                if (even == true) z = 1.0; else z = 0.5;
                if (a > 40.0) // ACM remark (5)
                {
                    if (even == true) ee = 0.0;
                    else ee = 0.5723649429247000870717135;
                    c = Math.Log(a); // log base e
                    while (z <= x)
                    {
                        ee = Math.Log(z) + ee;
                        s += Exp(c * z - a - ee); // ACM update remark (6)
                        z += 1.0;
                    }
                    return s;
                } // a > 40.0
                else
                {
                    if (even == true) 
                        ee = 1.0;
                    else
                        ee = 0.5641895835477562869480795 / Math.Sqrt(a);
                    c = 0.0;
                    while (z <= x)
                    {
                        ee *= (a / z); // ACM update remark (7)
                        c += ee;
                        z += 1.0;
                    }
                    return c * y + s;
                }
            } // df > 2
            else
            {
                return s;
            }
        } // ChiSquarePval()


        /// <summary>
        /// Calculate the probability of a gaussian distributed z.
        /// </summary>
        /// <param name="z">z-value (-inf to +inf)</param>
        /// <returns>p under Normal curve from -inf to z</returns>
        /// <remarks>
        /// Taken from the <see href="https://docs.microsoft.com/en-us/archive/msdn-magazine/2017/march/test-run-chi-squared-goodness-of-fit-using-csharp">
        /// MSDN Magazine (2017)</see>. Implements ACM 209.</remarks>
        public static double Gauss(double z)
        {
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called ‘z’ in 209
            double w; // 209 scratch variable
            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y
                      * 2.0;
                }
                else
                {
                    y -= 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                     + 0.006549791214) * y - 0.010557625006) * y
                     + 0.011630447319) * y - 0.009279453341) * y
                     + 0.005353579108) * y - 0.002141268741) * y
                     + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        } // Gauss()

        #region TESTING HELPERS
        [Test]
        [TestCase(4.102, 1, 5, ExpectedResult = 0.04283)] // taken from https://www.mathsisfun.com/data/chi-square-test.html
        [TestCase(10.828, 1, 5, ExpectedResult = 0.001)] // taken from https://www.medcalc.org/manual/chi-square-table.php
        [TestCase(1143.917, 1000, 5, ExpectedResult = 0.001)]
        [TestCase(0.0000393, 1, 5, ExpectedResult = 0.995)]
        [TestCase(888.564, 1000, 5, ExpectedResult = 0.995)]
        [TestCase(13.442, 10, 5, ExpectedResult = 0.20)]
        public double ChiSquarePVal_ValidResults(double Chi, int Df, int Decimals = 5)
        {
            return Math.Round(ChiSquarePval(Chi, Df), Decimals);
        }


        [Test]
        [TestCase(new int[] { 207, 282, 231, 242 }, new double[] { 222.64, 266.36, 215.36, 257.64 },
            3, ExpectedResult = 4.102)] // taken from https://www.mathsisfun.com/data/chi-square-test.html
        [TestCase(new int[] { 999, 1027 }, new double[] { 1013, 1013 }, 5, ExpectedResult = 0.38697)] // own example verified in R
        public double ChiFromFreqs_ValidResults(int[] Obs, double[] Exp, int Decimals = 5)
        {
            return Math.Round(ChiFromFreqs(Obs, Exp), Decimals);
        }
        #endregion



        #region TESTING RNG

        [DatapointSource]
        public int[] values = new int[] { 1, 2, 3, 5, 7, 11, 13 };
        [DatapointSource]
        public uint[] seeds = new uint[] { 1, 2, 3, 5, 7, 11, 13 };

        [Theory]
        public void IRandom_MinMax_ResultIsBetweenOrEqual(int min)
        {
            const int delta = 23;
            // Arrange
            var randomMersenne = new RandomMersenne();
            //int min = 0;
            int max = min + delta;

            // Act
            var result = randomMersenne.IRandom(min, max);

            // Assert
            Assert.LessOrEqual(min, result);
            Assert.GreaterOrEqual(max, result);
        }



        [Theory]
        public void Random_StateUnderTest_ExpectedBehavior(uint seed)
        {
            const double min = 0.0; // x by definition >= 0
            const double max = 1.0; // x by definition < 0

            // Arrange
            var randomMersenne = new RandomMersenne(seed);

            // Act
            var result = randomMersenne.Random();

            // Assert
            Assert.LessOrEqual(min, result);
            Assert.GreaterOrEqual(max, result);
        }



        [Theory]
        public void IRandom_Distribution_Uniform(uint seed, int length)
        {
            const int min = 1; // dice start with 1, so focus test on this
            const int trialsEach = 50000;
            int trials = length * trialsEach;

            // Arrange
            if (length <= 1) Assert.Pass("Do not test with length of 1");

            var randomMersenne = new RandomMersenne(seed+17);
            int[] ObsCount = new int[length];
            Fill<int>(ObsCount, 0);

            // Act
            for (int i = 0; i < trials; i++)
            {
                int result = randomMersenne.IRandom(min, min + length -1);
                ObsCount[result - min]++;
            }

            double[] ExpCount = new double[length];
            Fill<double>(ExpCount, trialsEach); // expected value

            var chi2 = ChiFromFreqs(ObsCount, ExpCount);
            double p = ChiSquarePval(chi2 < 0 ? 0.00001 : chi2, length-1);

            // Assert
            Assert.LessOrEqual(p, 0.95);
            Assume.That(p, Is.LessThan(0.80));
        }

        #endregion
    }
}
