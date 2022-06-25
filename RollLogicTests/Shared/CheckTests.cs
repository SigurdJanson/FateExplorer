using FateExplorer.Shared;
using NUnit.Framework;
using System;

namespace UnitTests.Shared
{
    [TestFixture]
    public class CheckTests
    {
        [Test]
        [TestCase(Check.Roll.Ability, ExpectedResult = "ATTR")]
        [TestCase(Check.Roll.Regenerate, ExpectedResult = "REGENERATE")]
        [TestCase(Check.Roll.Dodge, ExpectedResult = "DO")]
        [TestCase(Check.Roll.Initiative, ExpectedResult = "INI")]
        public string Check_BasicRollCheck_IdIsValid(Check.Roll action)
        {
            // Arrange
            var check = new Check(action);

            // Act
            string result = check;

            // Assert
            Assert.AreEqual(result, check.Id); // additional test
            return result;
        }



        [Test]
        [TestCase(Check.Combat.Attack, "CT_9", null, ExpectedResult = "CT_9/AT")]
        [TestCase(Check.Combat.Parry, "CT_9", null, ExpectedResult = "CT_9/PA")]
        [TestCase(Check.Combat.Attack, "CT_9", "SA_124", ExpectedResult = "CT_9/AT/SA_124")]
        public string Check_CombatCheck_IdIsValid(Check.Combat action, string CombatTec, string CombatStyle)
        {
            // Arrange
            var check = new Check(action, CombatTec, CombatStyle);

            // Act
            string result = check;

            // Assert
            Assert.AreEqual(result, check.Id); // additional test
            return result;
        }


        [Test]
        [TestCase(Check.Combat.Attack, "XY_9", null)]
        [TestCase(Check.Combat.Parry, "CT_9", "ZZ_888")]
        public void Check_CombatCheck_IdIsInvalid(Check.Combat action, string CombatTec, string CombatStyle)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var check = new Check(action, CombatTec, CombatStyle);
            });
        }
    }
}
