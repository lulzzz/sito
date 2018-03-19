// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Syntax.Inlines;

namespace Maddalena.Markdig.Renderers.Normalize.Inlines
{
    /// <summary>
    ///     A Normalize renderer for an <see cref="AutolinkInline" />.
    /// </summary>
    /// <seealso cref="Markdig.Renderers.Normalize.NormalizeObjectRenderer{Markdig.Syntax.Inlines.AutolinkInline}" />
    public class AutolinkInlineRenderer : NormalizeObjectRenderer<AutolinkInline>
    {
        protected override void Write(NormalizeRenderer renderer, AutolinkInline obj)
        {
            renderer.Write('<').Write(obj.Url).Write('>');
        }
    }
}