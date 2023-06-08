using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace FateExplorer.ViewModel.Tests;



[TestFixture()]
public class PraiseViMoTests
{
    private MockRepository mockRepository;
    private Mock<IGameDataService> gameDataService;
    private PraiseOrInsultDB Data;

    private PraiseViMo praise;

    private static readonly string[] FateQuoteData = new[] { "FateQuote0", "FateQuote1" };
    private static readonly PraiseOrInsultEntry[] PraiseData = new[] 
    { 
        new PraiseOrInsultEntry() { Text = "Praise0", Scope = 255 }, 
        new PraiseOrInsultEntry() { Text = "Praise1", Scope = 1 } 
    };
    private static readonly PraiseOrInsultEntry[] InsultData = new[]
    {
        new PraiseOrInsultEntry() { Text = "Insult0", Scope = 255 },
        new PraiseOrInsultEntry() { Text = "Insult1", Scope = 1 }
    };
    private static readonly string[] PraisePhraseData = new[] { "Phrase0 {0}", "Phrase1 {0}" };
    private static readonly string[] AdjectiveData = new[] { "Adj0", "Adj1" };


    [SetUp]
    public void Setup()
    {
        mockRepository = new(MockBehavior.Strict);
        gameDataService = mockRepository.Create<IGameDataService>();

        Data = new()
        {
            FateQuote = new[] { "FateQuote0", "FateQuote1" },
            Praise = PraiseData, Insult = InsultData,
            PraisePhrase = PraisePhraseData,
            PositiveAdjective = AdjectiveData
        };
        gameDataService.SetupGet(s => s.PraiseOrInsult).Returns(Data);

        praise = new(gameDataService.Object);
    }


    [Test, Retry(7)]
    public void GetFateQuoteTest()
    {
        // Arrange
        // Act
        var result = praise.GetFateQuote();

        // Assert
        Assert.That(result, Is.AnyOf(FateQuoteData));
        mockRepository.Verify();
    }



    [Test, Retry(7)]
    [TestCase(PraiseViMo.Praise, false)]
    [TestCase(PraiseViMo.Insult, false)]
    [TestCase(PraiseViMo.Praise, true)]
    [TestCase(PraiseViMo.Insult, true)]
    public void GiveTest(int Tone, bool isCombat)
    {
        // Arrange
        Check check = new(Check.Roll.Dodge);

        // Act
        var result = praise.Give(Tone, check);
        
        // Assert
        if (Tone == PraiseViMo.Praise)
        {
            if (isCombat)
                Assert.That(result, Is.AnyOf(new[] { Array.Find(PraiseData, i => i.Scope == 255).Text, "Phrase0 Adj0", "Phrase1 Adj1" }));
            else
                Assert.That(result, Is.AnyOf(new[] { PraiseData[0].Text, PraiseData[1].Text, "Phrase0 Adj0", "Phrase1 Adj1" }));
        }
        else if (Tone == PraiseViMo.Insult)
        {
            if (isCombat)
            {
                var Expected = Array.Find(InsultData, i => i.Scope == 255).Text;
                Console.WriteLine($"Expected: {Expected}");
                Assert.That(result, Is.EqualTo(Expected));
            }
            else
            {
                var Expected = InsultData.Select(i => i.Text).ToArray();
                Console.WriteLine($"Expected: {Expected}");
                Assert.That(result, Is.AnyOf(Expected));
            }
        }
        else
            Assert.Fail();
        mockRepository.Verify();
    }
}