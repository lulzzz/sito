// file:	Math\Linkers\CentroidLinker.cs
//
// summary:	Implements the centroid linker class

using System.Collections.Generic;
using Maddalena.Core.Numl.Math.LinearAlgebra;
using Maddalena.Core.Numl.Math.Metrics;

namespace Maddalena.Core.Numl.Math.Linkers
{
    /// <summary>A centroid linker.</summary>
    public class CentroidLinker : ILinker
    {
        /// <summary>The metric.</summary>
        private readonly IDistance _metric;
        /// <summary>Constructor.</summary>
        /// <param name="metric">The metric.</param>
        public CentroidLinker(IDistance metric)
        {
            _metric = metric;
        }
        /// <summary>Distances.</summary>
        /// <param name="x">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <param name="y">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <returns>A double.</returns>
        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            return _metric.Compute(x.Mean(), y.Mean());
        }
    }
}