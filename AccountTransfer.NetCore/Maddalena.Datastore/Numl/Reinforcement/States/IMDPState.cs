using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Maddalena.Numl.AI;
using Maddalena.Numl.Math.LinearAlgebra;

namespace Maddalena.Numl.Reinforcement
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
