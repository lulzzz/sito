﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Diagnostics;
using Maddalena.Markdig.Syntax.Inlines;

namespace Maddalena.Markdig.Extensions.TaskLists
{
    /// <summary>
    /// An inline for TaskList.
    /// </summary>
    [DebuggerDisplay("TaskList {Checked}")]
    public class TaskList : LeafInline
    {
        public bool Checked { get; set; }
    }
}