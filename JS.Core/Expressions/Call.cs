using System;
using System.Collections.Generic;
using JS.Core.Core;
using JS.Core.Core.Interop;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Statements;

namespace JS.Core.Expressions
{
    public enum CallMode
    {
        Regular = 0,
        Construct,
        Super
    }

    [Serializable]
    public sealed class Call : Expression
    {
        internal bool withSpread;
        internal bool allowTCO;
        internal CallMode _callMode;

        public CallMode CallMode => _callMode;
        protected internal override bool ContextIndependent => false;
        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType
        {
            get
            {

                if (_left is VariableReference reference)
                {
                    var desc = reference._descriptor;
                    if (desc.initializer is FunctionDefinition fe)
                        return fe._functionInfo.ResultType; // для рекурсивных функций будет Unknown
                }

                return PredictedType.Unknown;
            }
        }
        public Expression[] Arguments { get; }
        public bool AllowTCO => allowTCO && _callMode == 0;

        protected internal override bool NeedDecompose
        {
            get
            {
                if (_left.NeedDecompose)
                    return true;

                for (var i = 0; i < Arguments.Length; i++)
                {
                    if (Arguments[i].NeedDecompose)
                        return true;
                }

                return false;
            }
        }

        internal Call(Expression first, Expression[] arguments)
            : base(first, null, false)
        {
            this.Arguments = arguments;
        }

        public override JSValue Evaluate(Context context)
        {
            var temp = _left.Evaluate(context);
            JSValue targetObject = context._objectSource;
            ICallable callable = null;
            Function func = null;

            if (temp._valueType >= JSValueType.Object)
            {
                if (temp._valueType == JSValueType.Function)
                {
                    func = temp._oValue as Function;
                    callable = func;
                }

                if (func == null)
                {
                    callable = temp._oValue as ICallable;
                    if (callable == null)
                        callable = temp.Value as ICallable;
                    if (callable == null)
                    {
                        if (temp.Value is Proxy typeProxy)
                            callable = typeProxy.PrototypeInstance as ICallable;
                    }
                }
            }

            if (callable == null)
            {
                for (int i = 0; i < this.Arguments.Length; i++)
                {
                    context._objectSource = null;
                    this.Arguments[i].Evaluate(context);
                }

                context._objectSource = null;

                // Аргументы должны быть вычислены даже если функция не существует.
                ExceptionHelper.ThrowTypeError(_left.ToString() + " is not a function");

                return null;
            }
            else if (func == null)
            {
                checkStack();
                Context.CurrentGlobalContext._callDepth++;
                try
                {
                    switch (_callMode)
                    {
                        case CallMode.Construct:
                            {
                                return callable.Construct(Tools.CreateArguments(Arguments, context));
                            }
                        case CallMode.Super:
                            {
                                return callable.Construct(targetObject, Tools.CreateArguments(Arguments, context));
                            }
                        default:
                            return callable.Call(targetObject, Tools.CreateArguments(Arguments, context));
                    }
                }
                finally
                {
                    Context.CurrentGlobalContext._callDepth--;
                }
            }
            else
            {
                if (allowTCO
                    && _callMode == 0
                    && (func._functionDefinition.kind != FunctionKind.Generator)
                    && (func._functionDefinition.kind != FunctionKind.MethodGenerator)
                    && (func._functionDefinition.kind != FunctionKind.AnonymousGenerator)
                    && context._owner != null
                    && func == context._owner._oValue)
                {
                    tailCall(context, func);
                    context._objectSource = targetObject;
                    return JSValue.undefined;
                }
                else
                    context._objectSource = null;

                checkStack();
                Context.CurrentGlobalContext._callDepth++;
                try
                {
                    if (_callMode == CallMode.Construct)
                        targetObject = null;

                    if ((temp._attributes & JSValueAttributesInternal.Eval) != 0)
                        return callEval(context);

                    return func.InternalInvoke(targetObject, Arguments, context, withSpread, _callMode != 0);
                }
                finally
                {
                    Context.CurrentGlobalContext._callDepth--;
                }
            }
        }

