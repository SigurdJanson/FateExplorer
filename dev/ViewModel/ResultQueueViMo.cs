using FateExplorer.RollLogic;
using System.Collections.Generic;

namespace FateExplorer.ViewModel
{
    public class ResultQueueViMo : Queue<RollResultViMo>
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
        /// <param name="maxCount">THe maximum allowed elements in the queue</param>
        public ResultQueueViMo(int maxCount = -1)
        {
            MaxCount = maxCount;
        }




        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">The object to add to the queue. The value can be null.</param>
        public new void Enqueue(RollResultViMo item)
        {
            base.Enqueue(item);
            ResetMax();
        }
    }
}
