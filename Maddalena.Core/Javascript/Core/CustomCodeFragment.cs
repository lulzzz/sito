using System;

namespace Maddalena.Core.Javascript.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CustomCodeFragment : Attribute
    {
        public CodeFragmentType Type { get; }
        public string[] ReservedWords { get; }

        public CustomCodeFragment()
            : this(CodeFragmentType.Statement)
        { }

        public CustomCodeFragment(CodeFragmentType codeFragmentType)
            : this(codeFragmentType, null)
        {

        }

        public CustomCodeFragment(CodeFragmentType codeFragmentType, params string[] reservedWords)
        {
            Type = codeFragmentType;
            ReservedWords = reservedWords ?? new string[0];
        }
    }
}
