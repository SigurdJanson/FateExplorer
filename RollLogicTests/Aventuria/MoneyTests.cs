using Aventuria;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria
{
    [TestFixture]
    public class MoneyTests
    {


        [SetUp]
        public void SetUp()
        {}


        public static object[] ToCurrencyTestCase =
        {
            new object[] { 13.2m,  Currency.DwarvenThaler,   1.1m },
            new object[] { -12.0m, Currency.DwarvenThaler,  -1.0m },
            new object[] { 0.0m,   Currency.DwarvenThaler,   0.0m },
            new object[] { 13.2m,  Currency.NostrianCrown,   13.2m/5 }
        };

        [Test, TestCaseSource(nameof(ToCurrencyTestCase))]
        public void ToCurrency_IdenticalCurrency_IdenticalValue(decimal m, Currency rc, decimal r)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = money.ToCurrency(rc);

            // Assert
            Assert.That(result, Is.EqualTo(new Money(r, rc) ));
        }

        [Test]
        [TestCase("13.2", ExpectedResult = "1.1")]
        [TestCase("0.0", ExpectedResult = "0.0")]
        [TestCase("-12.0", ExpectedResult = "-1.0")]
        public decimal ToCurrencyValue(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = money.ToCurrencyValue(Currency.DwarvenThaler);

            // Assert
            return result;
        }


        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = 0)]
        [TestCase("0.0", "1.0", ExpectedResult = -1)]
        [TestCase("1.0", "0.0", ExpectedResult = 1)]
        [TestCase("0.0", "-0.0", ExpectedResult = 0)]
        public int CompareTo(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            decimal value = v;

            // Act
            var result = money.CompareTo(value);

            // Assert
            return result;
        }



        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "0.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = true)]
        [TestCase("-1.0", "-1.0", ExpectedResult = true)]
        [TestCase("-0.0", "-0.0", ExpectedResult = true)]
        [TestCase("-1.0", "0.0", ExpectedResult = false)]
        public bool Equals_MoneyAsObject_SameCurrency_TrueOrFalse(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            Money val = new(v, Currency.ReferenceCurrency);

            // Act
            var result = money.Equals((object)val);

            // Assert
            return result;
        }
        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = false)]
        public bool Equals_NoMoneyObject_False(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            
            // Act
            var result = money.Equals(v);

            // Assert
            return result;
        }



        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "0.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = true)]
        [TestCase("-1.0", "-1.0", ExpectedResult = true)]
        [TestCase("-0.0", "-0.0", ExpectedResult = true)]
        [TestCase("-1.0", "0.0", ExpectedResult = false)]
        public bool Equals_Money_SameCurrency_TrueOrFalse(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            Money val = new(v, Currency.ReferenceCurrency);

            // Act
            var result = money.Equals(val);

            // Assert
            return result;
        }

        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "0.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = false)]
        [TestCase("-1.0", "-1.0", ExpectedResult = false)]
        [TestCase("-0.0", "-0.0", ExpectedResult = false)]
        [TestCase("-1.0", "0.0", ExpectedResult = false)]
        public bool Equals_OtherCurrency_False(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.MiddenrealmThaler);
            Money val = new(v, Currency.DwarvenThaler);

            // Act
            var result = money.Equals(val);

            // Assert
            return result;
        }




        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "0.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = true)]
        [TestCase("-1.0", "-1.0", ExpectedResult = true)]
        [TestCase("-0.0", "-0.0", ExpectedResult = true)]
        [TestCase("-1.0", "0.0", ExpectedResult = false)]
        public bool GetHashCode_SameCurrency_TrueOrFalse(decimal m1, decimal m2)
        {
            // Arrange
            Money money1 = new(m1, Currency.ReferenceCurrency);
            Money money2 = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = money1.GetHashCode() == money2.GetHashCode();

            // Assert
            return result;
        }

        [Test]
        [TestCase("0.0", "1.0", ExpectedResult = false)]
        [TestCase("1.0", "0.0", ExpectedResult = false)]
        [TestCase("1.0", "1.0", ExpectedResult = false)]
        [TestCase("-1.0", "-1.0", ExpectedResult = false)]
        [TestCase("-0.0", "-0.0", ExpectedResult = false)]
        [TestCase("-1.0", "0.0", ExpectedResult = false)]
        public bool GetHashCode_OtherCurrency_False(decimal m1, decimal m2)
        {
            // Arrange
            Money money1 = new(m1, Currency.PaaviGuilder);
            Money money2 = new(m2, Currency.AlanfaOreal);

            // Act
            var result = money1.GetHashCode() == money2.GetHashCode();

            // Assert
            return result;
        }





        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = 0)]
        [TestCase("0.0", "1.0", ExpectedResult = -1)]
        [TestCase("1.0", "0.0", ExpectedResult = 1)]
        [TestCase("0.0", "-0.0", ExpectedResult = 0)]
        public int CompareTo_Money(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            Money value = new(v, Currency.ReferenceCurrency);

            // Act
            var result = money.CompareTo(value);

            // Assert
            return result;
        }

        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = 0)]
        [TestCase("0.0", "1.0", ExpectedResult = -1)]
        [TestCase("1.0", "0.0", ExpectedResult = 1)]
        [TestCase("0.0", "-0.0", ExpectedResult = 0)]
        public int CompareTo_MoneyAsObject(decimal m, decimal v)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);
            Money value = new(v, Currency.ReferenceCurrency);

            // Act
            var result = money.CompareTo((object)value);

            // Assert
            return result;
        }



        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = 1.0)]
        [TestCase("1.0", "-1.0", ExpectedResult = -1.0)]
        [TestCase("-1.0", "1.0", ExpectedResult = 1.0)]
        [TestCase("-1.0", "-1.0", ExpectedResult = -1.0)]
        [TestCase("-0.0", "-0.0", ExpectedResult = 0.0)]
        [TestCase("-0.0", "1.0", ExpectedResult = 0.0)]
        [TestCase("0.0", "-1.0", ExpectedResult = -0.0)]
        [TestCase("-1.0", "0.0", ExpectedResult = 1.0)]
        public decimal CopySign(decimal m1, decimal m2)
        {
            // Arrange
            Money money1 = new(m1, Currency.ReferenceCurrency);
            Money money2 = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = Money.CopySign(money1, money2);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }



        [Test]
        [TestCase("1.0", ExpectedResult = 1.0)]
        [TestCase("11.1", ExpectedResult = 1.0)]
        [TestCase("-1.0", ExpectedResult = -1.0)]
        [TestCase("-11.1", ExpectedResult = -1.0)]
        [TestCase("0.0", ExpectedResult = 0.0)]
        [TestCase("-0.0", ExpectedResult = 0.0)]
        public int Sign(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.Sign(money);

            // Assert
            return result;
        }



        [Test]
        [TestCase("1.0", "1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("0.5", "-1.0", "1.0", ExpectedResult = "0.5")]
        [TestCase("-0.5", "-1.0", "1.0", ExpectedResult = "-0.5")]
        [TestCase("1.0", "-1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("-1.0", "-1.0", "1.0", ExpectedResult = "-1.0")]
        [TestCase("-2.0", "-1.0", "1.0", ExpectedResult = "-1.0")]
        [TestCase("2.0", "-1.0", "1.0", ExpectedResult = "1.0")]
        public decimal Clamp_StateUnderTest_ExpectedBehavior(decimal v, decimal _min, decimal _max)
        {
            // Arrange
            Money value = new(v, Currency.ReferenceCurrency);
            Money min = new(_min, Currency.ReferenceCurrency);
            Money max = new(_max, Currency.ReferenceCurrency);

            // Act
            var result = Money.Clamp(value, min, max);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }


        [Test]
        [TestCase("1.0", ExpectedResult = false)]
        [TestCase("-1.0", ExpectedResult = false)]
        [TestCase("0.0", ExpectedResult = true)]
        [TestCase("-0.0", ExpectedResult = true)]
        public bool IsZero(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsZero(money);

            // Assert
            return result;
        }


        [Test]
        [TestCase("1.0", "1.0")]
        [TestCase("-1.0", "1.0")]
        [TestCase("0.0", "0.0")]
        [TestCase("-0.0", "0.0")]
        [TestCase("2.0", "2.0")]
        [TestCase("-2.0", "2.0")]
        public void Abs_StateUnderTest_ExpectedBehavior(decimal m, decimal abs)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.Abs(money);

            // Assert
            Assert.That(result, Is.EqualTo(new Money(abs, Currency.ReferenceCurrency)));
        }

        [Test]
        public void IsComplexNumber()
        {
            // Arrange
            Money money = new(1.0m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsComplexNumber(money);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("1.0")]
        [TestCase("-1.0")]
        [TestCase("0.0")]
        [TestCase("-0.0")]
        [TestCase("2.0")]
        [TestCase("-2.0")]
        public void IsInteger_Integer_True(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsInteger(money);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        [TestCase("1.000000000000000001")]
        [TestCase("-1.0000000000000000001")]
        [TestCase("0.99999999999999999999")]
        [TestCase("-0.99999999999999999999")]
        public void IsInteger_NoInteger_False(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsInteger(money);

            // Assert
            Assert.IsFalse(result);
        }



        [Test]
        public void IsRealNumber__False()
        {
            // Arrange
            Money money = new(1.0m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsRealNumber(money);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsImaginaryNumber__False()
        {
            // Arrange
            Money money = new(1.0m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsImaginaryNumber(money);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        [TestCase("0.0")]
        [TestCase("-0.0")]
        [TestCase("2.0")]
        [TestCase("-2.0")]
        public void IsEvenInteger_IntegerEven_True(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsEvenInteger(money);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        [TestCase("1.0")]
        [TestCase("-1.0")]
        public void IsEvenInteger_IntegerOdd_False(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsEvenInteger(money);

            // Assert
            Assert.IsFalse(result);
        }
        [TestCase("0.00000000000001")]
        [TestCase("-0.00000000000001")]
        [TestCase("2.00000000000001")]
        [TestCase("-2.00000000000001")]
        public void IsEvenInteger_NoIntegerEven_True(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsEvenInteger(money);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("1.0")]
        [TestCase("-1.0")]
        public void IsOddInteger_IntegerOdd_true(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsOddInteger(money);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        [TestCase("0.0")]
        [TestCase("-0.0")]
        [TestCase("2.0")]
        [TestCase("-2.0")]
        public void IsOddInteger_IntegerEven_False(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsOddInteger(money);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("1.0", ExpectedResult = true)]
        [TestCase("-1.0", ExpectedResult = false)]
        [TestCase("0.0", ExpectedResult = true)]
        [TestCase("-0.0", ExpectedResult = false)]
        public bool IsPositive(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsPositive(money);

            // Assert
            return result;
        }

        [TestCase(+1, ExpectedResult = true)]
        [TestCase(-1, ExpectedResult = false)]
        public bool IsPositive_MinMaxValue(int sign)
        {
            // Arrange
            decimal m;
            if (sign > 0) m = decimal.MaxValue; 
            else m = decimal.MinValue;
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsPositive(money);

            // Assert
            return result;
        }



        [Test]
        public void IsPositiveInfinity()
        {
            // Arrange
            Money money = new(Decimal.MaxValue, Currency.ReferenceCurrency);

            // Act
            var result = Money.IsPositiveInfinity(money);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("0", ExpectedResult = false)]
        [TestCase("-0", ExpectedResult = true)]
        [TestCase("1", ExpectedResult = false)]
        [TestCase("-1", ExpectedResult = true)]
        public bool IsNegative(decimal Value)
        {
            // Arrange
            Money money = new(Value, Currency.Andrathaler);

            // Act
            var result = Money.IsNegative(money);

            // Assert
            return result;
        }

        [Test]
        public void IsNegativeInfinity()
        {
            // Arrange
            Money money = new(Decimal.MinValue, Currency.Andrathaler);

            // Act
            var result = Money.IsNegativeInfinity(money);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void IsFinite()
        {
            // Arrange
            Money money = new(decimal.MaxValue, Currency.Andrathaler);

            // Act
            var result = Money.IsFinite(money);

            // Assert
            Assert.IsTrue(result);

        }

        [Test]
        [TestCase("+1.0000000001", ExpectedResult = 1)]
        [TestCase("-1.0000000001", ExpectedResult = -1)]
        [TestCase("+0.999999999999", ExpectedResult = 0)]
        [TestCase("-0.999999999999", ExpectedResult = 0)]
        public decimal Truncate(decimal m)
        {
            // Arrange
            Money money = new(m, Currency.ReferenceCurrency);

            // Act
            var result = Money.Truncate(money);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }

        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("1.0", "-1.0", ExpectedResult = "1.0")]
        [TestCase("-1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
        public decimal MaxMagnitude(decimal m1, decimal m2)
        {
            // Arrange
            Money x = new(m1, Currency.ReferenceCurrency);
            Money y = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = Money.MaxMagnitude(x, y);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }

        [Test] // same as `MaxMagnitude`
        [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("1.0", "-1.0", ExpectedResult = "1.0")]
        [TestCase("-1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
        public decimal MaxMagnitudeNumber(decimal m1, decimal m2)
        {
            // Arrange
            Money x = new(m1, Currency.ReferenceCurrency);
            Money y = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = Money.MaxMagnitudeNumber(x, y);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }

        [Test]
        [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-1.0", "1.0", ExpectedResult = "-1.0")]
        [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
        public decimal MinMagnitude(decimal m1, decimal m2)
        {
            // Arrange
            Money x = new(m1, Currency.ReferenceCurrency);
            Money y = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = Money.MinMagnitude(x, y);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }

        [Test] // same as `MinMagnitude`
        [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
        [TestCase("1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-1.0", "1.0", ExpectedResult = "-1.0")]
        [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
        [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
        public decimal MinMagnitudeNumber(decimal m1, decimal m2)
        {
            // Arrange
            Money x = new(m1, Currency.ReferenceCurrency);
            Money y = new(m2, Currency.ReferenceCurrency);

            // Act
            var result = Money.MinMagnitudeNumber(x, y);

            // Assert
            return result.ToCurrencyValue(Currency.ReferenceCurrency);
        }

        [Test]
        public void IsNaN_Always_False()
        {
            // Arrange
            Money money = new(83736363, Currency.Andrathaler);

            // Act
            var result = Money.IsNaN(money);

            // Assert
            Assert.That(result, Is.False);

        }



    }
}
