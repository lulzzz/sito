// file:	Supervised\IGenerator.cs
//
// summary:	Declares the IGenerator interface

using System.Collections.Generic;
using Maddalena.Core.Numl.Math.LinearAlgebra;
using Maddalena.Core.Numl.Model;

namespace Maddalena.Core.Numl.Supervised
{
    /// <summary>Interface for generator.</summary>
    public interface IGenerator : IModelBase
    {
        /// <summary>Generates.</summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="examples">The examples.</param>
        /// <returns>An IModel.</returns>
        IModel Generate(Descriptor descriptor, IEnumerable<object> examples);
        /// <summary>Generates the given examples.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="descriptor">Descriptor</param>
        /// <param name="examples">The examples.</param>
        /// <returns>An IModel.</returns>
        IModel Generate<T>(Descriptor descriptor, IEnumerable<T> examples) where T : class;
        /// <summary>Generates.</summary>
        /// <param name="x">The Matrix to process.</param>
        /// <param name="y">The Vector to process.</param>
        /// <returns>An IModel.</returns>
        IModel Generate(Matrix x, Vector y);
    }
}
