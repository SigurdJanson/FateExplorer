using FateExplorer.RollLogic;
using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Collections;

namespace RollLogicTests.RollLogic.RollChecks
{

    [TestFixture]
    public class SkillCheckMTests
    {
        //[Test]
        //public void HasNextStep_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var skillCheckM = new SkillCheckM(TODO, TODO);

        //    // Act
        //    var result = skillCheckM.HasNextStep();

        //    // Assert
        //    Assert.Fail();
        //}

        //[Test]
        //public void RollNextStep_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var skillCheckM = new SkillCheckM(TODO, TODO);

        //    // Act
        //    var result = skillCheckM.RollNextStep();

        //    // Assert
        //    Assert.Fail();
        //}


        #region ComputeSkillQuality

        public class SkillQualityCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                int[] Results = new int[]
                    { 0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 };
                for (int i = -1; i < 18; i++)
                    yield return new object[] { i, Results[i + 1] };
            }
        }

        [Test]
        [TestCaseSource(typeof(SkillQualityCases))]
        public void ComputeSkillQuality(int Remainder, int Expected)
        {
            // Arrange
            // Act
            var result = SkillCheckM.ComputeSkillQuality(Remainder);

            // Assert
            Assert.AreEqual(Expected, result);
        }

        #endregion



        #region ComputeSkillRemainder

        //  for (r in 2:10) {
        //    o <- VerifySkillRoll(rep(r, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = 0L)
        //    expect_identical(o, list(Message = "Success", QL = 1L, Remainder = 0L))
        //  }
        [Test]
        public void ComputeSkillRemainder_NoSkill_RemainderIs1([Range(2, 10, 1)] int Eyes)
        {
            // Arrange
            // Act
            var result = SkillCheckM.ComputeSkillRemainder(
                new int[3] { Eyes, Eyes, Eyes }, new int[3] { 10, 10, 10 }, 0, 0);

            // Assert
            Assert.AreEqual(0, result);
        }


        //  for (r in 11:19) {
        //    o <- VerifySkillRoll(rep(r, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = 0L)
        //    expect_identical(o, list(Message = "Fail", QL = 0L, Remainder = -3L*(r-10L)))
        //  }
        [Test]
        public void ComputeSkillRemainder_NoSkill_Fail_RemainderChanges([Range(11, 19, 1)] int Eyes)
        {
            const int Ability = 10;

            // Arrange
            int Expected = -3 * (Eyes - Ability);

            // Act
            var result = SkillCheckM.ComputeSkillRemainder(
                new int[3] { Eyes, Eyes, Eyes }, new int[3] { Ability, Ability, Ability }, 0, 0);

            // Assert
            Assert.AreEqual(Expected, result);
        }


        [Test]
        public void ComputeSkillRemainder_Critical_RemainderEqualsSkill(
            [Values(1)] int v1, [Values(1)] int v2, [Random(1, 20, 10)] int v3,
            [Random(0, 20, 5)] int Skill, [Random(-6, 6, 5)] int Mod)
        {
            const int Ability = 10;

            // Arrange
            int[] Roll = new int[3] { v1, v2, v3 };

            // Act
            var result = SkillCheckM.ComputeSkillRemainder(
                Roll, new int[3] { Ability, Ability, Ability }, Skill, Mod);

            // Assert
            Assert.AreEqual(Skill, result);
        }

        [Test]
        public void ComputeSkillRemainder_Botch_RemainderEquals0(
            [Values(20)] int v1, [Values(20)] int v2, [Random(1, 20, 10)] int v3,
            [Random(0, 20, 5)] int Skill, [Random(-6, 6, 5)] int Mod)
        {
            const int Ability = 10;

            // Arrange
            // Act
            var result = SkillCheckM.ComputeSkillRemainder(
                new int[3] { v1, v2, v3 }, new int[3] { Ability, Ability, Ability }, Skill, Mod);

            // Assert
            Assert.AreEqual(0, result);
        }


        //  for (m in -5L:-1L) {
        //    o <- VerifySkillRoll(rep(10L, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = m)
        //    expect_identical(o, list(Message = "Fail", QL = 0L, Remainder = 3L*m))
        //  }
        //  for (m in 0L:5L) {
        //    o <- VerifySkillRoll(rep(10L, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = m)
        //    expect_identical(o, list(Message = "Success", QL = 1L, Remainder = 0L))
        //  }
        [Test]
        public void ComputeSkillRemainder_ChangingModifier_ConstantRemainder([Range(-5, 5, 1)] int Modifier)
        {
            const int Ability = 10;
            const int Roll = 10;

            // Arrange
            int Expected = Modifier < 0 ? 3 * Modifier : 0;

            // Act
            var result = SkillCheckM.ComputeSkillRemainder(
                new int[3] { Roll, Roll, Roll }, new int[3] { Ability, Ability, Ability }, 0, Modifier);

            // Assert
            Assert.AreEqual(Expected, result);
        }


        // Example Core Rules, p. 22
        [Test]
        [TestCase(new int[3] { 10, 18, 12 }, new int[3] { 13, 12, 14 }, 7, 0, ExpectedResult = 1)]
        public int ComputeSkillRemainder_Samples(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            // Arrange
            // Act
            var result = SkillCheckM.ComputeSkillRemainder(Eyes, Attributes, Skill, Mod);

            // Assert
            return result;
        }



        //  # A skill < 0 is always a fail (even with fantastic abilities and divine mod)
        //  o <- VerifySkillRoll(rep(10L, 3), Abilities = c(19L, 17L, 18L), Skill = -1L, Modifier = 99L)
        //  expect_identical(o, list(Message = "Fail", QL = 0L, Remainder = -1L))
        //})
        //#########################

        #endregion



        #region ComputSuccess Level

        //  for (r in 2:10) {
        //    o <- VerifySkillRoll(rep(r, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = 0L)
        //    expect_identical(o, list(Message = "Success", QL = 1L, Remainder = 0L))
        //  }
        [Test]
        public void ComputeSuccess_NoSkill_ReturnsSuccess([Range(2, 10, 1)] int Eyes)
        {
            // Arrange
            // Act
            var result = SkillCheckM.ComputeSuccess(
                new int[3] { Eyes, Eyes, Eyes }, new int[3] { 10, 10, 10 }, 0, 0);

            // Assert
            Assert.AreEqual(RollSuccessLevel.Success, result);
        }


        //  for (r in 11:19) {
        //    o <- VerifySkillRoll(rep(r, 3), Abilities = c(10L, 10L, 10L), Skill = 0L, Modifier = 0L)
        //    expect_identical(o, list(Message = "Fail", QL = 0L, Remainder = -3L*(r-10L)))
        //  }
        [Test]
        public void ComputeSuccess_NoSkill_ReturnFail([Range(11, 19, 1)] int Eyes)
        {
            const int Ability = 10;

            // Act
            var result = SkillCheckM.ComputeSuccess(
                new int[3] { Eyes, Eyes, Eyes }, new int[3] { Ability, Ability, Ability }, 0, 0);

            // Assert
            Assert.AreEqual(RollSuccessLevel.Fail, result);
        }


        [Test]
        public void ComputeSuccess_Critical_CriticalLevel(
            [Values(1)] int v1, [Values(1)] int v2, [Random(1, 20, 10)] int v3,
            [Random(0, 20, 5)] int Skill, [Random(-6, 6, 5)] int Mod)
        {
            const int Ability = 10;

            // Arrange
            int[] Roll = new int[3] { v1, v2, v3 };

            // Act
            var result = SkillCheckM.ComputeSuccess(
                Roll, new int[3] { Ability, Ability, Ability }, Skill, Mod);

            // Assert
            Assert.AreEqual(RollSuccessLevel.Critical, result);
        }


        [Test]
        public void ComputeSuccess_Botch_BotchLevel(
            [Values(20)] int v1, [Values(20)] int v2, [Random(1, 20, 10)] int v3,
            [Random(0, 20, 5)] int Skill, [Random(-6, 6, 5)] int Mod)
        {
            const int Ability = 10;

            // Arrange
            // Act
            var result = SkillCheckM.ComputeSuccess(
                new int[3] { v1, v2, v3 }, new int[3] { Ability, Ability, Ability }, Skill, Mod);

            // Assert
            Assert.AreEqual(RollSuccessLevel.Botch, result);
        }


        // Example Core Rules, p. 22
        [Test]
        [TestCase(new int[3] { 10, 18, 12 }, new int[3] { 13, 12, 14 }, 7, 0, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(new int[3] { 10, 18, 12 }, new int[3] { 13, 12, 14 }, 7, -1, ExpectedResult = RollSuccessLevel.Success)]
        [TestCase(new int[3] { 10, 18, 12 }, new int[3] { 13, 12, 14 }, 7, -2, ExpectedResult = RollSuccessLevel.Fail)]
        public RollSuccessLevel ComputeSuccess_Samples(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            // Arrange
            // Act
            var result = SkillCheckM.ComputeSuccess(Eyes, Attributes, Skill, Mod);

            // Assert
            return result;
        }


        #endregion

        //[Test]
        //public void RollSuccess_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var skillCheckM = new SkillCheckM(TODO, TODO);
        //    int Roll = 0;

        //    // Act
        //    var result = skillCheckM.RollSuccess(
        //        Roll);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
