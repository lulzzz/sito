// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Renderers;
using Maddalena.Markdig.Renderers.Html;

namespace Maddalena.Markdig.Extensions.Abbreviations
{
    /// <summary>
    /// A HTML renderer for a <see cref="AbbreviationInline"/>.
    /// </summary>
    /// <seealso cref="Markdig.Renderers.Html.HtmlObjectRenderer{CustomContainer}" />
    public class HtmlAbbreviationRenderer : HtmlObjectRenderer<AbbreviationInline>
    {
        protected override void Write(HtmlRenderer renderer, AbbreviationInline obj)
        {
            // <abbr title="Hyper Text Markup Language">HTML</abbr>
            var abbr = obj.Abbreviation;
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<abbr").WriteAttributes(obj).Write(" title=\"").WriteEscape(ref abbr.Text).Write("\">");
            }
            renderer.Write(abbr.Label);
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("</abbr>");
            }
        }
    }
}