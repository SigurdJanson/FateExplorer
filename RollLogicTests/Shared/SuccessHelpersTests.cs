using FateExplorer.Shared;
using NUnit.Framework;
using System;

namespace RollLogicTests.Shared
{
    [TestFixture]
    public class SuccessHelpersTests
    {
        [Test]
        [TestCase(1, 1, ExpectedResult = RollSuccessLevel.PendingCritical)]
        [TestCase(2, 1, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(1, 2, ExpectedResult = RollSuccessLevel.PendingCritical)]
        [TestCase(2, 2, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(20, 20, ExpectedResult = RollSuccessLevel.PendingBotch)]
        [TestCase(19, 20, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(20, 19, ExpectedResult = RollSuccessLevel.PendingBotch)]
        [TestCase(19, 19, ExpectedResult = RollSuccessLevel.Success)]
        public RollSuccessLevel PrimaryD20Success(int Eyes, int Attr)
        {
            // Arrange
            // Act
            var result = SuccessHelpers.PrimaryD20Success(Eyes, Attr);

            // Assert
            return result;
        }


        [Test]
        [TestCase(1, 1, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(2, 1, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(1, 2, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(2, 2, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(20, 20, ExpectedResult = RollSuccessLevel.Botch)]
        [TestCase(19, 20, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(20, 19, ExpectedResult = RollSuccessLevel.Botch)]
        [TestCase(19, 19, ExpectedResult = RollSuccessLevel.Success)]
        public RollSuccessLevel D20Success(int Eyes, int Attr)
        {
            // Arrange
            // Act
            var result = SuccessHelpers.D20Success(Eyes, Attr);

            // Assert
            return result;
        }

        [Test]
        [TestCase(RollSuccessLevel.Critical, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(RollSuccessLevel.PendingCritical, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(RollSuccessLevel.Success, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Fail, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.PendingBotch, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.Botch, RollSuccessLevel.Critical, ExpectedResult = RollSuccessLevel.Fail)]

        [TestCase(RollSuccessLevel.Critical, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(RollSuccessLevel.PendingCritical, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(RollSuccessLevel.Success, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Fail, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.PendingBotch, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.Botch, RollSuccessLevel.Success, ExpectedResult = RollSuccessLevel.Fail)]

        [TestCase(RollSuccessLevel.Critical, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.PendingCritical, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Success, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Fail, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.PendingBotch, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Botch)]
        [TestCase(RollSuccessLevel.Botch, RollSuccessLevel.Fail, ExpectedResult = RollSuccessLevel.Botch)]

        [TestCase(RollSuccessLevel.Critical, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.PendingCritical, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Success, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(RollSuccessLevel.Fail, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(RollSuccessLevel.PendingBotch, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Botch)]
        [TestCase(RollSuccessLevel.Botch, RollSuccessLevel.Botch, ExpectedResult = RollSuccessLevel.Botch)]

        [TestCase(RollSuccessLevel.PendingCritical, RollSuccessLevel.na, ExpectedResult = RollSuccessLevel.PendingCritical)]
        [TestCase(RollSuccessLevel.PendingBotch, RollSuccessLevel.na, ExpectedResult = RollSuccessLevel.PendingBotch)]
        public RollSuccessLevel CheckSuccess(RollSuccessLevel Prim, RollSuccessLevel Conf)
        {
            // Arrange
            // Act
            var result = SuccessHelpers.CheckSuccess(Prim, Conf);

            // Assert
            return result;
        }

        [Test]
        [TestCase(1, 1, 20, ExpectedResult = RollSuccessLevel.Critical)]
        [TestCase(2, 1, 1, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(2, 1, 2, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(1, 2, 1, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(2, 2, 1, ExpectedResult = RollSuccessLevel.Fail)]
        [TestCase(2, 2, 2, ExpectedResult = RollSuccessLevel.Success)]
        public RollSuccessLevel CheckSuccess_StateUnderTest_ExpectedBehavior1(int PrimEyes, int ConfEyes, int Attr)
        {
            // Arrange
            // Act
            var result = SuccessHelpers.CheckSuccess(PrimEyes, ConfEyes, Attr);

            // Assert
            return result;
        }
    }
}
