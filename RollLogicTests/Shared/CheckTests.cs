using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Shared
{
    [TestFixture]
    public class CheckTests
    {
        [Test, Description("enum values shall not overlap")]
        public void VerifyEnumTypes()
        {
            // Arrange
            List<int> rolls = new((int[])Enum.GetValues(typeof(Check.Roll)));
            List<int> skills = new((int[])Enum.GetValues(typeof(Check.Skill)));
            List<int> combats = new((int[])Enum.GetValues(typeof(Check.Combat)));
            // Act
            // Assert
            Assert.That(rolls.Intersect(skills).Count(), Is.Zero);
            Assert.That(rolls.Intersect(combats).Count(), Is.Zero);
            Assert.That(skills.Intersect(combats).Count(), Is.Zero);
        }


        [Test]
        [TestCase(Check.Roll.Ability, ExpectedResult = true)]
        [TestCase(Check.Roll.Regenerate, ExpectedResult = true)]
        [TestCase(Check.Roll.Dodge, ExpectedResult = true)]
        [TestCase(Check.Roll.Initiative, ExpectedResult = true)]
        public bool Check_Is_Roll(Check.Roll action) 
        {
            // Arrange
            var check = new Check(action);

            // Act
            bool result = check.Is(action);

            // Assert
            return result;
        }




        [Test]
        [TestCase(Check.Skill.Skill, ExpectedResult = true)]
        [TestCase(Check.Skill.Arcane, ExpectedResult = true)]
        [TestCase(Check.Skill.Karma, ExpectedResult = true)]
        public bool Check_Is_Skill(Check.Skill action)
        {
            // Arrange
            var check = new Check(action);

            // Act
            bool result = check.Is(action);

            // Assert
            return result;
        }






        [Test]
        [TestCase(Check.Combat.Attack, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, ExpectedResult = true)]
        // check for combinations
        public bool Is_Combat(Check.Combat action, CombatBranch branch)
        {
            // Arrange
            var check = new Check(action, ChrAttrId.CombatTecBaseId, branch);

            // Act
            bool result = check.Is(action);

            // Assert
            return result;
        }

        [Test]
        [TestCase(Check.Combat.Attack, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, ExpectedResult = true)]
        // check for combinations
        public bool Is_IsAnyCombat_CheckSpecific(Check.Combat action, CombatBranch branch)
        {
            // Arrange
            var check = new Check(Check.Combat.Any, ChrAttrId.CombatTecBaseId, branch);

            // Act
            bool result = check.Is(action);

            // Assert
            return result;
        }


        [Test]
        [TestCase(Check.Combat.Attack, 0, ExpectedResult = false)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = false)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, ExpectedResult = false)]
        [TestCase(Check.Combat.Any, 0, ExpectedResult = true)]
        // check for combinations
        public bool Is_IsSpecificCombat_CheckAny(Check.Combat action, CombatBranch branch)
        {
            // Arrange
            var check = new Check(action, ChrAttrId.CombatTecBaseId, branch);

            // Act
            bool result = check.Is(Check.Combat.Any);

            // Assert
            return result;
        }


        [Test]
        [TestCase(Check.Combat.Attack, 0, ExpectedResult = false)]
        [TestCase(Check.Combat.Parry, 0, ExpectedResult = false)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, ExpectedResult = true)]
        // check for combinations
        public bool Check_Is_Branch(Check.Combat action, CombatBranch branch)
        {
            // Arrange
            var check = new Check(action, ChrAttrId.CombatTecBaseId, branch);

            // Act
            bool result = check.Is(branch);

            // Assert
            return result;
        }

        [Test]
        [TestCase(Check.Combat.Attack, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, 0, ExpectedResult = true)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = true)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, ExpectedResult = true)]
        // check for combinations
        public bool Check_Is_CombatBranch(Check.Combat action, CombatBranch branch)
        {
            // Arrange
            var check = new Check(action, ChrAttrId.CombatTecBaseId, branch);

            // Act
            bool result = check.Is(action, branch);

            // Assert
            return result;
        }


        [Test]
        [TestCase(Check.Combat.Attack, CombatBranch.Unspecififed, Check.Combat.Attack, CombatBranch.Melee, ExpectedResult = false)]
        [TestCase(Check.Combat.Parry, CombatBranch.Unspecififed, Check.Combat.Attack, CombatBranch.Unspecififed, ExpectedResult = false)]
        [TestCase(Check.Combat.Parry, CombatBranch.Unspecififed, Check.Combat.Attack, CombatBranch.Ranged, ExpectedResult = false)]
        [TestCase(Check.Combat.Attack, CombatBranch.Melee, Check.Combat.Attack, CombatBranch.Unarmed, ExpectedResult = false)]
        [TestCase(Check.Combat.Parry, CombatBranch.Shield, Check.Combat.Parry, CombatBranch.Ranged, ExpectedResult = false)]
        public bool Check_Is_Not_CombatBranch(
            Check.Combat initAction, CombatBranch initBranch, Check.Combat checkAction, CombatBranch checkBranch)
        {
            // Arrange
            var check = new Check(initAction, ChrAttrId.CombatTecBaseId, initBranch);

            // Act
            bool result = check.Is(checkAction, checkBranch);

            // Assert
            return result;
        }


        #region id strings

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
            Assert.That(result, Is.EqualTo(check.Id)); // additional test
            return result;
        }




        [Test]
        [TestCase(Check.Combat.Attack, "CT_9", null, ExpectedResult = "CT_9/AT")]
        [TestCase(Check.Combat.Parry, "CT_9", null, ExpectedResult = "CT_9/PA")]
        [TestCase(Check.Combat.Attack, "CT_9", "SA_124", ExpectedResult = "CT_9/AT/SA_124")]
        public string Check_CombatCheck_IdIsValid(Check.Combat action, string CombatTec, string CombatStyle)
        {
            // Arrange
            var check = new Check(action, CombatTec, combatStyle: CombatStyle);

            // Act
            string result = check;

            // Assert
            Assert.That(result, Is.EqualTo(check.Id)); // additional test
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
                var check = new Check(action, CombatTec, combatStyle: CombatStyle);
            });
        }

        #endregion
    }
}
