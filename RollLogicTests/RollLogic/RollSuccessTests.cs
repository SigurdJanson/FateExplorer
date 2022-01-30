using FateExplorer.RollLogic;
using NUnit.Framework;
using System;

namespace RollLogicTests.RollLogic
{
    [TestFixture]
    public class RollSuccessTests
    {
        [Test]
        [TestCase(1, 1, ExpectedResult = RollSuccess.Level.PendingCritical)]
        [TestCase(2, 1, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(1, 2, ExpectedResult = RollSuccess.Level.PendingCritical)]
        [TestCase(2, 2, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(20, 20, ExpectedResult = RollSuccess.Level.PendingBotch)]
        [TestCase(19, 20, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(20, 19, ExpectedResult = RollSuccess.Level.PendingBotch)]
        [TestCase(19, 19, ExpectedResult = RollSuccess.Level.Success)]
        public RollSuccess.Level PrimaryD20Success_DefaultBotchThreshold_Permutations(int Eyes, int Attr)
        {
            // Arrange
            var rollSuccess = new RollSuccess();

            // Act
            var result = rollSuccess.PrimaryD20Success(Eyes, Attr);

            // Assert
            return result;
        }



        [Test]
        [TestCase(1, 1, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(2, 1, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(1, 2, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(2, 2, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(20, 20, ExpectedResult = RollSuccess.Level.Botch)]
        [TestCase(19, 20, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(20, 19, ExpectedResult = RollSuccess.Level.Botch)]
        [TestCase(19, 19, ExpectedResult = RollSuccess.Level.Success)]
        public RollSuccess.Level D20Success_Permutations(int Eyes, int Attr)
        {
            // Arrange
            var rollSuccess = new RollSuccess();

            // Act
            var result = rollSuccess.D20Success(Eyes, Attr);

            // Assert
            return result;
        }

        [Test]
        [TestCase(RollSuccess.Level.Critical, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(RollSuccess.Level.PendingCritical, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(RollSuccess.Level.Success, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Fail, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.PendingBotch, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.Botch, RollSuccess.Level.Critical, ExpectedResult = RollSuccess.Level.Fail)]

        [TestCase(RollSuccess.Level.Critical, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(RollSuccess.Level.PendingCritical, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Critical)]
        [TestCase(RollSuccess.Level.Success, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Fail, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.PendingBotch, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.Botch, RollSuccess.Level.Success, ExpectedResult = RollSuccess.Level.Fail)]

        [TestCase(RollSuccess.Level.Critical, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.PendingCritical, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Success, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Fail, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.PendingBotch, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Botch)]
        [TestCase(RollSuccess.Level.Botch, RollSuccess.Level.Fail, ExpectedResult = RollSuccess.Level.Botch)]

        [TestCase(RollSuccess.Level.Critical, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.PendingCritical, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Success, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Success)]
        [TestCase(RollSuccess.Level.Fail, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Fail)]
        [TestCase(RollSuccess.Level.PendingBotch, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Botch)]
        [TestCase(RollSuccess.Level.Botch, RollSuccess.Level.Botch, ExpectedResult = RollSuccess.Level.Botch)]

        [TestCase(RollSuccess.Level.PendingCritical, RollSuccess.Level.na, ExpectedResult = RollSuccess.Level.PendingCritical)]
        [TestCase(RollSuccess.Level.PendingBotch, RollSuccess.Level.na, ExpectedResult = RollSuccess.Level.PendingBotch)]
        public RollSuccess.Level staticCheckSuccess_Combinations(RollSuccess.Level Prim, RollSuccess.Level Conf)
        {
            // Arrange
            //var rollSuccess = new RollSuccess();

            // Act
            var result = RollSuccess.CheckSuccess(Prim, Conf);

            // Assert
            return result;
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
