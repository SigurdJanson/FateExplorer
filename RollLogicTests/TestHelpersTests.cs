using NUnit.Framework;
using RollLogicTests;
using System;

namespace RollLogicTests
{
    [TestFixture]
    public class TestHelpers_Tests
    {
        private class SimpleType
        {
            public int I { get; set; }
            public string Name { get; set; }
            public DateTime DateTime { get; set; }
        }


        private class ComplexType : SimpleType
        {
            public SimpleType Simple1 { get; set; }
        }



        #region SIMPLE TYPES -----

        [Test]
        public void IsTier1Equal_SimpleTypesAllEqual_ReturnsTrue()
        {
            // Arrange
            string TestString = "Fate is Fate";
            SimpleType self = new()
            {
                I = 1, Name = TestString, DateTime = DateTime.Today
            };
            SimpleType to = new()
            {
                I = 1, Name = TestString, DateTime = DateTime.Today
            };

            // Act
            var result = TestHelpers.IsTier1Equal(self, to);

            // Assert
            Assert.IsTrue(result);
        }



        [Test]
        public void IsTier1Equal_SimpleTypes_UnequalStringRefButSameString_ReturnsTrue()
        {
            // Arrange
            const string TestString = "Fate is Fate";
            string a = new(TestString);
            string b = TestString;
            SimpleType self = new()
            {
                I = 1,
                Name = a,
                DateTime = DateTime.Today
            };
            SimpleType to = new()
            {
                I = 1,
                Name = b,
                DateTime = DateTime.Today
            };
            Assume.That(!ReferenceEquals(self.Name, to.Name));

            // Act
            var result = TestHelpers.IsTier1Equal(self, to);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void IsTier1Equal_SimpleTypesUnequal_ReturnsFalse()
        {
            // Arrange
            string TestString = "Fate is Fate";
            SimpleType self = new()
            {
                I = 1, Name = TestString, DateTime = DateTime.Today
            };
            SimpleType to = new()
            {
                I = 1, Name = TestString, DateTime = DateTime.Today.AddTicks(1)
            };

            // Act
            var result = TestHelpers.IsTier1Equal(self, to);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public void IsTier1Equal_ComplexContentsEqual_ReturnsFalse()
        {
            // Arrange
            string TestString = "Fate is Fate";
            ComplexType self = new()
            {
                I = 1,
                Name = TestString,
                DateTime = DateTime.Today
            };
            ComplexType to = new()
            {
                I = 1,
                Name = TestString,
                DateTime = DateTime.Today,
                Simple1 = self
            };
            self.Simple1 = to;

            // Act
            var result = TestHelpers.IsTier1Equal(self, to);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion ---------------





        #region COMPLEX TYPES ----

        [Test]
        public void IsDeeplyEqual_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string TestString = "Fate is Fate";
            ComplexType self = new()
            {
                I = 1,
                Name = TestString,
                DateTime = DateTime.Today
            };
            ComplexType to = new()
            {
                I = 1,
                Name = TestString,
                DateTime = DateTime.Today,
                Simple1 = self
            };
            self.Simple1 = to;

            // Act
            var result = TestHelpers.IsDeeplyEqual(self, to);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion ---------------
    }
}
