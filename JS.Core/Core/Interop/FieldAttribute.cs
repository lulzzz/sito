using System;

namespace JS.Core.Core.Interop
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class FieldAttribute : Attribute
    {
    }
}
