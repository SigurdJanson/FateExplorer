using FateExplorer.CharacterImport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitTests.CharacterImport;


[TestFixture]
public class JsonActivatableSIDConverterTests
{
    const string TestFile = "_test_sid.json";

    public class TestClass
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("sid"), JsonConverter(typeof(JsonActivatableSIDConverter))]
        public string Sid { get; set; }
        
        [JsonPropertyName("sid1"), JsonConverter(typeof(JsonActivatableSIDConverter))]
        public string Sid1 { get; set; }
        
        [JsonPropertyName("sid2"), JsonConverter(typeof(JsonActivatableSIDConverter))]
        public string Sid2 { get; set; }
    }



    [Test]
    public void Read_FromFile()
    {
        // Arrange
        string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2TestData));
        string fileName = Path.GetFullPath(Path.Combine(BasePath, TestFile));
        string jsonString = File.ReadAllText(fileName);

        // Act
        var Result = JsonSerializer.Deserialize<List<TestClass>>(jsonString);

        // Assert
        Assert.AreEqual(4, Result.Count); // 4 objects in file

        Assert.AreEqual("Thirty", Result[0].Sid);
        Assert.AreEqual("One point five", Result[0].Sid1);
        Assert.AreEqual("999", Result[0].Sid2);

        Assert.AreEqual("12", Result[1].Sid);
        Assert.AreEqual("-24", Result[1].Sid1);
        Assert.AreEqual(null, Result[1].Sid2);

        Assert.AreEqual("0.75", Result[2].Sid);
        Assert.AreEqual("1999", Result[2].Sid1);
        Assert.AreEqual("-2000", Result[2].Sid2);

        Assert.AreEqual("0", Result[3].Sid);
        Assert.AreEqual(Int32.MaxValue.ToString(), Result[3].Sid1);
        Assert.AreEqual(Int32.MinValue.ToString(), Result[3].Sid2);
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
        const string Expected = "[{\"id\":\"1\",\"name\":\"A\",\"amount\":1,\"sid\":1},{\"id\":\"2\",\"name\":\"B\",\"amount\":2,\"sid\":\"2.0\",\"sid2\":\"Sid2\"}]";

        List<TestClass> Tester = new()
        {
            new TestClass() { Amount = 1, Id = "1", Name = "A", Sid = "1" },
            new TestClass() { Amount = 2, Id = "2", Name = "B", Sid = "2.0", Sid2 = "Sid2" }
        };

        // Act
        var result = JsonSerializer.Serialize<List<TestClass>>(Tester, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

        // Assert
        Assert.AreEqual(Expected, result);
    }



    [Test]
    public void CanConvert_ListOfInt_True()
    {
        // Arrange
        var Converter = new JsonActivatableSIDConverter();
        // Act, Assert
        Assert.IsTrue(Converter.CanConvert(typeof(string)));
        //-Assert.IsTrue(Converter.CanConvert(typeof(int)));
        //--Assert.IsTrue(Converter.CanConvert(typeof(List<int>)));
    }

}