        private JSValue callEval(Context context)
        {
            if (_callMode != CallMode.Regular)
                ExceptionHelper.ThrowTypeError("function eval(...) cannot be called as a constructor");

            if (Arguments == null || Arguments.Length == 0)
                return JSValue.NotExists;

            var evalCode = Arguments[0].Evaluate(context);

            for (int i = 1; i < this.Arguments.Length; i++)
            {
                context._objectSource = null;
                this.Arguments[i].Evaluate(context);
            }

            if (evalCode._valueType != JSValueType.String)
                return evalCode;

            return context.Eval(evalCode.ToString(), false);
        }

        private void tailCall(Context context, Function func)
        {
            context._executionMode = ExecutionMode.TailRecursion;

            var arguments = new Arguments(context);

            for (int i = 0; i < this.Arguments.Length; i++)
                arguments.Add(Tools.EvalExpressionSafe(context, Arguments[i]));
            context._objectSource = null;

            arguments.callee = func;
            context._executionInfo = arguments;
        }

        private static void checkStack()
        {
            if (Context.CurrentGlobalContext._callDepth >= 1000)
                throw new JSException(new RangeError("Stack overflow."));
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (stats != null)
                stats.UseCall = true;

            this._codeContext = codeContext;

            if (_left is Super super)
            {
                super.ctorMode = true;
                _callMode = CallMode.Super;
            }

            for (var i = 0; i < Arguments.Length; i++)
            {
                Parser.Build(ref Arguments[i], expressionDepth + 1, variables, codeContext | CodeContext.InExpression, message, stats, opts);
            }

            base.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
            if (_left is Variable)
            {
                var name = _left.ToString();
                if (name == "eval" && stats != null)
                {
                    stats.ContainsEval = true;
                    foreach (var variable in variables)
                    {
                        variable.Value.captured = true;
                    }
                }
                VariableDescriptor f = null;
                if (variables.TryGetValue(name, out f) && f.initializer is FunctionDefinition func)
                {
                    for (var i = 0; i < func.parameters.Length; i++)
                    {
                        if (i >= Arguments.Length)
                            break;
                        if (func.parameters[i].lastPredictedType == PredictedType.Unknown)
                            func.parameters[i].lastPredictedType = Arguments[i].ResultType;
                        else if (Tools.CompareWithMask(func.parameters[i].lastPredictedType, Arguments[i].ResultType, PredictedType.Group) != 0)
                            func.parameters[i].lastPredictedType = PredictedType.Ambiguous;
                    }
                }
            }
            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            base.Optimize(ref _this, owner, message, opts, stats);
            for (var i = Arguments.Length; i-- > 0;)
            {
                var cn = Arguments[i] as CodeNode;
                cn.Optimize(ref cn, owner, message, opts, stats);
                Arguments[i] = cn as Expression;
            }
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            var result = new CodeNode[Arguments.Length + 1];
            result[0] = _left;
            Arguments.CopyTo(result, 1);
            return result;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {
            _left.Decompose(ref _left, result);

            var lastDecomposeIndex = -1;
            for (var i = 0; i < Arguments.Length; i++)
            {
                Arguments[i].Decompose(ref Arguments[i], result);
                if (Arguments[i].NeedDecompose)
                {
                    lastDecomposeIndex = i;
                }
            }

            for (var i = 0; i < lastDecomposeIndex; i++)
            {
                if (!(Arguments[i] is ExtractStoredValue))
                {
                    result.Add(new StoreValue(Arguments[i], false));
                    Arguments[i] = new ExtractStoredValue(Arguments[i]);
                }
            }
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            base.RebuildScope(functionInfo, transferedVariables, scopeBias);

            for (var i = 0; i < Arguments.Length; i++)
                Arguments[i].RebuildScope(functionInfo, transferedVariables, scopeBias);
        }

        public override string ToString()
        {
            string res = _left + "(";
            for (int i = 0; i < Arguments.Length; i++)
            {
                res += Arguments[i];
                if (i + 1 < Arguments.Length)
                    res += ", ";
            }
            res += ")";

            if (_callMode == CallMode.Construct)
                return "new " + res;
            return res;
        }
    }
}