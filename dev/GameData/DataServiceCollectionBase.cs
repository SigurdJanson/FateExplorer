using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class DataServiceCollectionBase<T> where T : ICharacterAttribute
    {
        /// <summary>
        /// List containing the data base entries
        /// </summary>
        [JsonPropertyName("Entries")]
        public IReadOnlyList<T> Data { get; set; }

        /// <summary>
        /// Return the element of the data base with the designated numeric index.
        /// </summary>
        /// <param name="i">i-th item</param>
        /// <returns>Data base object at index i</returns>
        public T this[int i] => Data[i];

        /// <summary>
        /// Return the element of the data base with the designated id string.
        /// </summary>
        /// <param name="IdString">A unique string id</param>
        /// <returns>The object with the given id; default if the id does not exist.</returns>
        public T this[string IdString] => Data.First(i => i.Id == IdString);

        /// <summary>
        /// Number of entries in the data base
        /// </summary>
        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }
}
