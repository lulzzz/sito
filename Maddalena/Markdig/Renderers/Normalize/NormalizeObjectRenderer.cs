// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Syntax;

namespace Maddalena.Markdig.Renderers.Normalize
{
    /// <summary>
    /// A base class for Normalize rendering <see cref="Block"/> and <see cref="Markdig.Syntax.Inlines.Inline"/> Markdown objects.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <seealso cref="Markdig.Renderers.IMarkdownObjectRenderer" />
    public abstract class NormalizeObjectRenderer<TObject> : MarkdownObjectRenderer<NormalizeRenderer, TObject> where TObject : MarkdownObject
    {
    }
}