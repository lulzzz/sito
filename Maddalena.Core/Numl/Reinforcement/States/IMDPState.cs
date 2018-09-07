using Maddalena.Core.Numl.AI;
using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Reinforcement.States
{
    /// <summary>
    /// IMDPState interface.
    /// </summary>
    public interface IMDPState : IState
    {
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        Vector Features { get; set; }
    }
}
