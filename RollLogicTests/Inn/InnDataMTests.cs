using FateExplorer.Inn;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace UnitTests.Inn;


[TestFixture]
public class InnDataMTests
{
    private MockRepository mockRepository;

    /// <summary>
    /// Leading string to identify the json file
    /// </summary>
    public const string FilenameId = "inns";


    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);
    }


    private InnDataM CreateInnDataM()
    {
        return new InnDataM();
    }


    protected InnDataM CreateDBfromFile(string Language)
    {
        Assume.That(Language, Is.EqualTo("en").Or.EqualTo("de"));
        // Arrange
        string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
        string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}_{Language}.json"));
        string jsonString = File.ReadAllText(fileName);

        // Act
        InnDataM Result = JsonSerializer.Deserialize<InnDataM>(jsonString);

        return Result;
    }



    [Test]
    [TestCase("de")]
    [TestCase("en")]
    public void Deserialise(string language)
    {
        // Arrange
        var innDataM = this.CreateDBfromFile(language);

        // Act


        // Assert
        Assert.That(innDataM.Unit.Length, Is.GreaterThan(0));
        Assert.That(innDataM.Category.Count, Is.GreaterThan(0));
        Assert.That(innDataM.Qualifier.Length, Is.GreaterThan(0));
        Assert.That(innDataM.NameBase.Length, Is.GreaterThan(0));
        Assert.That(innDataM.FullName.Length, Is.GreaterThan(0));
        this.mockRepository.VerifyAll();
    }
}
