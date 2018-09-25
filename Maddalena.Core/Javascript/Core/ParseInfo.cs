using System.Collections.Generic;

namespace Maddalena.Core.Javascript.Core
{
    public sealed class ParseInfo
    {
        public readonly string SourceCode;
        public readonly Stack<bool> AllowBreak;
        public readonly Stack<bool> AllowContinue;
        public readonly List<VariableDescriptor> Variables;
        public readonly Dictionary<string, JSValue> StringConstants;
        public readonly Dictionary<int, JSValue> IntConstants;
        public readonly Dictionary<double, JSValue> DoubleConstants;

        public List<string> Labels;
        public int LabelsCount;
        public int AllowReturn;
        public int LexicalScopeLevel;
        public int FunctionScopeLevel;
        public CodeContext CodeContext;
        public bool Strict;
        public string Code;
        public bool AllowDirectives;
        public int BreaksCount;
        public int ContiniesCount;

        public readonly InternalCompilerMessageCallback Message;

        public ParseInfo(string code, string sourceCode, InternalCompilerMessageCallback message)
        {
            Code = code;
            SourceCode = sourceCode;
            Labels = new List<string>();
            AllowDirectives = true;
            AllowBreak = new Stack<bool>();
            AllowBreak.Push(false);
            AllowContinue = new Stack<bool>();
            AllowContinue.Push(false);
            this.Message = message;
            StringConstants = new Dictionary<string, JSValue>();
            IntConstants = new Dictionary<int, JSValue>();
            DoubleConstants = new Dictionary<double, JSValue>();
            Variables = new List<VariableDescriptor>();
        }

        internal JSValue GetCachedValue(int value)
        {
            if (!IntConstants.ContainsKey(value))
            {
                JSValue jsvalue = value;
                IntConstants[value] = jsvalue;
                return jsvalue;
            }

            return IntConstants[value];
        }
    }
}