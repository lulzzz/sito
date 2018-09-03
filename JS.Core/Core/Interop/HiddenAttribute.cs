using System;

namespace JS.Core.Core.Interop
{
    /// <summary>
    /// Член, помеченный данным аттрибутом, не будет доступен из сценария.
    /// </summary>
#if !(PORTABLE)
    [Serializable]
#endif
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class HiddenAttribute : Attribute
    {
    }
}
