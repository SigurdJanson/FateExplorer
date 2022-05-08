using System;

namespace FateExplorer.Calendar
{
    /// <summary>
    /// Interface that provides the current date for the game play.
    /// </summary>
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
