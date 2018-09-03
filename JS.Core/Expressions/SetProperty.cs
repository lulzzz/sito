using System;
using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class SetProperty : Expression
    {
        private readonly JSValue tempContainer1;
        private readonly JSValue tempContainer2;
        private readonly JSValue cachedMemberName;

        public Expression Source => _left;
        public Expression FieldName => _right;
        public Expression Value { get; private set; }

        protected internal override bool ContextIndependent => false;

        internal override bool ResultInTempContainer => true;

        internal SetProperty(Expression obj, Expression fieldName, Expression value)
            : base(obj, fieldName, true)
        {
            if (fieldName is Constant)
                cachedMemberName = fieldName.Evaluate(null);
            else
                tempContainer1 = new JSValue();
            Value = value;
            tempContainer2 = new JSValue();
        }

        public override JSValue Evaluate(Context context)
        {
            lock (this)
            {
                JSValue sjso = null;
                JSValue source = null;
                source = _left.Evaluate(context);
                if (source._valueType >= JSValueType.Object
                    && source._oValue != null
                    && source._oValue != source
                    && (sjso = source._oValue as JSValue) != null
                    && sjso._valueType >= JSValueType.Object)
                {
                    source = sjso;
                    sjso = null;
                }
                else
                {
                    tempContainer2.Assign(source);
                    source = tempContainer2;
                }

                source.SetProperty(
                    cachedMemberName ?? safeGet(tempContainer1, _right, context),
                    safeGet(_tempContainer, Value, context),
                    context._strict);

                context._objectSource = null;
                return _tempContainer;
            }
        }

        private static JSValue safeGet(JSValue temp, CodeNode source, Context context)
        {
            temp.Assign(source.Evaluate(context));
            return temp;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            _codeContext = codeContext;
            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            var cn = Value as CodeNode;
            Value.Optimize(ref cn, owner, message, opts, stats);
            Value = cn as Expression;
            base.Optimize(ref _this, owner, message, opts, stats);
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            base.RebuildScope(functionInfo, transferedVariables, scopeBias);

            Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return new CodeNode[] { _left, _right, Value };
        }

        public override string ToString()
        {
            var res = _left.ToString();
            int i = 0;
            var cn = _right as Constant;
            if (_right is Constant
                && cn.value.ToString().Length > 0
                && (Parser.ValidateName(cn.value.ToString(), ref i, true)))
                res += "." + cn.value;
            else
                res += "[" + _right + "]";
            return res + " = " + Value;
        }
    }
}