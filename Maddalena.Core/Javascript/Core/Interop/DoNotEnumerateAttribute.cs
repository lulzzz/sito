using System;

namespace Maddalena.Core.Javascript.Core.Interop
{
    /// <summary>
    /// Указывает, что помеченный член следует пропустить при перечислении в конструкции for-in
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, Inherited = false)]
    public sealed class DoNotEnumerateAttribute : Attribute
    {

    }
}
