using System;

namespace FateExplorer.Calendar
{
    public interface IDateOfPlay
    {
        /// <summary>
        /// Notify registered components when the hero has changed.
        /// </summary>
        public event Action OnChange;

        /// <summary>
        /// The current date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
