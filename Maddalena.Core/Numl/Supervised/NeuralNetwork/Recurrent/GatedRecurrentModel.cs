using System.Linq;
using Maddalena.Core.Numl.Math.Functions;
using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Supervised.NeuralNetwork.Recurrent
{
    /// <summary>
    /// Gated Recurrent Neural Network model.
    /// </summary>
    public class GatedRecurrentModel : Model, ISequenceModel
    {
        /// <summary>Gets or sets the network.</summary>
        /// <value>The network.</value>
        public Network Network { get; set; }

        /// <summary>
        /// Gets or sets the output layer function (i.e. Softmax).
        /// </summary>
        public IFunction OutputFunction { get; set; }

        /// <summary>
        /// Predicts the next label for the given input.
        /// </summary>
        /// <param name="x">State.</param>
        /// <returns></returns>
        public override double Predict(Vector x)
        {
            Vector output = this.PredictSequence(x);
            return (output.Length > 1 ? output.MaxIndex() : output.First());
        }

        /// <summary>
        /// Predicts the next sequence label for the given input.
        /// </summary>
        /// <param name="x">State.</param>
        /// <returns></returns>
        public Vector PredictSequence(Vector x)
        {
            this.Preprocess(x);

            this.Network.Forward(x);

            Vector output = this.Network.Output();
            return output;
        }
    }
}
