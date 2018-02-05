using System;
using Maddalena.Identity;
using Maddalena.Mongo;

namespace Maddalena.Modules.Board
{
    public class BoardComment
    {
        public ObjectRef<ApplicationUser> User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Board { get; set; }

        public string Body { get; set; }

        public string Extra { get; set; }
    }
}