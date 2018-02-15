using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.Mongo;
using Mongolino;

namespace Maddalena.Modules.Board
{
    public class BoardPost : DBObject<BoardPost>
    {
        static BoardPost()
        {
            DescendingIndex(x => x.Timestamp);
            DescendingIndex(x => x.Board);
        }

        public ObjectRef<MongoIdentityUser> User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Board { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }

        public decimal Views { get; set; }

        public List<BoardComment> Comments { get; set; } = new List<BoardComment>();
    }
}