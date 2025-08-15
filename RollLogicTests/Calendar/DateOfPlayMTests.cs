using FateExplorer.Calendar;
using FateExplorer.Shared.ClientSideStorage;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class DateOfPlayMTests
    {
        private MockRepository mockRepository;

        private Mock<IClientSideStorage> mockClientSideStorage;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockClientSideStorage = mockRepository.Create<IClientSideStorage>();
        }

        private DateOfPlayM CreateDateOfPlayM()
        {
            return new DateOfPlayM(mockClientSideStorage.Object);
        }


        [Test]
        public void GetDate()
        {
            // Arrange
            var dateOfPlayM = CreateDateOfPlayM();

            // Act
            var result = dateOfPlayM.Date;

            // Assert
            Assert.That(DateTime.Today, Is.EqualTo(result.Date));
        }

        [Test]
        public void SetDate()
        {
            const int day = 24, month = 12, year = 2020;
            // Arrange
            var dateOfPlayM = CreateDateOfPlayM();
            mockClientSideStorage.Setup(c => c.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(Task.CompletedTask);

            // Act
            dateOfPlayM.Date = new DateTime(year, month, day);

            // Assert
            Assert.That(day, Is.EqualTo(dateOfPlayM.Date.Day));
            Assert.That(month, Is.EqualTo(dateOfPlayM.Date.Month));
            Assert.That(year, Is.EqualTo(dateOfPlayM.Date.Year));
            mockClientSideStorage.Verify(c => c.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()), Times.Once);
        }



        [Test]
        public async Task RestoreSavedState_CallingStorage()
        {
            const int day = 24, month = 12, year = 2020;
            // Arrange
            var dateOfPlayM = this.CreateDateOfPlayM(); // create with date = today
            var ExpectedDate = new DateTime(year, month, day);
            mockClientSideStorage.Setup(c => c.Retrieve(
                It.Is<string>(s => s == dateOfPlayM.GetType().ToString()), 
                It.IsAny<string>()) )
                .Returns(Task.FromResult(ExpectedDate.ToString()));

            // Act
            await dateOfPlayM.RestoreSavedState(); // restore date from the past

            // Assert
            Assert.That(ExpectedDate, Is.EqualTo(dateOfPlayM.Date));
            this.mockRepository.VerifyAll();
        }
    }
}
