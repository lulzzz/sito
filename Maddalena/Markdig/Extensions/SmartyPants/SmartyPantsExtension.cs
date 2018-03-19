﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Parsers.Inlines;
using Maddalena.Markdig.Renderers;

namespace Maddalena.Markdig.Extensions.SmartyPants
{
    /// <summary>
    ///     Extension to enable SmartyPants.
    /// </summary>
    public class SmartyPantsExtension : IMarkdownExtension
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SmartyPantsExtension" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SmartyPantsExtension(SmartyPantOptions options)
        {
            Options = options ?? new SmartyPantOptions();
        }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        public SmartyPantOptions Options { get; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<SmartyPantsInlineParser>())
            {
                // Insert the parser after the code span parser
                pipeline.InlineParsers.InsertAfter<CodeInlineParser>(new SmartyPantsInlineParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var htmlRenderer = renderer as HtmlRenderer;
            if (htmlRenderer != null)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<HtmlSmartyPantRenderer>())
                {
                    htmlRenderer.ObjectRenderers.Add(new HtmlSmartyPantRenderer(Options));
                }
            }
        }
    }
}