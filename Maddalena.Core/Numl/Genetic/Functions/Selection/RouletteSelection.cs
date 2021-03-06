﻿using System.Collections.Generic;
using System.Linq;
using Maddalena.Core.Numl.Genetic.Metrics;
using Maddalena.Core.Numl.Math.Filters;
using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Genetic.Functions.Selection
{
    /// <summary>
    /// RouletteSelection class.
    /// </summary>
    public class RouletteSelection : ISelectionFunction
    {
        private readonly IFilter _Filter;

        /// <summary>
        /// Initializes a new RouletteSelection object.
        /// </summary>
        public RouletteSelection()
        {
            this._Filter = new ProportionalFilter();
        }

        /// <summary>
        /// Applies Roulette Wheel sampling to the chromosomes.
        /// </summary>
        /// <param name="pool">Pool of chromosomes to select from.</param>
        /// <returns></returns>
        public IEnumerable<IChromosome> Select(IEnumerable<IChromosome> pool, FitnessMode fitnessMode)
        {
            Vector weights = pool.Select(s => s.Weight).ToVector();

            return this._Filter.Filter(pool, weights);
        }
    }
}
