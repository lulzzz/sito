// file:	Math\Metrics\EuclidianDistance.cs
//
// summary:	Implements the euclidian distance class

using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Math.Metrics
{
    /// <summary>An euclidian distance.</summary>
    public sealed class EuclidianDistance : IDistance
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        public double Compute(Vector x, Vector y)
        {
            return (x - y).Norm();
        }
    }
}
