﻿using System.Collections.Generic;
using Maddalena.Core.Numl.Genetic.Metrics;

namespace Maddalena.Core.Numl.Genetic.Algorithms
{
    /// <summary>
    /// IGeneticAlgorithm interface.
    /// </summary>
    public interface IGeneticAlgorithm
    {
        /// <summary>
        /// Evolves a given population of chromosomes using a genetic algorithm.
        /// </summary>
        /// <param name="chromosomes">Chromosomes to undergo evolution.</param>
        /// <param name="fitnessMetric">Fitness metric to use for evaluation.</param>
        /// <param name="fitnessMode">Fitness mode for evaluating performance.</param>
        IEnumerable<IChromosome> Evolve(IEnumerable<IChromosome> chromosomes, IFitnessMetric fitnessMetric, FitnessMode fitnessMode);
    }
}
