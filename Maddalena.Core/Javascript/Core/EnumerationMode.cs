using System;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public enum EnumerationMode
    {
        KeysOnly = 0,
        RequireValues,
        RequireValuesForWrite
    }
}