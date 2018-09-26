using System;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public enum PropertyScope
    {
        Common = 0,
        Own = 1,
        Super = 2,
        PrototypeOfSuperclass = 3
    }
}