﻿using System;
using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class Constant : Expression
    {
        internal JSValue value;

        public JSValue Value => value;

        protected internal override PredictedType ResultType
        {
            get
            {
                if (value == null)
                    return PredictedType.Unknown;
                switch (value._valueType)
                {
                    case JSValueType.Undefined:
                    case JSValueType.NotExists:
                    case JSValueType.NotExistsInObject:
                        return PredictedType.Undefined;
                    case JSValueType.Boolean:
                        return PredictedType.Bool;
                    case JSValueType.Integer:
                        return PredictedType.Int;
                    case JSValueType.Double:
                        return PredictedType.Double;
                    case JSValueType.String:
                        return PredictedType.String;
                    default:
                        return PredictedType.Object;
                }
            }
        }

        internal override bool ResultInTempContainer => false;

        protected internal override bool ContextIndependent => true;

        public Constant(JSValue value)
            : base(null, null, false)
        {
            this.value = value;
        }

        public override JSValue Evaluate(Context context)
        {
            return value;
        }

        protected internal override JSValue EvaluateForWrite(Context context)
        {
            return value == JSValue.undefined ? value : base.EvaluateForWrite(context);
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return value?._oValue as CodeNode[];
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            _codeContext = codeContext;

            if ((opts & Options.SuppressUselessExpressionsElimination) == 0 && expressionDepth <= 1)
            {
                _this = null;
                Eliminated = true;
                if (message != null && (value._valueType != JSValueType.String || value._oValue.ToString() != "use strict"))
                    message(MessageLevel.Warning, Position, Length, "Unused constant was removed. Maybe, something missing.");
            }

            return false;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            if (value == null)
                return "";
            if (value._valueType == JSValueType.String)
                return "\"" + value._oValue + "\"";
            if (value._oValue is CodeNode[])
            {
                string res = "";
                for (var i = (value._oValue as CodeNode[]).Length; i-- > 0;)
                    res = (i != 0 ? ", " : "") + (value._oValue as CodeNode[])[i] + res;
                return res;
            }
            return value.ToString();
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {

        }
    }
}