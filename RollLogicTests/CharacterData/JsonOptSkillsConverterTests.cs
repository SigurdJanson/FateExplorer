using FateExplorer.CharacterImport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitTests.CharacterData
{
    [TestFixture]
    public class JsonOptSkillsConverterTests
    {
        readonly Dictionary<string, int> TargetResult = new()
        {
            { "SPELL_6", 7 },
            { "SPELL_8", 6 },
            { "SPELL_79", 0 },
            { "SPELL_20", 11 },
            { "SPELL_134", 7 },
            { "SPELL_35", 5 },
            { "SPELL_40", 4 },
            { "SPELL_72", 1 },
            { "SPELL_3", 5 },
            { "SPELL_7", 5 },
            { "SPELL_30", 5 },
            { "SPELL_9", 5 }
        };

        public class TestClass
        {
            [JsonPropertyName("beginning")]
            public int Beginning { get; set; }

            [JsonPropertyName("spells"), JsonConverter(typeof(JsonOptSkillsConverter))]
            public Dictionary<string, int> Spells { get; set; }

            [JsonPropertyName("end")]
            public string End { get; set; }
        }


        [Test]
        public void Read_StateUnderTest_ReadCorrectly()
        {

            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2TestData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"_test_spellsjsonimport.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            TestClass Result = JsonSerializer.Deserialize<TestClass>(jsonString);
            // Act
            //var result = jsonOptSkillsConverter.Read(ref reader, typeToConvert, options);

            // Assert
            Assert.AreEqual(12, Result.Spells.Count);
            foreach (var t in TargetResult)
                Assert.AreEqual(t.Value, Result.Spells[t.Key]);
        }



        [Test]
        public void CanConvert_CorrectType_ReturnsTrue()
        {
            // Arrange
            var jsonOptSkillsConverter = new JsonOptSkillsConverter();
            Dictionary<string, int> TestObject = new();
            Type objectType = TestObject.GetType(); //typeof(TestObject.Ty);

            // Act
            var Result = jsonOptSkillsConverter.CanConvert(objectType);

            // Assert
            Assert.IsTrue(Result);
        }
    }
}
