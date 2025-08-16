using FateExplorer.GameData;
using FateExplorer.Shared;
using FateExplorer.ViewModel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitTests.ViewModel
{
    [TestFixture]
    public class RollHandlerViMoTests
    {
        //private static readonly string JsonV1 = "{\"Entries\": {" +
        //    "\"ATTR\": {\"id\": \"ATTR\", \"roll\": \"DSA5/0/ability\", \"name\": \"Eigenschaftsprobe\", \"type\": \"simple\"}," +
        //    "\"TAL\":  {\"id\": \"TAL\", \"roll\": \"DSA5/0/skill/mundane\", \"name\": \"Fertigkeitsprobe\", \"type\": \"simple\"}, " +
        //    "\"SPELL\":{\"id\": \"SPELL\", \"roll\": \"DSA5/0/skill/arcane\", \"name\": \"Zauber\", \"type\": \"simple\"}, " +
        //    "\"LITURGY\": {\"id\": \"LITURGY\", \"roll\": \"DSA5/0/skill/karma\", \"name\": \"Liturgiewirken\", \"type\": \"simple\"}, " +
        //    "\"REGENERATE\": {\"id\": \"REGENERATE\", \"roll\": \"DSA5/0/regeneration\", \"name\": \"Regeneration\", \"type\": \"value\"}, " +
        //    "\"INI\": {\"id\": \"INI\", \"roll\": \"DSA5/0/initiative\", \"name\": \"Initiative\", \"type\": \"compare\"}, " +
        //    "\"CT_9/AT+SA_186\": {\"id\": \"CT_9/AT+SA_186\", \"roll\": \"DSA5/0/initiative\", \"name\": \"Hruruzat Attacke\", \"type\": \"compare\"}}";
        /// <value>
        /// Number of mappings in rollresolver.json
        /// </value>
        private const int RollCheckMappings = 44;

        private MockRepository mockRepository;
        private Mock<IGameDataService> mockGameData;


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
            this.mockGameData = mockRepository.Create<IGameDataService>();
        }

        private RollHandlerViMo CreateRollHandlerViMo(string jsonString)
        {
            var Result = new RollHandlerViMo(new HttpClient(), mockGameData.Object);
            Result.ReadRollMappings(jsonString);
            Result.RegisterChecks();
            return Result;
        }

        private static string GetMappingDataFromWWWroot()
        {
            const string FilenameId = "rollresolver";

            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}.json")); // _{Language}
            string jsonString = File.ReadAllText(fileName);
            return jsonString;
        }



        [Test]
        public void CheckMappingFileFormat()
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();

            // Act
            var Result = JsonSerializer.Deserialize<Dictionary<string, RollMappingViMo>>(jsonString);

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(44, Is.EqualTo(Result.Count));
            this.mockRepository.VerifyAll();
        }



        [Test]
        public void OpenRollCheck_BasicAttributes_ReturnValidCheck([Values("ATTR_4")] string Id)
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();
            RollHandlerViMo ClassUnderTest = CreateRollHandlerViMo(jsonString);
            Assume.That(ClassUnderTest.RollMappings.Count, Is.EqualTo(RollCheckMappings));

            AbilityDTO data = new() { Id = Id };

            // Act
            var Result = ClassUnderTest.OpenRollCheck(new Check(Check.Roll.Ability), data, new CheckContextViMo());

            // Assert
            Assert.That(Result, Is.Not.Null);
        }



        static IEnumerable<int[]> Routine_InsufficientAbility()
        {
            // 3 abilities
            yield return new int[] { 13, 13, 12 };
            yield return new int[] { 13, 13, 12 };
            yield return new int[] { 13, 13, 12 };
        }
        static IEnumerable<int[]> Routine_EnoughAbility()
        {
            // 3 abilities
            yield return new int[] { 13, 13, 13 };
            yield return new int[] { 19, 13, 13 };
            yield return new int[] { 19, 19, 13 };
            yield return new int[] { 19, 19, 19 };
        }


        // Ability values < 13 do not support routine skill checks of this type.
        [Test]
        public void RoutineSkillCheck_AbilitiesTooSmall_0(
            [ValueSource(nameof(Routine_InsufficientAbility))] int[] AbilityVal,
            [Values(0, 10, 20)] int SkillVal,
            [Values(0, 10, 20)] int Modifier )
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();
            RollHandlerViMo ClassUnderTest = CreateRollHandlerViMo(jsonString);

            SkillsDTO skill = new() { Id = "Any", Name = "Any", EffectiveValue = SkillVal };
            AbilityDTO[] ability = new AbilityDTO[3];
            for (int ai = 0; ai < 3; ai++)
                ability[ai] = new() { Id = "Any", Name = "Any", EffectiveValue = AbilityVal[ai] };

            // Act
            bool result = ClassUnderTest.CanRoutineSkillCheck(skill, ability, new Modifier(Modifier));

            // Assert
            Assert.That(false, Is.EqualTo(result));
        }


        // Skill value of 0 does not support routine skill checks of this type.
        [Test]
        public void RoutineSkillCheck_SkillTooSmall_0(
            [ValueSource(nameof(Routine_EnoughAbility))] int[] AbilityVal,
            [Values(0)] int SkillVal,
            [Values(0, 10, 20)] int Modifier)
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();
            RollHandlerViMo ClassUnderTest = CreateRollHandlerViMo(jsonString);

            SkillsDTO skill = new() { Id = "Any", Name = "Any", EffectiveValue = SkillVal };
            AbilityDTO[] ability = new AbilityDTO[3];
            for (int ai = 0; ai < 3; ai++)
                ability[ai] = new() { Id = "Any", Name = "Any", EffectiveValue = AbilityVal[ai] };

            // Act
            bool result = ClassUnderTest.CanRoutineSkillCheck(skill, ability, new Modifier(Modifier));

            // Assert
            Assert.That(false, Is.EqualTo(result));
        }



        [Test, Sequential]
        public void RoutineSkillCheck_Enough_QS1(
            [ValueSource(nameof(Routine_EnoughAbility))] int[] AbilityVal,
            [Values(19, 13, 10, 1)] int SkillVal,
            [Values(-3, -1, 0, 3)] int Modifier)
        {
            // Arrange
            string jsonString = GetMappingDataFromWWWroot();
            RollHandlerViMo ClassUnderTest = CreateRollHandlerViMo(jsonString);

            SkillsDTO skill = new() { Id = "Any", Name = "Any", EffectiveValue = SkillVal };
            AbilityDTO[] ability = new AbilityDTO[3];
            for (int ai = 0; ai < 3; ai++)
                ability[ai] = new() { Id = "Any", Name = "Any", EffectiveValue = AbilityVal[ai] };

            // Act
            bool result = ClassUnderTest.CanRoutineSkillCheck(skill, ability, new Modifier(Modifier));

            // Assert
            Assert.That(false, Is.LessThan(result));
        }
    }
}
