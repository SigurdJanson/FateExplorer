using System;

namespace FateExplorer.Shared
{
    /// <summary>
    /// Implements the state container pattern to allow Blazor inter-class communication
    /// </summary>
    public interface IStateContainer
    {
        /// <summary>
        /// Invoked every time a class has been changed
        ///     <example>
        ///     The typical usage would be: 
        ///     <code>private void NotifyStateChange() => OnStateChanged?.Invoke();</code>
        ///     </example>
        /// </summary>
        event Action OnStateChanged;
    }
}
