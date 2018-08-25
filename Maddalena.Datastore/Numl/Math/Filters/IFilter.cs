using System;
using System.Collections.Generic;
using System.Text;

using Maddalena.Numl.Math.LinearAlgebra;

namespace Maddalena.Numl.Math.Filters
{
    /// <summary>
    /// IFilter interface.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Filters the objects based on their corresponding weights.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="objects">Object array to filter.</param>
        /// <param name="zeta">Corresponding weights for each object.</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        IEnumerable<T> Filter<T>(IEnumerable<T> objects, Vector zeta);
    }
}
