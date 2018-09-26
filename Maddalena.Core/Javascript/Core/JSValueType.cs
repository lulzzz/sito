using System;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public enum JSValueType
    {
        NotExists = 0,
        NotExistsInObject = 1,
        Undefined = 3,                          // 000000000011 // значение undefined говорит о том, что этот объект, вообще-то, определён, но вот его значение нет
        Boolean = 4 | Undefined,                // 000000000111
        Integer = 8 | Undefined,                // 000000001011
        Double = 16 | Undefined,                // 000000010011
        String = 32 | Undefined,                // 000000100011
        Symbol = 64 | Undefined,                // 000001000011
        Object = 128 | Undefined,               // 000010000011
        Function = 256 | Undefined,             // 000100000011
        Date = 512 | Undefined,                 // 001000000011
        Property = 1024 | Undefined,            // 010000000011
        SpreadOperatorResult = 2048 | Undefined // 100000000011
    }
}