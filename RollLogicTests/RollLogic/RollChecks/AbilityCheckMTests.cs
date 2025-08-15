using FateExplorer.RollLogic;
using FateExplorer.Shared;
using NUnit.Framework;


namespace UnitTests.RollLogic
{
    [TestFixture]
    public class AbilityCheckMTests
    {



        [Test, Repeat(10)]
        public void RollSuccess_Primary_NoConfirmation_()
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
            var abilityCheckM = new AbilityCheckM(Ability, new BaseContextM(), null);

            // Act
            var result = abilityCheckM.SuccessOfRoll(RollType.Primary);

            // Assert primary roll
            Assert.Multiple(() =>
            {
                // ... that it must have changed from NA to somethign meaningful
                Assert.AreNotEqual(RollSuccess.Level.na, result);
                // ... that it is no fail because effective ability value is 20 and that's not possible
                Assert.AreNotEqual(RollSuccess.Level.Fail, result);
                // ... that there are no confirmed cricitals or botches
                Assert.AreNotEqual(RollSuccess.Level.Critical, result);
                Assert.AreNotEqual(RollSuccess.Level.Botch, result);
            });
            
            // Assert confirmation roll
            // ... that there is no confirmation result
            Assert.That(RollSuccess.Level.na, Is.EqualTo(abilityCheckM.SuccessOfRoll(RollType.Confirm)));

            // Assert total check
            Assert.That(result, Is.EqualTo(abilityCheckM.Success.CurrentLevel));
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
