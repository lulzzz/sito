using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Supervised
{
    /// <summary>
    /// Implements a Sequence model.
    /// </summary>
    public interface ISequenceModel : IModel
    {
        /// <summary>
        /// Predicts the given example.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        Vector PredictSequence(Vector x);
    }
}
