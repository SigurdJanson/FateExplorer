﻿using FateExplorer.ViewModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RollLogicTests.ViewModel
{
    [TestFixture]
    public class RollHandlerViMoTests
    {
        private static readonly string JsonV1 = "{\"Entries\": {" +
            "\"ATTR\": {\"id\": \"ATTR\", \"roll\": \"DSA5/0/ability\", \"name\": \"Eigenschaftsprobe\", \"type\": \"simple\"}," +
            "\"TAL\":  {\"id\": \"TAL\", \"roll\": \"DSA5/0/skill/mundane\", \"name\": \"Fertigkeitsprobe\", \"type\": \"simple\"}, " +
            "\"SPELL\":{\"id\": \"SPELL\", \"roll\": \"DSA5/0/skill/arcane\", \"name\": \"Zauber\", \"type\": \"simple\"}, " +
            "\"LITURGY\": {\"id\": \"LITURGY\", \"roll\": \"DSA5/0/skill/karma\", \"name\": \"Liturgiewirken\", \"type\": \"simple\"}, " +
            "\"REGENERATE\": {\"id\": \"REGENERATE\", \"roll\": \"DSA5/0/regeneration\", \"name\": \"Regeneration\", \"type\": \"value\"}, " +
            "\"INI\": {\"id\": \"INI\", \"roll\": \"DSA5/0/initiative\", \"name\": \"Initiative\", \"type\": \"compare\"}, " +
            "\"CT_9/AT+SA_186\": {\"id\": \"CT_9/AT+SA_186\", \"roll\": \"DSA5/0/initiative\", \"name\": \"Hruruzat Attacke\", \"type\": \"compare\"}}";
        
        private MockRepository mockRepository;


        public class DataTestClass
        {
            /// <summary>
            /// Definition must match <see cref="RollHandlerViMo.RollMappings"/>
            /// </summary>
            [JsonPropertyName("Entries")]
            public Dictionary<string, RollMappingViMo> RollMappingMock { get; set; }

            public int Count { get => RollMappingMock.Count; }
        }


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private RollHandlerViMo CreateRollHandlerViMo(string jsonString)
        {
            var Result = new RollHandlerViMo(new HttpClient());
            Result.ReadRollMappings(jsonString);
            Result.RegisterChecks();
            return Result;
        }

        private string GetMappingDataFromWWWroot()
        {
            const string FilenameId = "rollresolver";

            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}.json")); // _{Language}
            string jsonString = File.ReadAllText(fileName);
            return jsonString;
        }



        [Test]
        public void _CheckMappingFileFormat_Read2Dictionary_NoExceptions()
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();

            // Act
            DataTestClass Result = JsonSerializer.Deserialize<DataTestClass>(jsonString);

            // Assert
            Assert.IsNotNull(Result);
            Assert.AreEqual(7, Result.Count);
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void OpenRollCheck_BasicAttributes_ReturnValidCheck([Values("ATTR_4")] string Id)
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();
            RollHandlerViMo ClassUnderTest = CreateRollHandlerViMo(jsonString);
            Assume.That(ClassUnderTest.RollMappings.Count, Is.EqualTo(7));

            // Act
            var Result = ClassUnderTest.OpenRollCheck(Id);

            // Assert
            Assert.NotNull(Result);
        }
    }
}