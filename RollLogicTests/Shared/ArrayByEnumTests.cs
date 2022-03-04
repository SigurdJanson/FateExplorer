using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Linq;

namespace vmCode_UnitTests.Shared
{
    [TestFixture]
    public class ArrayByEnumTests
    {
        enum TestEnum { Zero = 0, One = 1, Two = 2, Three = 3, Four = 4, Five = 5 }



        [Test]
        public void IndexOperator()
        {
            // Arrange
            var arrayByEnum = new ArrayByEnum<string, TestEnum>();
            var enumValues = Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>();

            // Act
            foreach (TestEnum e in enumValues)
            {
                arrayByEnum[e] = $"{(int)e}";
            }

            // Assert
            Assert.AreEqual(new string[] { "0", "1", "2", "3", "4", "5" }, arrayByEnum);
        }



        [Test]
        public void Iterator()
        {
            // Arrange
            var arrayByEnum = new ArrayByEnum<string, TestEnum>();
            arrayByEnum[TestEnum.Zero] = "0000000000";
            arrayByEnum[TestEnum.One] = "1";
            arrayByEnum[TestEnum.Two] = "02";
            arrayByEnum[TestEnum.Three] = "003";
            arrayByEnum[TestEnum.Four] = "FooooUUUURRR";
            arrayByEnum[TestEnum.Five] = "00005";

            // Act
            foreach (var e in arrayByEnum)
            {
                Assume.That(e is not null);
            }

            // Assert
            
        }
    }
}
