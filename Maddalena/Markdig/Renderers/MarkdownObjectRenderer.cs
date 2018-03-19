// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Helpers;
using Maddalena.Markdig.Syntax;

namespace Maddalena.Markdig.Renderers
{
    /// <summary>
    ///     A base class for rendering <see cref="Block" /> and <see cref="Markdig.Syntax.Inlines.Inline" /> Markdown objects.
    /// </summary>
    /// <typeparam name="TRenderer">The type of the renderer.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <seealso cref="Markdig.Renderers.IMarkdownObjectRenderer" />
    public abstract class MarkdownObjectRenderer<TRenderer, TObject> : IMarkdownObjectRenderer
        where TRenderer : RendererBase where TObject : MarkdownObject
    {
        public delegate bool TryWriteDelegate(TRenderer renderer, TObject obj);

        protected MarkdownObjectRenderer()
        {
            TryWriters = new OrderedList<TryWriteDelegate>();
        }

        /// <summary>
        ///     Gets the optional writers attached to this instance.
        /// </summary>
        public OrderedList<TryWriteDelegate> TryWriters { get; }

        public virtual bool Accept(RendererBase renderer, MarkdownObject obj)
        {
            return obj is TObject;
        }

        public virtual void Write(RendererBase renderer, MarkdownObject obj)
        {
            var htmlRenderer = (TRenderer) renderer;
            var typedObj = (TObject) obj;

            // Try processing
            for (int i = 0; i < TryWriters.Count; i++)
            {
                var tryWriter = TryWriters[i];
                if (tryWriter(htmlRenderer, typedObj))
                {
                    return;
                }
            }

            Write(htmlRenderer, typedObj);
        }

        /// <summary>
        ///     Writes the specified Markdown object to the renderer.
        /// </summary>
        /// <param name="renderer">The renderer.</param>
        /// <param name="obj">The markdown object.</param>
        protected abstract void Write(TRenderer renderer, TObject obj);
    }
}