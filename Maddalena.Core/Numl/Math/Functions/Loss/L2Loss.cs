using Maddalena.Core.Numl.Math.LinearAlgebra;
using Maddalena.Core.Numl.Supervised;

namespace Maddalena.Core.Numl.Math.Functions.Loss
{
    public class L2Loss : ILossFunction
    {
        /// <summary>
        /// Computes the L2 loss delta between the two vectors.
        /// </summary>
        /// <param name="x">Predicted values.</param>
        /// <param name="y">Actual values.</param>
        /// <returns>Vector.</returns>
        public double Compute(Vector x, Vector y)
        {
            return Score.ComputeRMSE(y, x);
        }
    }
}
