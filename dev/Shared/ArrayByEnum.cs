using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FateExplorer.Shared
{
    /// <summary>An array containing objects of type T indexed by an enum U.</summary>
    /// <typeparam name="T">Type stored in array</typeparam>
    /// <typeparam name="U">Indexer Enum type</typeparam>
    /// <remarks>With regards to <seealso href="https://stackoverflow.com/a/50969107/13241545"/></remarks>
    public class ArrayByEnum<T, U> : IEnumerable<T> where U : Enum
    {
        private readonly T[] _array;
        private readonly int _lower;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArrayByEnum()
        {
            _lower = Convert.ToInt32(Enum.GetValues(typeof(U)).Cast<U>().Min());
            int upper = Convert.ToInt32(Enum.GetValues(typeof(U)).Cast<U>().Max());
            _array = new T[1 + upper - _lower];
        }

        /// <summary>
        /// Gets or sets the element at the specified Enum index.
        /// </summary>
        /// <param name="index">The Enum index of the element to get or set.</param>
        /// <returns>Element at the given index</returns>
        public T this[U index]
        {
            get { return _array[Convert.ToInt32(index) - _lower]; }
            set { _array[Convert.ToInt32(index) - _lower] = value; }
        }

        /// <summary>
        /// Gets the number of elements contained in the array
        /// </summary>
        public int Count => typeof(U).GetFields().Length;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return Enum.GetValues(typeof(U)).Cast<U>().Select(i => this[i]).GetEnumerator();
        }


        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enum.GetValues(typeof(U)).Cast<U>().Select(i => this[i]).GetEnumerator();
        }
    }
}
