// file:	Math\Linkers\ILinker.cs
//
// summary:	Declares the ILinker interface

using System.Collections.Generic;
using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Math.Linkers
{
    /// <summary>Interface for linker.</summary>
    public interface ILinker
    {
        /// <summary>Distances.</summary>
        /// <param name="x">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <param name="y">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <returns>A double.</returns>
        double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y);
    }
}
