using FateExplorer.GameLogic;
using Moq;
using NUnit.Framework;

namespace RollLogicTests.GameLogic
{
    [TestFixture]
    public class ResilienceMTests
    {
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
        }


        private ResilienceM CreateResilianceMByBaseValue(int Base, int Extra, string[] Abs)
        {
            return new ResilienceM(
                this.mockCharacterM.Object, Base, Extra, Abs);
        }


        private ResilienceM CreateResilianceMByValue(int Value, string[] Abs)
        {
            return new ResilienceM(
                this.mockCharacterM.Object, Value, Abs);
        }


        [Test]
        [TestCase("ATTR_1", 13, "ATTR_2", 15, "ATTR_3", 13, 2)]
        [TestCase("ATTR_7", 10, "ATTR_7", 10, "ATTR_8", 12, 0)]
        public void Constructor_ByValue_Correct(string ab1, int ab1v, string ab2, int ab2v, string ab3, int ab3v, int Value)
        {
            const int JunisBaseValue = -5;
            // Arrange
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab1)))
                .Returns(ab1v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab2)))
                .Returns(ab2v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab3)))
                .Returns(ab3v);


            // Act
            var resilianceM = this.CreateResilianceMByValue(Value, new string[3] { ab1, ab2, ab3 });

            // Assert
            Assert.AreEqual(Value, resilianceM.Value);
            Assert.AreEqual(JunisBaseValue, resilianceM.BaseValue);
            Assert.AreEqual(0, resilianceM.ExtraValue);

            this.mockRepository.VerifyAll();
        }


        [Test]
        [TestCase("ATTR_1", 13, "ATTR_2", 15, "ATTR_3", 13, -5, 1, 3)]
        [TestCase("ATTR_7", 10, "ATTR_7", 10, "ATTR_8", 12, -5, 0, 0)]
        public void Constructor_ByBaseValue_Correct(string ab1, int ab1v, string ab2, int ab2v, string ab3, int ab3v, int BaseValue, int Extra, int Target)
        {
            // Arrange
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab1)))
                .Returns(ab1v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab2)))
                .Returns(ab2v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab3)))
                .Returns(ab3v);


            // Act
            var resilianceM = this.CreateResilianceMByBaseValue(BaseValue, Extra, new string[3] { ab1, ab2, ab3 });

            Assume.That(resilianceM.BaseValue, Is.EqualTo(BaseValue));
            Assume.That(resilianceM.ExtraValue, Is.EqualTo(Extra));

            // Assert
            Assert.AreEqual(Target, resilianceM.Value);
            this.mockRepository.VerifyAll();
        }
    }
}
