using System;
using System.Collections;
using System.Collections.Generic;

namespace FateExplorer.Shared;

public class WeightedList<T> : IEnumerable<T>, IList<T>
{
    private List<T> list;
    private List<float> weight;

    /// <summary>
    /// The weighted size of the list
    /// </summary>
    private double Size = EmptySize;
    private const double EmptySize = 0;


    /// <summary>
    /// Default constructor creates an empty weighted list.
    /// </summary>
    public WeightedList()
    {
        list = new List<T>();
        weight = new List<float>();
    }
    
    
    /// <summary>
    /// Constructor generating the list from en <c>IEnumerable</c>. Weights are initialized with 1.
    /// </summary>
    /// <param name="items"></param>
    public WeightedList(IEnumerable<T> items)
    {
        list = new(items);
        weight = new List<float>(list.Count);
        for (int i = 0; i < list.Count; i++)
            weight[i] = 1;
    }


    /// <summary>
    /// Adds an item to the list.
    /// </summary>
    /// <param name="item">The object to add.</param>
    /// <param name="weight">The weight of item.</param>
    public void Add(T item, float weight)
    {
        list.Add(item);
        this.weight.Add(weight);
        Size = 0;
    }


    /// <summary>
    /// Inserts an item to the list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <c>value</c> should be inserted.</param>
    /// <param name="item">The object to insert.</param>
    /// <param name="weight">The weight of item.</param>
    public void Insert(int index, T item, float weight)
    {
        list.Insert(index, item);
        this.weight.Insert(index, weight);
        Size = 0;
    }


    /// <summary>
    /// Sets the weight at the given index.
    /// </summary>
    /// <param name="index">The zero-based index at which <c>weight</c> should be set.</param>
    /// <param name="weight">The weight of item.</param>
    public void SetWeightAt(int index, float weight)
    {
        this.weight[index] = weight;
        Size = 0;
    }

    //public bool IsFixedSize => false; // according to learn.microsoft.com an IList property but not recognized by VS

    /// <summary>
    /// Return a random entry from the list and sets the weight to zero if replace is <c>false</c>.
    /// </summary>
    /// <returns>A random item from the list.</returns>
    public T Random(bool replace = false)
    {
        if (Size == EmptySize)
            foreach(float w in weight)
                Size += w;

        Random r = new();
        var wi = r.NextDouble() * Size;

        int index = 0;
        foreach(float w in weight)
        {
            wi -= w;
            if (wi <= 0) return list[index];
            index++;
        }

        if (!replace) SetWeightAt(index, 0);
        return list[index];
    }



    /// <summary>
    /// Remove any list entries with a weight of zero.
    /// </summary>
    public void Compress()
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (weight[i] == 0)
            {
                list.RemoveAt(i);
                weight.RemoveAt(i);
            }
        }
    }



    #region IList

    public T this[int index] { get => list[index]; set => list[index] = value; }


    public int Count => list.Count;


    public bool IsReadOnly => false;


    public void Add(T item)
    {
        list.Add(item);
        weight.Add(1);
        Size = 0;
    }


    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        weight.Insert(index, 1);
        Size = 0;
    }


    public int IndexOf(T item) => list.IndexOf(item);


    public bool Remove(T item)
    {
        int i = list.IndexOf(item);
        if (i == -1) return false;
        RemoveAt(i); // removes both weight and item
        Size = 0;
        return true;
    }


    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
        weight.RemoveAt(index);
        Size = 0;
    }


    public void Clear()
    {
        list.Clear();
        weight.Clear();
        Size = 0;
    }


    public bool Contains(T item) => list.Contains(item);


    public void CopyTo(T[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex);
    }

    #endregion


    #region IEnumerable

    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }

    #endregion
}
