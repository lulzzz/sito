﻿namespace Maddalena.Core.Numl.Genetic.Functions.Mutation
{
    /// <summary>
    /// Random flip mutation function.
    /// <para>Randomly flips values in the genetic sequence, from zero to one and vice versa.</para>
    /// </summary>
    public class FlipMutation : MutationBase
    {
        /// <summary>
        /// Mutates the chromosome using random flips.
        /// </summary>
        /// <param name="chromosome">Chromosome to mutate.</param>
        /// <returns>IChromosome.</returns>
        public override IChromosome Mutate(IChromosome chromosome)
        {
            var clone = chromosome.Clone();

            double val = clone.Sequence[this.N];
            clone.Sequence[this.N] = (val > 0 ? val - 1 : val + 1);

            return clone;
        }
    }
}
