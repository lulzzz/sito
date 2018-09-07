﻿// file:	Math\Linkers\SingleLinker.cs
//
// summary:	Implements the single linker class

using System;
using System.Collections.Generic;
using System.Linq;
using Maddalena.Core.Numl.Math.LinearAlgebra;
using Maddalena.Core.Numl.Math.Metrics;

namespace Maddalena.Core.Numl.Math.Linkers
{
    /// <summary>A single linker.</summary>
    public class SingleLinker : ILinker
    {
        /// <summary>The metric.</summary>
        private readonly IDistance _metric;
        /// <summary>Constructor.</summary>
        /// <param name="metric">The metric.</param>
        public SingleLinker(IDistance metric)
        {
            _metric = metric;
        }
        /// <summary>Distances.</summary>
        /// <param name="x">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <param name="y">The IEnumerable&lt;Vector&gt; to process.</param>
        /// <returns>A double.</returns>
        public double Distance(IEnumerable<Vector> x, IEnumerable<Vector> y)
        {
            double distance = -1;
            double leastDistance = Int32.MaxValue;

            for (int i = 0; i < x.Count(); i++)
            {
                for (int j = i + 1; j < y.Count(); j++)
                {
                    distance = _metric.Compute(x.ElementAt(i), y.ElementAt(j));

                    if (distance < leastDistance)
                        leastDistance = distance;
                }
            }

            return leastDistance;
        }
    }
}
