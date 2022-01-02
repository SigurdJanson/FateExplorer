using FateExplorer.CharacterData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RollLogicTests.CharacterData
{
    [TestFixture]
    public class JsonFakeListConverterTests
    {

        public class TestClass
        {
            [JsonPropertyName("beginning")]
            public int Beginning { get; set; }

            [JsonPropertyName("items"), JsonConverter(typeof(JsonFakeListConverter<BelongingItem>))]
            public List<BelongingItem> Items { get; set; }

            [JsonPropertyName("end")]
            public string End { get; set; }
        }



        [Test]
        public void Write_StateUnderTest_ExpectedBehavior()
        {
            // Assert
            Assert.Fail();
        }

        [Test]
        public void Read_StateUnderTest_ExpectedBehavior()
        {
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "_test_fakelistimport.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            TestClass Result = JsonSerializer.Deserialize<TestClass>(jsonString);

            // Assert
            Assert.AreEqual(3, Result.Items.Count);
            //Assert.AreEqual(Ability1, Result[0].ShortName);
            //Assert.AreEqual(AbilityLast, Result[^1].ShortName);
        }

        [Test]
        public void CanConvert_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var jsonFakeListConverter = new JsonFakeListConverter<TestClass>();
            List<TestClass> TestObject = new();
            Type objectType = TestObject.GetType(); //typeof(TestObject.Ty);

            // Act
            var Result = jsonFakeListConverter.CanConvert(objectType);

            // Assert
            Assert.IsTrue(Result);
        }
    }
}
