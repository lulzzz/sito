﻿// file:	Math\Metrics\IDistance.cs
//
// summary:	Declares the IDistance interface

using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Math.Metrics
{
    /// <summary>Interface for distance.</summary>
    public interface IDistance
    {
        /// <summary>Computes.</summary>
        /// <param name="x">The Vector to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>A double.</returns>
        double Compute(Vector x, Vector y);
    }
}
