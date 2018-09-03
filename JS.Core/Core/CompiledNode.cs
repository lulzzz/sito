using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JS.Core.Core.JIT;
using JS.Core.Expressions;
using NiL.JS;
using Expression = JS.Core.Expressions.Expression;

namespace JS.Core.Core
{
    [Serializable]
    public sealed class CompiledNode : Expression
    {
        private static readonly MethodInfo WrapMethod = typeof(JITHelpers).GetMethod("wrap", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly ParameterExpression WrapContainerParameter = System.Linq.Expressions.Expression.Parameter(typeof(JSValue), "wrapContainer");

        private static readonly ParameterExpression[] LambdaArgs = {
                    JITHelpers.ContextParameter,
                    JITHelpers.DynamicValuesParameter,
                    WrapContainerParameter
                };

        private readonly CodeNode[] _dynamicValues;
        private Func<Context, CodeNode[], JSValue, JSValue> _compiledTree;
        private System.Linq.Expressions.Expression _tree;

        public CodeNode Original { get; }

        protected internal override bool ContextIndependent => Original is Expression && ((Expression) Original).ContextIndependent;

        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType => (Original as Expression)?.ResultType ?? PredictedType.Unknown;

        public override int Length
        {
            get => Original.Length;
            internal set => Original.Length = value;
        }

        public override int Position
        {
            get => Original.Position;
            internal set => Original.Position = value;
        }

        public CompiledNode(CodeNode original, System.Linq.Expressions.Expression tree, CodeNode[] dynamicValues)
            : base((original as Expression)?._left, (original as Expression)?._right, (original is Expression) && (original as Expression)._tempContainer == null)
        {
            if (_tempContainer == null)
                _tempContainer = (original as Expression)._tempContainer;
            Original = original;
            _tree = tree;
            _dynamicValues = dynamicValues;
        }

        public CompiledNode(Expression original, System.Linq.Expressions.Expression tree, CodeNode[] dynamicValues)
            : base(original._left, original._right, original._tempContainer == null)
        {
            if (_tempContainer == null)
                _tempContainer = original._tempContainer;
            Original = original;
            _tree = tree;
            _dynamicValues = dynamicValues;
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return Original.Childs;
        }

        public override JSValue Evaluate(Context context)
        {
            if (_compiledTree == null)
            {
                System.Linq.Expressions.Expression tree;
                _tree = _tree.Reduce();
                if (Original is Expression)
                {
                    tree = typeof(JSValue).IsAssignableFrom(_tree.Type) ? _tree : System.Linq.Expressions.Expression.Call(WrapMethod.MakeGenericMethod(_tree.Type), _tree, WrapContainerParameter);
                }
                else
                {
                    tree = System.Linq.Expressions.Expression.Block(_tree, JITHelpers.UndefinedConstant);
                }
                _compiledTree = System.Linq.Expressions.Expression.Lambda<Func<Context, CodeNode[], JSValue, JSValue>>(tree, LambdaArgs).Compile();
            }
            var result = _compiledTree(context, _dynamicValues, _tempContainer);
            return result;
        }

        protected internal override JSValue EvaluateForWrite(Context context)
        {
            return Original.EvaluateForWrite(context);
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            return Original.Build(ref _this, expressionDepth, variables, codeContext, message, stats, opts);
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            Original.Optimize(ref _this, owner, message, opts, stats);
        }

        internal override System.Linq.Expressions.Expression TryCompile(bool selfCompile, bool forAssign, Type expectedType, List<CodeNode> dynamicValues)
        {
            return _tree;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return Original.ToString();
        }
    }
}
