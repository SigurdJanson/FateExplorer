using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.GameData
{
    public class DataServiceCollectionBase<T>
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<T> Data { get; set; }

        public T this[int i] => Data[i];

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }
}
