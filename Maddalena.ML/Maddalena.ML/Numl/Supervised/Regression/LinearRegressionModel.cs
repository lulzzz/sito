using System;
using System.Linq;
using Maddalena.Numl.Math.LinearAlgebra;
using System.Collections.Generic;

namespace Maddalena.Numl.Supervised.Regression
{
    /// <summary>
    /// Linear Regression model
    /// </summary>
    public class LinearRegressionModel : Model
    {
        /// <summary>
        /// Theta parameters vector mapping X to y.
        /// </summary>
        public Vector Theta { get; set; }

        /// <summary>
        /// Initialises a new LinearRegressionModel object
        /// </summary>
        public LinearRegressionModel() { }

        /// <summary>
        /// Create a prediction based on the learned Theta values and the supplied test item.
        /// </summary>
        /// <param name="x">Training record</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            this.Preprocess(x);

            return x.Insert(0, 1.0, false).Dot(Theta);
        }
    }
}
