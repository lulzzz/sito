﻿using System;
using Maddalena.Security;
using Mongolino;

namespace Maddalena.Modules.Board
{
    public class BoardComment
    {
        public ApplicationUser User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Board { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }
    }
}