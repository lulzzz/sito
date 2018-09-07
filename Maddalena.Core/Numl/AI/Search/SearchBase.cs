using System;

namespace Maddalena.Core.Numl.AI.Search
{
    /// <summary>
    /// Class Search.
    /// </summary>
    public abstract class SearchBase
    {
        /// <summary>
        /// Occurs when [successor expanded].
        /// </summary>
        public event EventHandler<StateExpansionEventArgs> SuccessorExpanded;

        /// <summary>
        /// Handles the <see cref="E:SuccessorExpanded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StateExpansionEventArgs&lt;T&gt;"/> instance containing the event data.</param>
        protected virtual void OnSuccessorExpanded(object sender, StateExpansionEventArgs e)
        {
            SuccessorExpanded?.Invoke(sender, e);
        }
    }

}