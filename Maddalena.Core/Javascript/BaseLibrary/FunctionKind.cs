using System;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    /// <summary>
    /// Возможные типы функции в контексте использования.
    /// </summary>
    [Serializable]
    public enum FunctionKind
    {
        Function = 0,
        Getter,
        Setter,
        AnonymousFunction,
        AnonymousGenerator,
        Generator,
        Method,
        MethodGenerator,
        Arrow,
        AsyncFunction,
        AsyncAnonymousFunction,
        AsyncArrow
    }
}