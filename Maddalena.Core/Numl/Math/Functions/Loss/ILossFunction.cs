using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Math.Functions.Loss
{
    /// <summary>
    /// Delta loss function interface.
    /// </summary>
    public interface ILossFunction
    {
        /// <summary>
        /// Computes the delta between the two vectors.
        /// </summary>
        /// <param name="x">Predicted values.</param>
        /// <param name="y">Actual values.</param>
        /// <returns>double.</returns>
        double Compute(Vector x, Vector y);
    }
}
