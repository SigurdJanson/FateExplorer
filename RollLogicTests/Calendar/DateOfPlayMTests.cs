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
            Assert.AreEqual(DateTime.Today, result.Date);
        }

        [Test]
        public void SetDate()
        {
            const int day = 24, month = 12, year = 2020;
            // Arrange
            var dateOfPlayM = CreateDateOfPlayM();
            mockClientSideStorage.SetupAsync(c => c.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()));

            // Act
            dateOfPlayM.Date = new DateTime(year, month, day);

            // Assert
            Assert.AreEqual(day, dateOfPlayM.Date.Day);
            Assert.AreEqual(month, dateOfPlayM.Date.Month);
            Assert.AreEqual(year, dateOfPlayM.Date.Year);
            mockClientSideStorage.Verify(c => c.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()), Times.Once);
        }



        [Test]
        public async Task RestoreSavedState_CallingStorage()
        {
            const int day = 24, month = 12, year = 2020;
            // Arrange
            var dateOfPlayM = this.CreateDateOfPlayM(); // create with date = today
            var ExpectedDate = new DateTime(year, month, day);
            mockClientSideStorage.SetupAsync(c => c.Retrieve(
                It.Is<string>(s => s == dateOfPlayM.GetType().ToString()), 
                It.IsAny<string>()) )
                .Returns(ExpectedDate.ToString());

            // Act
            await dateOfPlayM.RestoreSavedState(); // restore date from the past

            // Assert
            Assert.AreEqual(ExpectedDate, dateOfPlayM.Date);
            this.mockRepository.VerifyAll();
        }
    }
}
