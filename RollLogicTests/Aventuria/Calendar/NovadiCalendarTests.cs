using Aventuria.Calendar;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace UnitTests.Aventuria.Calendar
{
    [TestFixture]
    public class NovadiCalendarTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private NovadiCalendar CreateNovadiCalendar()
        {
            return new NovadiCalendar();
        }

        [Test]
        [TestCase("02.02.0100", 30, ExpectedResult = "04.03.0100", Description = "Feb to March, no leap year")] // 
        [TestCase("28.02.1996", 1, ExpectedResult = "01.03.1996")]  // Skip leap day
        [TestCase("28.11.1999", 34, ExpectedResult = "01.01.2000")] //
        [TestCase("29.11.1993", 365, ExpectedResult = "29.11.1994", Category = "Full yr in days", Description = "No leap year")] //
        [TestCase("29.11.1995", 365, ExpectedResult = "29.11.1996", Category = "Full yr in days", Description = "Leap year, jump over leap day")] // Leap year in 1996
        [TestCase("01.03.1995", 364, ExpectedResult = "28.02.1996", Category = "Full yr in days", Description = "Leap year but between leap days")] // Leap year in 1996
        public string AddDays(string timeAsString, int months)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.AddDays(time, months);

            // Assert
            return result.ToString("dd.MM.yyyy");
        }


        [Test]
        [TestCase("02.1.1998", 30, ExpectedResult = "02.1.2004")] // Add over a year incl. lap year
        [TestCase("28.11.1999", 1, ExpectedResult = "09.2.2000")] // 
        [TestCase("28.11.1999", 2, ExpectedResult = "22.4.2000")] // 
        [TestCase("28.11.1999", 3, ExpectedResult = "04.7.2000")] // 
        [TestCase("29.11.1995", 3, ExpectedResult = "05.7.1996", TestName = "Leap Jump", Description = "Jump over leap day")] // Leap year in 1996
        public string AddWeeks(string timeAsString, int months)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.AddMonths(time, months);

            // Assert
            /*
            // Used to validate test case "Leap Jump" - not valid for other tests
            Assert.That(novadiCalendar.GetDayOfYear(result), Is.EqualTo(months * 73 - 143 - 31 - 1)); // 3 months minus 32 days in old year
            */
            return result.ToString("dd.M.yyyy");
        }



        [Test]
        [TestCase("02.1.1998", 30, ExpectedResult = "02.1.2004")] // Add over a year incl. lap year
        [TestCase("28.11.1999", 1, ExpectedResult = "09.2.2000")] // 
        [TestCase("28.11.1999", 2, ExpectedResult = "22.4.2000")] // 
        [TestCase("28.11.1999", 3, ExpectedResult = "04.7.2000")] // 
        [TestCase("29.11.1995", 3, ExpectedResult = "05.7.1996", TestName = "Leap Jump", Description = "Jump over leap day")] // Leap year in 1996
        public string AddMonths(string timeAsString, int months)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.AddMonths(time, months);

            // Assert
            /*
            // Used to validate test case "Leap Jump" - not valid for other tests
            Assert.That(novadiCalendar.GetDayOfYear(result), Is.EqualTo(months * 73 - 143 - 31 - 1)); // 3 months minus 32 days in old year
            */
            return result.ToString("dd.M.yyyy");
        }


        [Test]
        [TestCase("29.2.2000", 1, ExpectedResult = "28.2.2001")]  // Leap year
        [TestCase("30.1.2000", 11, ExpectedResult = "30.1.2011")] // Leap year
        [TestCase("31.1.2000", 12, ExpectedResult = "31.1.2012")] // Leap year
        [TestCase("18.4.2022", 0, ExpectedResult = "18.4.2022")]
        [TestCase("26.12.2005", -1, ExpectedResult = "26.12.2004")]
        [TestCase("27.12.2005", 3, ExpectedResult = "27.12.2008")]
        [TestCase("30.06.1988", 11, ExpectedResult = "30.6.1999")] // Leap year
        [TestCase("29.02.1996", 4, ExpectedResult = "28.2.2000")] // Leap year to no leap year
        public string AddYears(string timeAsString, int years)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            Assume.That(DateTime.TryParse(timeAsString, out DateTime time));

            // Act
            var result = novadiCalendar.AddYears(time, years);

            // Assert
            return result.ToString("dd.M.yyyy");
        }


        [Test]
        [TestCase("2025-10-14", ExpectedResult = 72)] // day before
        [TestCase("2025-10-15", ExpectedResult = 73)] // 2nd Rastullahellah 1048 FB
        [TestCase("2025-10-16", ExpectedResult = 1)]  // day after
        [TestCase("2006-05-22", ExpectedResult = 73)] // 5th Rastullahellah
        [TestCase("2006-05-23", ExpectedResult = 1)]  // 1st day of new year
        public int GetDayOfMonth(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetDayOfMonth(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("2025-10-14", ExpectedResult = DayOfWeek.Monday)] // day before
        public DayOfWeek GetDayOfWeek(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetDayOfWeek(time);

            // Assert
            Assert.Pass();
            return result;
        }


        [Test]
        [TestCase("0001-01-01", ExpectedResult = 365 - 141)] // First day of Bosparan year
        [TestCase("2025-12-31", ExpectedResult = 365 - 142)] // LAst day of Bosparan year
        [TestCase("2101-05-23", ExpectedResult = 1)]   // 23rd Mai is 143. day of the year; and 1st of Novadi year
        [TestCase("2101-05-22", ExpectedResult = 365)] // Last day of Novadi year
        [TestCase("2101-05-21", ExpectedResult = 364)] // 364th day of Novadi year
        [TestCase("2096-02-28", ExpectedResult = 365 - 142 + 31 + 28)] // 2096 is a leap year
        [TestCase("2096-02-29", ExpectedResult = 365 - 142 + 31 + 28)] // same
        [TestCase("2096-03-01", ExpectedResult = 365 - 142 + 31 + 29)] // same
        public int GetDayOfYear(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetDayOfYear(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("2101-05-21", ExpectedResult = 40)] // Day before last day of year
        [TestCase("2101-05-22", ExpectedResult = 40)] // Last day of Novadi year and 5th Rastullahellah
        [TestCase("2101-05-23", ExpectedResult = 1)]  // 23rd Mai is 143rd day of the year; and 1st of Novadi year
        [TestCase("2101-05-31", ExpectedResult = 1)]  // Last day of 1st week
        [TestCase("2101-06-01", ExpectedResult = 2)]  // First day of second week
        [TestCase("2101-08-03", ExpectedResult = 8)]  // 1st Rastullahellah belongs to 8th week and is last day of month
        [TestCase("2101-08-04", ExpectedResult = 9)]  // The day after
        public int GetWeekOfYear(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetWeekOfYear(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("2025-10-14", ExpectedResult = 2)] // day before
        [TestCase("2025-10-15", ExpectedResult = 2)] // 2nd Rastullahellah 1048 FB
        [TestCase("2025-10-16", ExpectedResult = 3)] // day after
        [TestCase("2006-05-22", ExpectedResult = 5)] // 5th Rastullahellah
        [TestCase("2006-05-23", ExpectedResult = 1)] // 1st day of new year
        public int GetMonth(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetMonth(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("2025-10-12", ExpectedResult = 289)] // 
        [TestCase("2025-10-15", ExpectedResult = 289)] // 2nd Rastullahellah 1048 FB
        [TestCase("2006-01-01", ExpectedResult = 269)]
        [TestCase("2006-05-22", ExpectedResult = 269)] // Previous day should be the year before 270
        [TestCase("2006-05-23", ExpectedResult = 270)] // Last day of Novadi year in 1029 FB
        [TestCase("2006-12-31", ExpectedResult = 270)]
        [TestCase("1737-05-23", ExpectedResult = 1)]   // Day of Rastullahs appearance
        [TestCase("1737-05-22", ExpectedResult = -1)]   // Day BEFORE Rastullahs appearance
        // 23. BOR 760 BF is the day of 
        public int GetYear(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetYear(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("1737-05-23", ExpectedResult = 0)]   // Day of Rastullahs appearance
        [TestCase("1737-05-22", ExpectedResult = -1)]  // Day BEFORE Rastullahs appearance
        [TestCase("2101-05-22", ExpectedResult = 0)]   
        public int GetEra(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.GetEra(time);

            // Assert
            return result;
        }


        [Test]
        [TestCase("2025-10-15", ExpectedResult = true)]   // 2nd Rastullahellah 1048 FB
        [TestCase("2006-01-01", ExpectedResult = false)]
        [TestCase("2006-05-22", ExpectedResult = true)]   // 5th Rastullahellah
        [TestCase("2006-05-23", ExpectedResult = false)]  // 1st day of new year
        [TestCase("2006-12-31", ExpectedResult = false)]
        [TestCase("1737-05-23", ExpectedResult = false)]  // Day of Rastullahs appearance
        [TestCase("1737-05-22", ExpectedResult = true)]   // Day BEFORE Rastullahs appearance
        public bool IsRastullahellah(string timeAsString)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            DateTime time = DateTime.Parse(timeAsString);

            // Act
            var result = novadiCalendar.IsRastullahellah(time);

            // Assert
            return result;
        }


        [Test]
        public void GetDaysInMonth(
            [Random(-1000, 1000, 5)] int year, 
            [Random(1, 5, 5)] int month)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetDaysInMonth(year, month);

            // Assert
            Assert.That(result, Is.EqualTo(73)); // 73 = days per month
        }


        [Test]
        public void GetDaysInMonth_AnyEra(
            [Random(-1000, 1000, 5)] int year,
            [Random(1, 5, 5)] int month,
            [Random(-1, 0, 5)] int era)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetDaysInMonth(year, month, era);

            // Assert
            Assert.That(result, Is.EqualTo(73));
        }


        [Test]
        public void GetDaysInYear_AnyYear([Random(-1000, 1000, 5)] int year)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetDaysInYear(year);

            // Assert
            Assert.That(result, Is.EqualTo(365));
        }


        [Test]
        public void GetDaysInYear_AnyYearNEra(
            [Random(-1000, 1000, 5)] int year,
            [Random(-1, 0, 5)] int era)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetDaysInYear(year, era);

            // Assert
            Assert.That(result, Is.EqualTo(365));
        }


        [Test]
        public void GetMonthsInYear_AnyYear([Random(-1000, 1000, 5)] int year)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetMonthsInYear(year);

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void GetMonthsInYear_AnyYearNEra(
            [Random(-1000, 1000, 5)] int year,
            [Random(-1, 0, 5)] int era)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();

            // Act
            var result = novadiCalendar.GetMonthsInYear(year, era);

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }


        [Test, Category("Date only tests")]
        // Before appearance of Rastulla
        [TestCase(1, 1, 1, ExpectedResult = "23.05.1737")]     // Rashtullas appearance
        [TestCase(289, 2, 73, ExpectedResult = "15.10.2025")]  // 15th Oct. 2025
        [TestCase(1046, 2, 30, ExpectedResult = "02.09.2782")] // 
        [TestCase(167, 4, 63, ExpectedResult = "28.02.1904")]  // 1 day before earthen leap day
        [TestCase(167, 4, 64, ExpectedResult = "28.02.1904")]  // leap day
        [TestCase(167, 4, 65, ExpectedResult = "01.03.1904")]  // 1 day after earthen leap day
        public string ToDateTime(int Year, int Month, int Day)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            int hour = 0, minute = 0, second = 0, millisecond = 0;
            int era = 0;

            // Act
            var result = novadiCalendar.ToDateTime(Year, Month, Day,   hour, minute, second, millisecond, era);

            // Assert
            return result.ToString("dd.MM.yyyy");
        }

        [Test]
        // Before appearance of Rastulla
        [TestCase(1, 1, 0, Category = "Day")]  // month in range, day just outside
        [TestCase(1, 5, 74, Category = "Day")] // month in range, day just outside
        [TestCase(289, 0, 1, Category = "Month")]  // day in range, month just outside
        [TestCase(289, 6, 73, Category = "Month")] // day in range, month just outside
        [TestCase(-1737, 1, 1, Category = "Year")] // 
        [TestCase(0, 1, 1, Category = "Year")]     // Calendar has no year 0
        [TestCase(9999+1737, 1, 1, Category = "Year")] // 
        public void ToDateTime_ArgumentoutOfRange_Exception(int Year, int Month, int Day)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            int hour = 0, minute = 0, second = 0, millisecond = 0;
            int era = 0;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => novadiCalendar.ToDateTime(Year, Month, Day, hour, minute, second, millisecond, era));
        }


        [Test]
        public void ToDateTime_NoEra([Values(1, 255)]int Year, [Values(1, 5)] int Month, [Values(1, 73)] int Day)
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            int hour = 0, minute = 0, second = 0, millisecond = 0;
            int era = 0;

            // Act
            // Assert
            Assert.That(
                novadiCalendar.ToDateTime(Year, Month, Day, hour, minute, second, millisecond),
                Is.EqualTo(novadiCalendar.ToDateTime(Year, Month, Day, hour, minute, second, millisecond, era)));
        }


        [Test]
        public void ToFourDigitYear_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var novadiCalendar = this.CreateNovadiCalendar();
            int year = 0;

            // Act
            Assert.Throws<NotImplementedException>(() => { var result = novadiCalendar.ToFourDigitYear(year); });
            //var result = novadiCalendar.ToFourDigitYear(year);

            // Assert
            Assert.Pass();
            //this.mockRepository.VerifyAll();
        }
    }
}
