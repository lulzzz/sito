﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Extensions.Figures;
using Maddalena.Markdig.Extensions.Tables;
using Maddalena.Markdig.Renderers;
using Maddalena.Markdig.Renderers.Html;
using Maddalena.Markdig.Syntax;
using Maddalena.Markdig.Syntax.Inlines;

namespace Maddalena.Markdig.Extensions.Bootstrap
{
    /// <summary>
    ///     Extension for tagging some HTML elements with bootstrap classes.
    /// </summary>
    /// <seealso cref="Markdig.IMarkdownExtension" />
    public class BootstrapExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Make sure we don't have a delegate twice
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            foreach (var node in document.Descendants())
            {
                if (node is Block)
                {
                    if (node is Table)
                    {
                        node.GetAttributes().AddClass("table");
                    }
                    else if (node is QuoteBlock)
                    {
                        node.GetAttributes().AddClass("blockquote");
                    }
                    else if (node is Figure)
                    {
                        node.GetAttributes().AddClass("figure");
                    }
                    else if (node is FigureCaption)
                    {
                        node.GetAttributes().AddClass("figure-caption");
                    }
                }
                else if (node is Inline)
                {
                    var link = node as LinkInline;
                    if (link != null && link.IsImage)
                    {
                        link.GetAttributes().AddClass("img-fluid");
                    }
                }
            }
        }
    }
}