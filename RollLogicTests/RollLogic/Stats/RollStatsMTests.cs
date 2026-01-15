using FateExplorer.RollLogic.Stats;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnitTests.RollLogic.Stats
{
    [TestFixture]
    public class RollStatsMTests
    {
        [SetUp]
        public void SetUp()
        {}

        private RollStatsM CreateRollStatsM()
        {
            return new RollStatsM();
        }

        #region Skills

        [TestCase(10, 10, 10,  2, 0, 0)]
        [TestCase(10, 11, 12,  5, 0, 1)]
        [TestCase( 2, 10, 18,  9, 0, 2)]
        [TestCase(19,  6, 14,  4, 5, 3)] // use case where fumbles must be subtracted from successes 
        [TestCase( 1,  5,  5,  1, 0, 4)]
        [TestCase( 6, 16, 18, 18, 0, 5)]
        [TestCase(12, 19, 19, 13, 0, 6)]
        public void ChancesOfSkill_BF_KnownTestCases_CorrectResult(int ab1, int ab2, int ab3, int skill, int mod, int exp)
        {
            // Arrange
            int[] abilities = [ab1, ab2, ab3];
            (List <string>, List<double>) expected = exp switch
            {
                0 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"], 
                      [0.00725, 0.786, 0.1995, 0.00725, 0.206750, 0.0, 0.0, 0.0, 0.0, 0.0]),
                1 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"], 
                      [0.00725, 0.5575, 0.428, 0.00725, 0.222, 0.21325, 0.0, 0.0, 0.0, 0.0]),
                2 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"], 
                      [0.00725, 0.588875, 0.396625, 0.00725, 0.1825, 0.110625, 0.11075, 0.0, 0.0, 0.0]),
                3 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.246375, 0.739125, 0.00725, 0.222625, 0.523750, 0.0, 0.0, 0.0, 0.0]),
                4 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.9795, 0.006, 0.00725, 0.013250, 0.0, 0.0, 0.0, 0.0, 0.0]),
                5 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.0, 0.9855, 0.00725, 0.027, 0.14925, 0.14925, 0.14925, 0.17225, 0.34575]),
                6 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.0, 0.9855, 0.00725, 0.0, 0.0995, 0.14925, 0.20125, 0.54275, 0.0]),
                _ => throw new ArgumentException("")
            };

            // Act
            var result = RollStatsM.ChancesOfSkill_BF(abilities, skill, mod);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1).Ulps);
        }


        [TestCase(10, 10, 10, 2, 0, 0)]
        [TestCase(10, 11, 12, 5, 0, 1)]
        [TestCase( 2, 10, 18, 9, 0, 2)]
        [TestCase(19,  6, 14, 4, 5, 3)] // Success too high by 14; fails too low by 14; ql1 too high by 14; ql2 ok // use case where fumbles must be subtracted from successes
        [TestCase( 1,  5,  5, 1, 0, 4)]
        [TestCase( 6, 16, 18, 18, 0, 5)]
        [TestCase(12, 19, 19, 13, 0, 6)]
        public void ChancesOfSkill_KnownTestCases_CorrectResult(int ab1, int ab2, int ab3, int skill, int mod, int exp)
        {
            // Arrange
            int[] abilities = [ab1, ab2, ab3];
            (List<string>, List<double>) expected = exp switch
            {
                0 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.786, 0.1995, 0.00725, 0.206750, 0.0, 0.0, 0.0, 0.0, 0.0]),
                1 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.5575, 0.428, 0.00725, 0.222, 0.21325, 0.0, 0.0, 0.0, 0.0]),
                2 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.588875, 0.396625, 0.00725, 0.1825, 0.110625, 0.11075, 0.0, 0.0, 0.0]),
                3 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.246375, 0.739125, 0.00725, 0.222625, 0.523750, 0.0, 0.0, 0.0, 0.0]),
                4 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.9795, 0.006, 0.00725, 0.013250, 0.0, 0.0, 0.0, 0.0, 0.0]),
                5 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.0, 0.9855, 0.00725, 0.027, 0.14925, 0.14925, 0.14925, 0.17225, 0.34575]),
                6 => (["Fumble", "Fail", "Success", "Critical", "QL1", "QL2", "QL3", "QL4", "QL5", "QL6"],
                      [0.00725, 0.0, 0.9855, 0.00725, 0.0, 0.0995, 0.14925, 0.20125, 0.54275, 0.0]),
                _ => throw new ArgumentException("")
            };

            // Act
            var result = RollStatsM.ChancesOfSkill(abilities, skill, mod);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(1).Ulps);
        }




        [Test, Sequential] // 
        public void ChancesOfSkill_CompareMethods_IdenticalResultsExpected(
            [Random(0, 20, 5)] int ab1, [Random(0, 20, 5)] int ab2, [Random(0, 20, 5)] int ab3,
            [Random(0, 20, 5)] int skill,
            [Random(-20, 20, 5)] int mod)
        {
            // Arrange
            int[] abilities = [ab1, ab2, ab3];
            if (ab1 + mod < 0) mod = -ab1;
            if (ab2 + mod < 0) mod = -ab2;
            if (ab3 + mod < 0) mod = -ab3;

            // Act
            var resultB = RollStatsM.ChancesOfSkill_BF(abilities, skill, mod);
            var resultO = RollStatsM.ChancesOfSkill(abilities, skill, mod);

            // Assert
            Assert.That(resultO, Is.EqualTo(resultB).Within(1.0 / 16000));
        }


        [Test, Sequential]
        public void ChancesOfSkill_BF_RandomInput_SumIs1(
            [Random(0, 20, 5)] int ab1, [Random(0, 20, 5)] int ab2, [Random(0, 20, 5)] int ab3,
            [Random(0, 20, 5)] int skill,
            [Values(-10, -5, 0, 5, 10)] int mod)
        {
            // Arrange
            int[] abilities = [ab1, ab2, ab3];
            if (ab1 + mod < 0) mod = -ab1;
            if (ab2 + mod < 0) mod = -ab2;
            if (ab3 + mod < 0) mod = -ab3;

            // Act
            var result = RollStatsM.ChancesOfSkill_BF(abilities, skill, mod);

            // Assert
            Assert.That(result.Chances[0..4].Sum(), Is.EqualTo(1.0).Within(1).Ulps);
        }


        [Test, Sequential]
        public void ChancesOfSkill_RandomInput_SumIs1(
            [Random(0, 20, 5)] int ab1, [Random(0, 20, 5)] int ab2, [Random(0, 20, 5)] int ab3,
            [Random(0, 20, 5)] int skill,
            [Values(-10, -5, 0, 5, 10)] int mod)
        {
            // Arrange
            int[] abilities = [ab1, ab2, ab3];
            if (ab1 + mod < 0) mod = -ab1;
            if (ab2 + mod < 0) mod = -ab2;
            if (ab3 + mod < 0) mod = -ab3;

            // Act
            var result = RollStatsM.ChancesOfSkill(abilities, skill, mod);

            // Assert
            Assert.That(result.Chances[0..4].Sum(), Is.EqualTo(1.0).Within(1).Ulps);
        }


        [TestCase(3, 1, 0, "1d3")]
        [TestCase(3, 1, 1, "1d3+1")]
        [TestCase(20, 1, 2, "1d20+2")]
        [TestCase(6, 2, 1, "2d6+1")]
        [TestCase(6, 3, 7, "3d6+7")]
        public void ChancesOfDiceRolls_KnownTestCases([Values(3, 6, 6)] int sides, [Values(1, 2, 3)] int count, [Values(0, 0, 11)] int modifier, string expected)
        {
            // Arrange
            List<string> expectedNames = expected switch
            {
                "1d3" => ["1", "2", "3"],
                "1d3+1" => ["2", "3", "4"],
                "1d20+2" => Enumerable.Range(3, 20).Select(i => i.ToString()).ToList(),
                "2d6+1" => Enumerable.Range(3, 11).Select(i => i.ToString()).ToList(),
                "3d6+7" => Enumerable.Range(10, 16).Select(i => i.ToString()).ToList(),
                _ => throw new ArgumentException("Unknown expectation")
            };
            List<double> expectedValues = expected switch
            {
                "1d3" => [1.0 / 3, 1.0 / 3, 1.0 / 3],
                "1d3+1" => [1.0 / 3, 1.0 / 3, 1.0 / 3],
                "1d20+2" => Enumerable.Repeat(1.0/20, 20).ToList(),
                "2d6+1" => [1.0/36, 1.0 / 18, 1.0 / 12, 1.0 / 9, 5.0/36, 1.0 / 6, 
                            5.0 / 36, 1.0 / 9, 1.0 / 12, 1.0 / 18, 1.0 /36],
                "3d6+7" => [1.0 / 216, 3.0/216, 6.0 / 216, 10.0 / 216, 15.0 / 216, 21.0 / 216, 25.0 / 216, 27.0 / 216, 
                            27.0 / 216, 25.0 / 216, 21.0 / 216, 15.0 / 216, 10.0 / 216, 6.0 / 216, 3.0 / 216, 1.0 / 216],
                _ => throw new ArgumentException("Unknown expectation")
            };
            // Act
            var (Names, Chances) = RollStatsM.ChancesOfDiceRolls(sides, count, modifier);

            // Assert
            Assert.That(Names, Is.EqualTo(expectedNames));
            Assert.That(Chances, Is.EqualTo(expectedValues));
        }


        [Test, Sequential]
        public void ChancesOfDiceRolls_RandomInput_SumIs1([Values(3, 6, 6)] int sides, [Values(1, 2, 3)] int count, [Values(0, 0, 11)] int modifier)
        {
            // Arrange
            // Act
            var result = RollStatsM.ChancesOfDiceRolls(sides, count, modifier);
            //for (int i = 0; i < result.Names.Count; i++)
            //{
            //    TestContext.WriteLine($"{result.Names[i]} = {result.Chances[i]}");
            //}

            // Assert
            Assert.That(result.Chances.Sum(), Is.EqualTo(1).Within(1.0 / Math.Pow(10, 15)));
        }

        #endregion Skills



        #region Combat

        [Test]
        [TestCase(5, 0, "5+0")]
        [TestCase(5, 5, "5+5")] // same as 10+0
        [TestCase(10, 0, "10+0")]
        [TestCase(10, 1, "10+1")]
        public void ChancesOfCombat_KnownTestCases_CorrectResult(int skill, int mod, string exp)
        {
            // Arrange
            List<double> expectedValues = exp switch
            {
                "5+0" => [1.0 / 20 * 3 / 4, 3.0 / 4 - 3.0 / 4 / 20,   1.0 / 4 - 1.0 / 20 * 1.0 / 4, 1.0 / 20 * 1.0 / 4],
                "5+5" => [1.0 / 20 * 0.5,   1.0 / 2 - 1.0 / 2 / 20,   1.0 / 2 - 1.0 / 20 * 1.0 / 2,   1.0 / 20 * 1.0 / 2],
                "10+0" => [1.0 / 20 * 0.5,  1.0 / 2 - 1.0 / 2 / 20,   1.0 / 2 - 1.0 / 20 * 1.0 / 2,   1.0 / 20 * 1.0 / 2],
                "10+1" => [1.0 / 20 * 9.0 / 20, 9.0 / 20 - 9.0 / 20 / 20,  11.0 / 20 - 1.0 / 20 * 11.0 / 20,   1.0 / 20 * 11.0 / 20],
                _ => throw new ArgumentException("Unknown expectation")
            };
            List<string> expectedNames = ["Fumble", "Fail", "Success", "Critical"];
            var Expected = (expectedNames, expectedValues);
            Assume.That(expectedValues.Sum(), Is.EqualTo(1.0).Within(1.0).Ulps);

            // Act
            var result = RollStatsM.ChancesOfCombat(skill, mod);

            // Assert
            Assert.That(result, Is.EqualTo(Expected).Within(1.0).Ulps);
        }


        [Test, Sequential]
        public void ChancesOfCombat_RandomInput_SumIs1([Random(0, 20, 5)] int skill, [Values(-10, -5, 0, 5, 10)] int mod)
        {
            // Arrange

            // Act
            var result = RollStatsM.ChancesOfCombat(skill, mod);

            // Assert
            Assert.That(result.Chances.Sum(), Is.EqualTo(1).Within(1.0 / Math.Pow(10, 15)));
        }



        [Test]
        [TestCase( 4, 0, 2, 1, 1, "AT4 1W2+1")]
        [TestCase(10, 0, 3, 1, 0, "AT10 1W3+0")]
        [TestCase(12, 0, 6, 1, 0, "AT12 1W6+0")]
        [TestCase(12, 0, 6, 1, 2, "AT12 1W6+2")]
        [TestCase(8, 0, 7, 1, 2, "AT8 1W7+2")]
        [TestCase(11, 0, 6, 2, 0, "AT11 2W6+0")]
        [TestCase(19, 0, 3, 3, 4, "AT19 3W3+4")]
        public void ChancesOfHitPoints_KnownTestCases_CorrectResult(int skill, int mod, int hpsides, int hpcount, int hpmod, string exp)
        {
            // Arrange
            double critchance = 0.05 * skill / 20; // 1/20 = 0.05
            double successchance = (double)skill / 20 - critchance;
            List<double> expectedValues = exp switch
            {
                "AT4 1W2+1"  => [0.095, 0.095, 0.005, 0.0, 0.005],
                "AT10 1W3+0" => [successchance/3.0, (successchance + critchance) / 3.0, successchance / 3.0, critchance / 3.0, 0.0, critchance / 3.0],
                "AT12 1W6+0" => [0.095, (successchance + critchance) / 6.0, successchance / 6.0, (successchance + critchance) / 6.0, 
                                 successchance / 6.0, (successchance + critchance) / 6.0,
                                 0.0, critchance / 6.0, 0.0, critchance / 6.0, 0.0, critchance / 6.0],
                "AT12 1W6+2" => [0.095, successchance / 6.0, successchance / 6.0, 
                                 (successchance + critchance) / 6.0, successchance / 6.0, (successchance + critchance) / 6.0,
                                 0.0, critchance / 6.0, 0.0, critchance / 6.0, 0.0, critchance / 6.0, 0.0, critchance / 6.0],
                "AT8 1W7+2"  => [0.0542857142857143, 0.0542857142857143, 0.0542857142857143, 0.0571428571428572, 
                                 0.0542857142857143, 0.0571428571428572, 0.0542857142857143, 0.00285714285714286, 0,   
                                 0.00285714285714286, 0, 0.00285714285714286, 0, 0.00285714285714286, 0, 0.00285714285714286],
                "AT11 2W6+0" => [0.0145138888888889, 0.0290277777777778, 0.0443055555555556, 
                    0.0580555555555556, 0.0740972222222222, 0.0870833333333334, 0.0748611111111111, 
                    0.0580555555555556, 0.0465972222222222, 0.0290277777777778, 0.0183333333333333, 
                    0.0, 0.00458333333333333, 0.0, 0.00381944444444444, 0.0, 0.00305555555555556,
                    0.0, 0.00229166666666667, 0.0, 0.00152777777777778, 0.0, 0.000763888888888889],
                "AT19 3W3+4" => [0.0334259259259259, 0.100277777777778, 0.200555555555556, 0.233981481481481, 0.200555555555556,
                    0.100277777777778, 0.0334259259259259, 0.00175925925925926, 0, 0.00527777777777778, 0, 0.0105555555555556, 0, 
                    0.0123148148148148, 0, 0.0105555555555556, 0, 0.00527777777777778, 0, 0.00175925925925926],
                _ => throw new ArgumentException("Unknown expectation")
            };
            List<string> expectedNames = exp switch
            {
                "AT4 1W2+1"  => ["2", "3", "4", "5", "6"],
                "AT10 1W3+0" => ["1", "2", "3", "4", "5", "6"],
                "AT12 1W6+0" => ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                "AT12 1W6+2" => ["3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"],
                "AT8 1W7+2"  => ["3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18"],
                "AT11 2W6+0" => ["2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24"],
                "AT19 3W3+4" => ["7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26"],
                _ => throw new ArgumentException("Unknown expectation")
            };

            // Act
            var result = RollStatsM.ChancesOfHitPoints(skill, mod, hpsides, hpcount, hpmod);

            // Assert
            Assert.That(result.Names, Is.EqualTo(expectedNames));
            Assert.That(result.Chances, Is.EqualTo(expectedValues).Within(Math.Pow(10, -15)));
        }



        [Test, Sequential]
        public void ChancesOfHitPoints_RandomInput_SumEqualsTotalAT(
            [Random(0, 20, 3)] int skill, [Values(0, 1, 5)] int mod,
            [Random(1, 5, 3)] int hpsides, [Random(1, 5, 3)] int hpcount, [Random(0, 10, 3)] int hpmod)
        {
            // Arrange
            double probEffectiveAT = Math.Min((double)(skill + mod) / 20, 1.0);

            // Act
            var result = RollStatsM.ChancesOfHitPoints(skill, mod, hpsides, hpcount, hpmod);

            // Assert
            Assert.That(result.Chances.Sum(), Is.EqualTo(probEffectiveAT).Within(Math.Pow(10, -15)));
        }

        #endregion Combat



        #region ProbabilityDistributions

        [Test, Sequential]
        public void UniformD20SumDistribution_CompareMethods_IdenticalResultsExpected()
        {
            // Arrange
            // Act
            var resultB = RollStatsM.UniformD20SumDistributionBF();
            var resultO = RollStatsM.UniformD20SumDistribution();

            // Assert
            Assert.That(resultO, Is.EqualTo(resultB));
        }



        [Test, Sequential]
        public void UniformSkillSumDistributionTrunc_CompareMethods_IdenticalResultsExpected(
            [Random(1, 30, 50)] int lowera, [Random(1, 30, 50)] int lowerb, [Random(1, 30, 50)] int lowerc)
        {
            // Arrange
            // Act
            var resultB = RollStatsM.UniformSkillSumDistributionTrunc(lowera, lowerb, lowerc);
            var resultO = RollStatsM.UniformSkillSumDistributionTruncO(lowera, lowerb, lowerc);
            for (int i = 0; i < resultB.Count; i++)
            {
                TestContext.WriteLine($"{i + 3}: {resultB[i]} vs {resultO[i]}");
            }
            //TestContext.WriteLine(resultB.);
            //TestContext.WriteLine(resultO);
            
            // Assert
            Assume.That(resultB.Sum(), Is.EqualTo(20 * 20 *20));
            Assert.That(resultO, Is.EqualTo(resultB));
        }


        [TestCase( 5, 15, 10, 0)]
        [TestCase( 9, 10, 11, 1)]
        [TestCase(20, 20, 20, 2)]
        [TestCase( 1,  1,  1, 3)]
        public void UniformSkillCriticalDistributionTrunc(int lowera, int lowerb, int lowerc, int exp)
        {
            // Arrange
            List<int> expected = exp switch
            {
                0 => ([.. Enumerable.Repeat(0, 29), 28, 3, 3, 3, 3, 3,  2, 2, 2, 2, 2,  1, 1, 1, 1, 1, .. Enumerable.Repeat(0, 15)]),
                1 => ([.. Enumerable.Repeat(0, 29), 28, .. Enumerable.Repeat(3, 9), 2, 1, .. Enumerable.Repeat(0, 19)]),
                2 => ([.. Enumerable.Repeat(0, 59), 58]),
                3 => ([0, 0, 1, .. Enumerable.Repeat(3, 19), .. Enumerable.Repeat(0, 38)]),
                _ => throw new ArgumentException("")
            };

            // Act
            var result = RollStatsM.UniformSkillCriticalDistributionTrunc((uint)lowera, (uint)lowerb, (uint)lowerc);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion ProbabilityDistributions




        #region Helpers

        [TestCase(0, ExpectedResult = 1)]
        [TestCase(3, ExpectedResult = 1)]
        [TestCase(4, ExpectedResult = 2)]
        [TestCase(6, ExpectedResult = 2)]
        [TestCase(7, ExpectedResult = 3)]
        [TestCase(9, ExpectedResult = 3)]
        [TestCase(10, ExpectedResult = 4)]
        [TestCase(12, ExpectedResult = 4)]
        [TestCase(13, ExpectedResult = 5)]
        [TestCase(15, ExpectedResult = 5)]
        [TestCase(16, ExpectedResult = 6)]
        public int SkillPoints2QualityLevels_PositiveBorderValues_CorrectResult(int skillPoints)
        {
            return RollStatsM.SkillPoints2QualityLevels(skillPoints);
        }

        [TestCase(17, ExpectedResult = 6)]
        [TestCase(25, ExpectedResult = 6)]
        [TestCase(57, ExpectedResult = 6)]
        public int SkillPoints2QualityLevels_SPAbove16_Returns6(int skillPoints)
        {
            return RollStatsM.SkillPoints2QualityLevels(skillPoints);
        }

        [TestCase(-1, ExpectedResult = 1)]
        [TestCase(-5, ExpectedResult = 1)]
        [TestCase(-547, ExpectedResult = 1)]
        public int SkillPoints2QualityLevels_NegativeSP16_Return1(int skillPoints)
        {
            return RollStatsM.SkillPoints2QualityLevels(skillPoints);
        }



        [TestCase(1, 4, ExpectedResult = 1)]
        [TestCase(2, 2, ExpectedResult = 1)]
        [TestCase(2, 7, ExpectedResult = 6)]
        [TestCase(2, 12, ExpectedResult = 1)]
        [TestCase(3, 2, ExpectedResult = 0)] // below min
        [TestCase(3, 7, ExpectedResult = 15)]
        //[TestCase(7, 30, ExpectedResult = 27132)]
        public int NoOfWays_d6(int nDice, int sum)
        {
            // Arrange
            const int sides = 6;
            // Act
            var result = RollStatsM.NoOfWays(sides, nDice, sum);
            // Assert
            //Assert.That(result, Is.EqualTo(15));
            return result;
        }



        [Test]
        public void ConvolveCounts_d6_ReturnsCorrectValue()
        {
            // Arrange
            List<int> pmfA = [1, 1, 1, 1, 1, 1];
            List<int> pmfB = [1, 1, 1, 1, 1, 1];

            // Act
            var result = RollStatsM.ConvolveCounts(pmfA, pmfB);

            // Assert
            Assert.That(result, Is.EqualTo([1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1]));
        }


        [Test]
        public void ConvolveCounts_d3_ReturnsCorrectValue()
        {
            // Arrange
            List<int> pmfA = [1, 1, 1];
            List<int> pmfB = [1, 1, 1];

            // Act
            var result = RollStatsM.ConvolveCounts(pmfA, pmfB);

            // Assert
            Assert.That(result, Is.EqualTo([1, 2, 3, 2, 1]));
        }

        #endregion Helpers

    }
}
