using Maddalena.Core.Mongo;

namespace Maddalena.Core.Scripts
{
    public class Script : MongoObject
    {
        public string Name { get; set; }

        public ScriptLanguage Language { get; set; }

        public string Source { get; set; }
    }
}
