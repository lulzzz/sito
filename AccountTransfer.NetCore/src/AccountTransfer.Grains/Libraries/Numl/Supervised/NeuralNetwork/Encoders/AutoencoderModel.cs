using numl.Math.LinearAlgebra;

namespace numl.Supervised.NeuralNetwork.Encoders
{
    /// <summary>
    /// An Autoencoder model.
    /// </summary>
    public class AutoencoderModel : NeuralNetworkModel, ISequenceModel
    {
        /// <summary>
        /// Autoencodes the given example.
        /// </summary>
        /// <param name="x">The Vector to process.</param>
        /// <returns>An object.</returns>
        public override double Predict(Vector x)
        {
            Vector output = this.PredictSequence(x);
            return output.Sum();
        }

        /// <summary>
        /// Autoencodes the given example.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override Vector PredictSequence(Vector x)
        {
            this.Preprocess(x);

            this.Network.Forward(x);

            Vector output = this.Network.Output();
            return output;
        }
    }
}
