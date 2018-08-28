using Maddalena.Numl.Math.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maddalena.ML.MachineLearning.Numl.Supervised;

namespace Maddalena.Numl.Supervised
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
