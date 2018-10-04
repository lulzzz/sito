using System;
using Maddalena.Core.GridFs;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Scripts
{
    public class Script : MongoObject
    {
        public string Name { get; set; }

        public ScriptLanguage Language { get; set; }

        public DateTime LastModified { get; set; }

        public bool Visible { get; set; }

        public string Author { get; set; }

        public string Source { get; set; }
    }
}
