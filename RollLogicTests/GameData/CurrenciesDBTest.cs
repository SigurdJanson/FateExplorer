using FateExplorer.GameData;
using NUnit.Framework;

namespace UnitTests.GameData;


[TestFixture]
public class CurrenciesDBTest : GameDataTestsBase<CurrenciesDB, CurrencyDbEntry>
{
    public override string FilenameId { get => "currency"; }


    [Test]
    [TestCase("de", "Oreal", "Arganbrox (Zwergenschilling)")]
    [TestCase("en", "Oreal", "Arganbrox (Dwarven Shilling)")]
    public void LoadFromFile_ParseSuccessful(string Language, string Currency1, string CurrencyLast)
    {
        // Arrange
        // Act
        CurrenciesDB Result = CreateDBfromFile(Language);

        // Assert
        Assert.AreEqual(8, Result.Count);
        Assert.AreEqual(Currency1, Result[0].Name);
        Assert.AreEqual(CurrencyLast, Result[^1].Name);
    }

    // inherited: public void Count_ContentNotLoaded_Return0()
}
