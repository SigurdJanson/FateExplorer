using FateExplorer.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.GameData
{
    public class DataServiceCollectionBase<T> where T : ICharacterAttribute
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<T> Data { get; set; }

        public T this[int i] => Data[i];

        public T this[string IdString] => Data.First(i => i.Id == IdString);

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }
}
