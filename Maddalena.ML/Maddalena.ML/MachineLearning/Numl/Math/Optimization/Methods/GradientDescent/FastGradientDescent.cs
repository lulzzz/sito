﻿using System;
using System.Linq;
using Maddalena.Numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace Maddalena.Numl.Math.Optimization.Methods.GradientDescent
{
    /// <summary>
    /// A Stochastic Gradient Descent with Momentum method.
    /// </summary>
    public class FastGradientDescent : OptimizationMethod
    {
        /// <summary>
        /// Defines the Momentum to use.
        /// </summary>
        public double Momentum { get; set; }

        /// <summary>
        /// Initializes a new FastGradientDescent object with a default Momentum of 0.9
        /// </summary>
        public FastGradientDescent()
        {
            this.Momentum = 0.9;
        }

        /// <summary>
        /// Update and return the new Theta value.
        /// </summary>
        /// <param name="properties">Properties for the optimization routine.</param>
        /// <returns></returns>
        public override Vector UpdateTheta(Maddalena.Numl.Math.Optimization.OptimizerProperties properties)
        {
            Vector v = (this.Momentum * properties.Theta) - (properties.LearningRate * properties.Gradient);
            return properties.Theta + v;
        }
    }
}
