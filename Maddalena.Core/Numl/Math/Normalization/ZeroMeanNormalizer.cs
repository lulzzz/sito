using System;
using Maddalena.Core.Numl.Math.LinearAlgebra;

namespace Maddalena.Core.Numl.Math.Normalization
{
    /// <summary>
    /// Zero-Mean Feature normalizer to scale features to be zero centered.
    /// </summary>
    public class ZeroMeanNormalizer : INormalizer
    {
        /// <summary>
        /// Normalize a row vector using Z-Score normalization on the supplied feature properties.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Vector Normalize(Vector row, Summary properties)
        {
            if (row == null)
            {
                throw new ArgumentNullException("Row was null");
            }
            double[] item = new double[row.Length];
            for (int i = 0; i < row.Length; i++)
            {
                item[i] = (row[i] - properties.Average[i]);
            }
            return item;
        }
    }
}
