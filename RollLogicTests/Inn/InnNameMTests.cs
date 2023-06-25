using FateExplorer.Inn;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Text.Json;

namespace UnitTests.Inn
{
    [TestFixture]
    public class InnNameMTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }


        private InnNameM CreateInnNameM(Region region = Region.Arania)
        {
            //return new InnNameM();
            InnNameM Result;
            if (region != 0)
                Result = JsonSerializer.Deserialize<InnNameM>($"{{\"id\": 10, \"name\": \"Bierschwemme\", \"ql1\": 1, \"ql6\": 0.1, \"region\": [{(int)region}]}}");
            else
                Result = JsonSerializer.Deserialize<InnNameM>($"{{\"id\": 10, \"name\": \"Bierschwemme\", \"ql1\": 1, \"ql6\": 0.1}}");
            return Result;
        }


        [Test]
        [TestCase(Region.Arania, ExpectedResult = true)]
        [TestCase(Region.Cyclopeans, ExpectedResult = false)]
        public bool CanBeFound_RegionIsSpecified(Region Where)
        {
            // Arrange
            var innNameM = this.CreateInnNameM();

            // Act
            var result = innNameM.CanBeFound(Where);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }



        [Test]
        [TestCase(Region.Arania, ExpectedResult = true)]
        [TestCase(Region.Cyclopeans, ExpectedResult = true)]
        public bool CanBeFound_RegionNotSpecified_AllTrue(Region Where)
        {
            // Arrange
            var innNameM = this.CreateInnNameM(0);

            // Act
            var result = innNameM.CanBeFound(Where);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }

    }
}
