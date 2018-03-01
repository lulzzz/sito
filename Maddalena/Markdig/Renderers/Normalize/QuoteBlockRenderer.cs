// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Syntax;

namespace Maddalena.Markdig.Renderers.Normalize
{
    /// <summary>
    /// A Normalize renderer for a <see cref="QuoteBlock"/>.
    /// </summary>
    /// <seealso cref="Markdig.Renderers.Normalize.NormalizeObjectRenderer{Markdig.Syntax.QuoteBlock}" />
    public class QuoteBlockRenderer : NormalizeObjectRenderer<QuoteBlock>
    {
        protected override void Write(NormalizeRenderer renderer, QuoteBlock obj)
        {
            var quoteIndent = renderer.Options.SpaceAfterQuoteBlock ? obj.QuoteChar + " " : obj.QuoteChar.ToString();
            renderer.PushIndent(quoteIndent);
            renderer.WriteChildren(obj);
            renderer.PopIndent();

            renderer.FinishBlock(true);
        }
    }
}