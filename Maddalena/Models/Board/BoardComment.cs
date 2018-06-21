﻿using System;
using Maddalena.Security;

namespace Maddalena.Models.Board
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