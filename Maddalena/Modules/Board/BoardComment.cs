using System;
using Microsoft.AspNetCore.Identity.Mongo;
using Mongolino;

namespace Maddalena.Modules.Board
{
    public class BoardComment
    {
        public ObjectRef<MongoIdentityUser> User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Board { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }
    }
}