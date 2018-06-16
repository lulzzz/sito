﻿using System;
using System.Collections.Generic;
using Maddalena.Security;
using Mongolino;

namespace Maddalena.Models.Board
{
    public class BoardPost : DBObject<BoardPost>
    {
        static BoardPost()
        {
            DescendingIndex(x => x.Timestamp);
            DescendingIndex(x => x.Board);
        }

        public ApplicationUser User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Board { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }

        public decimal Views { get; set; }

        public List<BoardComment> Comments { get; set; } = new List<BoardComment>();
    }
}