﻿namespace Maddalena.ML.MachineLearning.Numl.Genetic.Metrics
{
    /// <summary>
    /// IFitnessMetric interface.
    /// </summary>
    public interface IFitnessMetric
    {
        /// <summary>
        /// Compares the chromosome against a signal for evaluating its fitness score.
        /// </summary>
        /// <param name="chromosome">Chromosome to evaluate.</param>
        /// <returns>double.</returns>
        double Fitness(IChromosome chromosome);
    }
}
