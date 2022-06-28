using FateExplorer.Calendar;
using FateExplorer.GameData;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UnitTests.Calendar
{
    [TestFixture]
    public class CalendarViMoTests
    {
        static IEnumerable<string> ValidStringDates()
        {
            yield return "4. Tsa 1028";
            yield return "4. TSA 1028";
            yield return "4. tsA 1028";
            yield return "4. tsa 1028";
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

        static IEnumerable<string> ValidBeforeFBStringDates()
        {
            yield return "11 Rondra654 v. BF";
            yield return "    27. Efferd 654 vBF";
            yield return " 27.Efferd 654  v   BF   ";
            yield return "4. Tsa 654 V.bf";
            yield return "4. Tsa 1 v. BF";
            yield return "5. Boron 28 v.BF";
            yield return "5. Boron 28v.BF";
        }

        static IEnumerable<string> InvalidRangeStringDates()
        {
            yield return "4. Tsa -977"; // latest invalid year
            yield return " 0. Efferd 12 BF"; // invalid day
            yield return " 31. Efferd 12 BF"; // invalid day
            yield return " 0. Namenlos 12";   // invalid day
            yield return " 6. Namenlos 12";   // invalid day
        }
        static IEnumerable<string> InvalidNameStringDates()
        {
            yield return " 2. MonthWithoutAName 1045";   // invalid month
        }

        private MockRepository mockRepository;
        private Mock<IDateOfPlay> mockDateOfPlay;
        CalendarDB CalendarData;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            mockDateOfPlay = mockRepository.Create<IDateOfPlay>();
            mockDateOfPlay.SetupProperty(c => c.Date, DateTime.Now);

            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"calendar_de.json"));
            string jsonString = File.ReadAllText(fileName);
            CalendarData = JsonSerializer.Deserialize<CalendarDB>(jsonString);
        }


        private CalendarViMo CreateCalendarViMo()
        {
            return new CalendarViMo(CalendarData, mockDateOfPlay.Object);
        }



        [Test]
        public void GotoTomorrow()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime Before = calendarViMo.EffectiveDate;

            // Act
            calendarViMo.GotoTomorrow();

            // Assert
            Assert.Greater(calendarViMo.EffectiveDate, Before);
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void GotoYesterday()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime After = calendarViMo.EffectiveDate;

            // Act
            calendarViMo.GotoYesterday();

            // Assert
            Assert.Less(calendarViMo.EffectiveDate, After);
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void GotoEarthDate()
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            calendarViMo.GotoYesterday();

            // Act
            Assume.That(calendarViMo.EffectiveDate.Date, Is.Not.EqualTo(DateTime.Today));
            calendarViMo.GotoEarthDate();

            // Assert
            Assert.AreEqual(DateTime.Today, calendarViMo.EffectiveDate.Date);
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
        [TestCaseSource(nameof(ValidBeforeFBStringDates))]
        public void Parse_ValidDateTime_BeforeFB(string dateStr)
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime result = DateTime.MinValue;

            // Act
            Assert.DoesNotThrow(() => result = calendarViMo.Parse(dateStr));

            // Assert
            Assert.Greater(result, DateTime.MinValue);
            Assert.Less(result.Year, 977);
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
        [TestCaseSource(nameof(InvalidNameStringDates))]
        public void Parse_InvalidNames_FormatException(string dateStr)
        {
            // Arrange
            var calendarViMo = this.CreateCalendarViMo();
            DateTime result = DateTime.MinValue;

            // Act
            Assert.Throws<FormatException>(() => result = calendarViMo.Parse(dateStr));

            // Assert
            mockRepository.VerifyAll();
        }
    }
}
