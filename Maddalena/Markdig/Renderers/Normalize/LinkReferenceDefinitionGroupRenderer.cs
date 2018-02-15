// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Maddalena.Markdig.Syntax;

namespace Maddalena.Markdig.Renderers.Normalize
{
    public class LinkReferenceDefinitionGroupRenderer : NormalizeObjectRenderer<LinkReferenceDefinitionGroup>
    {
        protected override void Write(NormalizeRenderer renderer, LinkReferenceDefinitionGroup obj)
        {
            renderer.EnsureLine();
            renderer.WriteChildren(obj);
            renderer.FinishBlock(false);
        }
    }
}