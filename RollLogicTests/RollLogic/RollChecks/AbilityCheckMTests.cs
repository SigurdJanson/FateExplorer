using FateExplorer.RollLogic;
using FateExplorer.Shared;
using FateExplorer.ViewModel;
using NUnit.Framework;
using System;

namespace RollLogicTests.RollLogic.RollChecks
{
    [TestFixture]
    public class AbilityCheckMTests
    {

        [Test]
        public void RollSuccess_NothingRolled_Exception()
        {
            AbilityDTO Ability = new()
            {
                Name = "Test-Ability", Id = "TestId", ShortName = "TT",
                Max = 10, Min = 0, EffectiveValue = 20
            };
            // Arrange
            var abilityCheckM = new AbilityCheckM(Ability, new SimpleCheckModifierM(0));

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var result = abilityCheckM.RollSuccess(0);
            });
        }



        [Test, Repeat(10)]
        public void RollSuccess_StateUnderTest_ExpectedBehavior()
        {
            AbilityDTO Ability = new()
            {
                Name = "Test-Ability",
                Id = "TestId",
                ShortName = "TT",
                Max = 10,
                Min = 0,
                EffectiveValue = 20
            };
            // Arrange
            var abilityCheckM = new AbilityCheckM(Ability, new SimpleCheckModifierM(0));
            abilityCheckM.RollNextStep();

            // Act
            var result = abilityCheckM.RollSuccess(0);

            // Assert
            Assert.AreEqual(RollSuccessLevel.Success, result);
        }



        [Test, Ignore("Not implemented")]
        public void RollNextStep_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            //var abilityCheckM = new AbilityCheckM(TODO, TODO);

            // Act
            //var result = abilityCheckM.RollNextStep();

            // Assert
            Assert.Fail();
        }
    }
}
