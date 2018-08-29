﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JS.Core.Core.JIT;
using JS.Core.Expressions;
using NiL.JS;
using Expression = JS.Core.Expressions.Expression;

namespace JS.Core.Core
{
#if !NET35
#if !PORTABLE 
    [Serializable]
#endif
    public sealed class CompiledNode : Expressions.Expression
    {
        private static readonly MethodInfo wrapMethod = typeof(JITHelpers).GetMethod("wrap", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly ParameterExpression wrapContainerParameter = System.Linq.Expressions.Expression.Parameter(typeof(JSValue), "wrapContainer");

        private static readonly ParameterExpression[] lambdaArgs = new[]
                {
                    JITHelpers.ContextParameter,
                    JITHelpers.DynamicValuesParameter,
                    wrapContainerParameter
                };

        private readonly CodeNode[] _dynamicValues;
        private Func<Context, CodeNode[], JSValue, JSValue> _compiledTree;
        private System.Linq.Expressions.Expression _tree;

        public CodeNode Original { get; }

        protected internal override bool ContextIndependent => Original is Expression && (Original as Expression).ContextIndependent;

        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType
        {
            get
            {
                if (!(Original is Expression))
                    return PredictedType.Unknown;
                return (Original as Expression).ResultType;
            }
        }

        public override int Length
        {
            get => Original.Length;
            internal set
            {
                Original.Length = value;
            }
        }

        public override int Position
        {
            get => Original.Position;
            internal set
            {
                Original.Position = value;
            }
        }

        public CompiledNode(CodeNode original, System.Linq.Expressions.Expression tree, CodeNode[] dynamicValues)
            : base(original is Expression ? (original as Expression)._left : null, original is Expression ? (original as Expression)._right : null, (original is Expression) && (original as Expression)._tempContainer == null)
        {
            if (_tempContainer == null)
                _tempContainer = (original as Expression)._tempContainer;
            this.Original = original;
            this._tree = tree;
            this._dynamicValues = dynamicValues;
        }

        public CompiledNode(Expression original, System.Linq.Expressions.Expression tree, CodeNode[] dynamicValues)
            : base(original._left, original._right, original._tempContainer == null)
        {
            if (_tempContainer == null)
                _tempContainer = original._tempContainer;
            this.Original = original;
            this._tree = tree;
            this._dynamicValues = dynamicValues;
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
                this._tree = this._tree.Reduce();
                if (Original is Expression)
                {
                    if (typeof(JSValue).IsAssignableFrom(this._tree.Type))
                        tree = this._tree;
                    else
                        tree = System.Linq.Expressions.Expression.Call(wrapMethod.MakeGenericMethod(this._tree.Type), this._tree, wrapContainerParameter);
                }
                else
                {
                    tree = System.Linq.Expressions.Expression.Block(this._tree, JITHelpers.UndefinedConstant);
                }
                //var ps = new PermissionSet(System.Security.Permissions.PermissionState.Unrestricted);
                //ps.Assert();
                //ps.AddPermission(new System.Security.Permissions.ZoneIdentityPermission(SecurityZone.MyComputer));
                //var assm = AppDomain.CurrentDomain.DefineDynamicAssembly(
                //    new AssemblyName("DynamicAssm" + Environment.TickCount),
                //    AssemblyBuilderAccess.RunAndCollect,
                //    ps,
                //    null,
                //    null);
                //var module = assm.DefineDynamicModule("DynamicModule");
                //var type = module.DefineType("DynamicType", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract);
                //var method = type.DefineMethod(
                //    "DynamicMethod",
                //    MethodAttributes.Public | MethodAttributes.Static,
                //    typeof(JSObject),
                //    new[] { typeof(Context), typeof(CodeNode[]), typeof(JSObject) });

                //Expression.Lambda(tree, lambdaArgs).CompileToMethod(method);
                //compiledTree = (Func<Context, CodeNode[], JSObject, JSObject>)type.CreateType().GetMethods()[0].CreateDelegate(typeof(Func<Context, CodeNode[], JSObject, JSObject>));

                _compiledTree = System.Linq.Expressions.Expression.Lambda<Func<Context, CodeNode[], JSValue, JSValue>>(tree, lambdaArgs).Compile();
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
#endif
}
