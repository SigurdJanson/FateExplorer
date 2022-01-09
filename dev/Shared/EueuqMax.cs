using System.Collections.Generic;

namespace FateExplorer.Shared
{
    /// <summary>
    /// Provides a queue with 2 extra features. A maximum limit of contents. If the queue grows
    /// larger the items exceeding the limit will be removed. And a reverse iterator.
    /// </summary>
    public class EueuqMax<T> : Queue<T>
    {
        private int maxCount;

        /// <summary>
        /// The maximum of the queue. It will not hold more items than that.
        /// if MaxCount <= 0 it will be ignored.
        /// </summary>
        public int MaxCount { 
            get => maxCount; 
            set
            {
                int oldMax = maxCount;
                maxCount = value;
                if (oldMax > value)
                    ResetMax();

                EnsureCapacity(maxCount + 1);
            }
        }

        /// <summary>
        /// Makes sure that the content of the queue does not exceed <see cref="MaxCount"/>
        /// </summary>
        protected void ResetMax()
        {
            if (MaxCount <= 0) return;
            while (Count > MaxCount)
                Dequeue();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCount">The maximum allowed elements in the queue</param>
        public EueuqMax(int maxCount = -1)
        {
            MaxCount = maxCount;
            EnsureCapacity(MaxCount+1);
        }




        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">The object to add to the queue. The value can be null.</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            ResetMax();
        }
    }
}
