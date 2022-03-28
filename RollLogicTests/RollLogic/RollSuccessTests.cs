using FateExplorer.RollLogic;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.RollLogic
{
    [TestFixture]
    public class RollSuccessTests
    {
        private static IEnumerable<TestCaseData> LevelCombinations
        {
            get
            {
                yield return new TestCaseData(RollSuccess.Level.Critical, RollSuccess.Level.Critical, RollSuccess.Level.Critical);
                yield return new TestCaseData(RollSuccess.Level.PendingCritical, RollSuccess.Level.Critical, RollSuccess.Level.Critical);
                yield return new TestCaseData(RollSuccess.Level.Success, RollSuccess.Level.Critical, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Fail, RollSuccess.Level.Critical, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.PendingBotch, RollSuccess.Level.Critical, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.Botch, RollSuccess.Level.Critical, RollSuccess.Level.Fail);

                yield return new TestCaseData(RollSuccess.Level.Critical, RollSuccess.Level.Success, RollSuccess.Level.Critical);
                yield return new TestCaseData(RollSuccess.Level.PendingCritical, RollSuccess.Level.Success, RollSuccess.Level.Critical);
                yield return new TestCaseData(RollSuccess.Level.Success, RollSuccess.Level.Success, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Fail, RollSuccess.Level.Success, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.PendingBotch, RollSuccess.Level.Success, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.Botch, RollSuccess.Level.Success, RollSuccess.Level.Fail);

                yield return new TestCaseData(RollSuccess.Level.Critical, RollSuccess.Level.Fail, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.PendingCritical, RollSuccess.Level.Fail, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Success, RollSuccess.Level.Fail, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Fail, RollSuccess.Level.Fail, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.PendingBotch, RollSuccess.Level.Fail, RollSuccess.Level.Botch);
                yield return new TestCaseData(RollSuccess.Level.Botch, RollSuccess.Level.Fail, RollSuccess.Level.Botch);

                yield return new TestCaseData(RollSuccess.Level.Critical, RollSuccess.Level.Botch, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.PendingCritical, RollSuccess.Level.Botch, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Success, RollSuccess.Level.Botch, RollSuccess.Level.Success);
                yield return new TestCaseData(RollSuccess.Level.Fail, RollSuccess.Level.Botch, RollSuccess.Level.Fail);
                yield return new TestCaseData(RollSuccess.Level.PendingBotch, RollSuccess.Level.Botch, RollSuccess.Level.Botch);
                yield return new TestCaseData(RollSuccess.Level.Botch, RollSuccess.Level.Botch, RollSuccess.Level.Botch);

                // Pending stays pending as long as confirmation is undefined
                yield return new TestCaseData(RollSuccess.Level.PendingCritical, RollSuccess.Level.na, RollSuccess.Level.PendingCritical);
                yield return new TestCaseData(RollSuccess.Level.PendingBotch, RollSuccess.Level.na, RollSuccess.Level.PendingBotch);

                // if primary is na, return na whatever the confirmation is
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.na, RollSuccess.Level.na);
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.PendingCritical, RollSuccess.Level.na);
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.Critical, RollSuccess.Level.na);
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.Fail, RollSuccess.Level.na);
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.Success, RollSuccess.Level.na);
                yield return new TestCaseData(RollSuccess.Level.na, RollSuccess.Level.Botch, RollSuccess.Level.na);
            }
        }


        [Test, TestCaseSource(nameof(LevelCombinations))]
        public void Update_ByLevel(RollSuccess.Level P, RollSuccess.Level C, RollSuccess.Level expectedResult)
        {
            // Arrange
            var rollSuccess = new RollSuccess();

            // Act
            rollSuccess.Update(P, C);

            // Assert
            Assert.AreEqual(expectedResult, rollSuccess.CurrentLevel);
        }


        [Test]
        [TestCase(1, 1, 0, RollSuccess.Level.Critical)]
        [TestCase(1, -1, 1, RollSuccess.Level.PendingCritical)]
        [TestCase(1, 2, 1, RollSuccess.Level.Success)]
        [TestCase(10, 20, 10, RollSuccess.Level.Success)] // Confirm should be ignored
        [TestCase(10, 1,  9, RollSuccess.Level.Fail)]
        [TestCase(2, -1, 1, RollSuccess.Level.Fail)]
        [TestCase(2, -1, 2, RollSuccess.Level.Success)]
        [TestCase(2, -1, 3, RollSuccess.Level.Success)]
        [TestCase(20, 20, 20, RollSuccess.Level.Botch)]
        [TestCase(19, 20, 19, RollSuccess.Level.Success)]
        [TestCase(20, 19, 19, RollSuccess.Level.Fail)]
        public void Update_ByRoll(int P, int C, int Attr, RollSuccess.Level expectedResult)
        {
            // Arrange
            var rollSuccess = new RollSuccess();
            DieRollM pRoll = new(20);
            pRoll.OpenRoll[0] = P;

            DieRollM cRoll;
            if (C > 0)
            {
                cRoll = new(20);
                cRoll.OpenRoll[0] = C;
            }
            else
            {
                cRoll = null;
            }

            // Act
            rollSuccess.Update(pRoll, cRoll, Attr);

            // Assert
            Assert.AreEqual(expectedResult, rollSuccess.CurrentLevel);
        }






        [Test, TestCaseSource(nameof(LevelCombinations))]
        public void staticCheckSuccess_Combinations(RollSuccess.Level Prim, RollSuccess.Level Conf, RollSuccess.Level expected)
        {
            // Arrange
            //var rollSuccess = new RollSuccess();

            // Act
            var result = RollSuccess.CheckSuccess(Prim, Conf);

            // Assert
            Assert.AreEqual(expected, result);
        }




        [Test]
        [TestCase(1, 1, 20, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(2, 1, 1, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(2, 1, 2, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(1, 2, 1, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(2, 2, 1, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(2, 2, 2, ExpectedResult = RollSuccess.Level.Success)]
        public RollSuccess.Level CheckSuccess_Combinations(int PrimEyes, int ConfEyes, int Attr)
        {
            // Arrange
            var rollSuccess = new RollSuccess();

            // Act
            var result = rollSuccess.CheckSuccess(PrimEyes, ConfEyes, Attr);

            // Assert
            return result;
        }
    }
}
