using System;
using System.Collections.Generic;
using JS.Core.Core;
using JS.Core.Expressions;

namespace NiL.JS.Statements
{
    public sealed class StoreValue : CodeNode
    {
        private readonly Expression _source;

        public override int Position
        {
            get => _source.Position;
            internal set
            {
                _source.Position = value;
            }
        }

        public override int Length
        {
            get => _source.Length;
            internal set
            {
                _source.Length = value;
            }
        }

        public bool ForWrite { get; }

        public StoreValue(Expression source, bool forWrite)
        {
            _source = source;
            ForWrite = forWrite;
        }

        public override JSValue Evaluate(Context context)
        {
            var temp = ForWrite ? _source.EvaluateForWrite(context) : _source.Evaluate(context);

            if (context._executionMode == ExecutionMode.Suspend)
                return null;
            else
                context.SuspendData[_source] = ForWrite ? temp : temp.CloneImpl(false);

            return null;
        }

        public override string ToString()
        {
            return _source.ToString();
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return _source.GetChildsImpl();
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return _source.Visit<T>(visitor);
        }

        public override void Decompose(ref CodeNode self)
        {

        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            _source.RebuildScope(functionInfo, transferedVariables, scopeBias);
        }
    }
}
