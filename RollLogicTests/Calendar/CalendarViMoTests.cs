using FateExplorer.Calendar;
using FateExplorer.GameData;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class CalendarViMoTests
    {
        static IEnumerable<string> ValidStringDates()
        {
            yield return "4. Tsa 1028";
            yield return "22. Boron 1045";
            yield return "22. Bor 1045";
            yield return "22. B 1045";
            yield return "11 Rondra 998";
            yield return "11 Rondra998";
            yield return "    27. Efferd 12";
            yield return " 27.Efferd 12  BF   ";
            yield return " 27.Efferd 12  Bf   ";
            yield return " 27.Efferd 12  bF   ";
            yield return " 27.Efferd 12  bf   ";
            yield return "4Tsa1028";
            yield return "4. Tsa 0";
            yield return "4. Tsa -976"; // earliest possible year
        }

        static IEnumerable<string> InvalidRangeStringDates()
        {
            yield return "4. Tsa -977"; // latest invalid year
            yield return " 31. Efferd 12 BF";
            yield return " 0. Namenlos 12";
        }

        private MockRepository mockRepository;
        private Mock<CalendarDB> mockCalendarDB;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }


        private CalendarViMo CreateCalendarViMo()
        {
            return new CalendarViMo(mockCalendarDB.Object);
        }



        [Test]
        public void GotoTomorrow_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();

            // Act
            calendarViMo.GotoTomorrow();

            // Assert
            //Assert.AreEqual();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void GotoYesterday_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();

            // Act
            calendarViMo.GotoYesterday();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void GotoEarthDate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();

            // Act
            calendarViMo.GotoEarthDate();

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }



        [Test]
        [TestCaseSource(nameof(ValidStringDates))]
        public void Parse_ValidDateTime(string dateStr)
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime result = DateTime.MinValue;

            // Act
            Assert.DoesNotThrow(() => result = calendarViMo.Parse(dateStr));

            // Assert
            Assert.Greater(result, DateTime.MinValue);
            mockRepository.VerifyAll();
        }


        [Test]
        [TestCaseSource(nameof(InvalidRangeStringDates))]
        public void Parse_InvalidRange_Exception(string dateStr)
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime result = DateTime.MinValue;

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => result = calendarViMo.Parse(dateStr));

            // Assert
            mockRepository.VerifyAll();
        }

        [Test]
        [TestCaseSource(nameof(InvalidRangeStringDates))]
        public void Parse_InvalidMonthName_Exception(string dateStr)
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime result = DateTime.MinValue;

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => result = calendarViMo.Parse(dateStr));

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
