// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Helpers;
using Maddalena.Markdig.Syntax.Inlines;

namespace Maddalena.Markdig.Extensions.Mathematics
{
    /// <summary>
    ///     A math inline element.
    /// </summary>
    /// <seealso cref="Markdig.Syntax.Inlines.EmphasisInline" />
    public class MathInline : LeafInline
    {
        /// <summary>
        ///     The content as a <see cref="StringSlice" />.
        /// </summary>
        public StringSlice Content;

        /// <summary>
        ///     Gets or sets the delimiter character used by this code inline.
        /// </summary>
        public char Delimiter { get; set; }

        /// <summary>
        ///     Gets or sets the delimiter count.
        /// </summary>
        public int DelimiterCount { get; set; }
    }
}