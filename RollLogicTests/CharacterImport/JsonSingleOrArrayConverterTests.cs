using FateExplorer.CharacterImport;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitTests.CharacterImport
{
    [TestFixture]
    public class JsonSingleOrArrayConverterTests
    {
        public class TestClass
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("stp"), JsonConverter(typeof(JsonSingleOrArrayConverter<List<int>,int>))]
            public List<int> Stp { get; set; }
            [JsonPropertyName("amount")]
            public int Amount { get; set; }
            [JsonPropertyName("weight")]
            public double Weight { get; set; }
        }



        [Test]
        public void Read_FromFile()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2TestData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "_test_singleORarray.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            var Result = JsonSerializer.Deserialize<List<TestClass>>(jsonString);

            // Assert
            Assert.AreEqual(4, Result.Count);

            Assert.AreEqual(30, Result[0].Stp[0]);
            Assert.AreEqual(1, Result[0].Stp.Count);

            Assert.AreEqual(2, Result[1].Stp.Count);
            Assert.That(Result[1].Stp[0], Is.EqualTo(4).Or.EqualTo(20));
            Assert.That(Result[1].Stp[1], Is.EqualTo(4).Or.EqualTo(20));

            Assert.AreEqual(1, Result[2].Stp.Count);
            Assert.AreEqual(2000, Result[2].Stp[0]);

            Assert.AreEqual(1, Result[3].Stp.Count);
            Assert.AreEqual(80, Result[3].Stp[0]);
        }


        //[Test]
        //public void Read()
        //{
        //    // Arrange
        //    const string TestStr = @"[{4}, {20}]";
        //    var Test = new JsonSingleOrArrayConverter<List<int>, int>();
        //    var Span = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(TestStr));
        //    var Reader = new Utf8JsonReader(Span);

        //    // Act
        //    var result = Test.Read(ref Reader, typeof(List<int>), new JsonSerializerOptions(){ AllowTrailingCommas = false });

        //    // Assert
        //    Assert.AreEqual(2, result.Count);
        //}


        //[Test]
        //public void Write_List()
        //{
        //    // Arrange
        //    const string Expected = "[{\"id\":\"1\",\"name\":\"A\",\"stp\":11,\"amount\":1,\"weight\":0},{\"id\":\"2\",\"name\":\"B\",\"stp\":[11,22],\"amount\":2,\"weight\":0}]";

        //    var Converter = new JsonSingleOrArrayConverter<List<int>, int>();
        //    List<int> TestList = new();
        //    TestList.Add(11);

        //    ArrayBufferWriter<byte> Buffer = new();
        //    Utf8JsonWriter Writer = new(Buffer);

        //    // Act
        //    Converter.Write(Writer, TestList, new JsonSerializerOptions());
        //    var result = Buffer.GetMemory().ToString();

        //    // Assert
        //    Assert.AreEqual(Expected, result);
        //}


        [Test]
        public void Write_WholeObject()
        {
            // Arrange
            const string Expected = "[{\"id\":\"1\",\"name\":\"A\",\"stp\":11,\"amount\":1,\"weight\":0},{\"id\":\"2\",\"name\":\"B\",\"stp\":[11,22],\"amount\":2,\"weight\":0}]";

            List<TestClass> Tester = new()
            {
                new TestClass() { Amount = 1, Id = "1", Name = "A", Stp = new List<int> { 11 } },
                new TestClass() { Amount = 2, Id = "2", Name = "B", Stp = new List<int> { 11, 22 } }
            };

            // Act
            var result = JsonSerializer.Serialize<List<TestClass>>(Tester);

            // Assert
            Assert.AreEqual(Expected, result);
        }



        [Test]
        public void CanConvert_ListOfInt_True()
        {
            // Arrange
            var Converter = new JsonSingleOrArrayConverter<List<int>, int>();
            // Act, Assert
            Assert.IsTrue(Converter.CanConvert(typeof(int)));
            Assert.IsTrue(Converter.CanConvert(typeof(List<int>)));
        }


        [Test]
        public void CanConvert_ListOfStruct_True()
        {
            // Arrange
            var Converter = new JsonSingleOrArrayConverter<List<DateOnly>, DateOnly>();
            // Act, Assert
            Assert.IsTrue(Converter.CanConvert(typeof(DateOnly)));
            Assert.IsTrue(Converter.CanConvert(typeof(List<DateOnly>)));
        }

    }
}
