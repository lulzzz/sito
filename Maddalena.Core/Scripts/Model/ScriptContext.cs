using System;

namespace Maddalena.Core.Scripts.Model
{
    public class ScriptContext
    {
        public Script Script { get; set; }

        public SystemInterface SystemInterface { get; set; }

        public Exception Exception { get; set; }
    }
}
