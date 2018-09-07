using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Functions;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Expressions;
using Maddalena.Core.Javascript.Statements;
using Expression = Maddalena.Core.Javascript.Expressions.Expression;
using linqEx = System.Linq.Expressions;
using PropertyPair = Maddalena.Core.Javascript.Core.PropertyPair;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    /// <summary>
    /// Возможные типы функции в контексте использования.
    /// </summary>
    [Serializable]
    public enum FunctionKind
    {
        Function = 0,
        Getter,
        Setter,
        AnonymousFunction,
        AnonymousGenerator,
        Generator,
        Method,
        MethodGenerator,
        Arrow,
        AsyncFunction,
        AsyncAnonymousFunction,
        AsyncArrow
    }

    [Serializable]
    public enum RequireNewKeywordLevel
    {
        Both = 0,
        WithNewOnly,
        WithoutNewOnly
    }

    [Serializable]
    public class Function : JSObject, ICallable
    {
        private static readonly FunctionDefinition creatorDummy = new FunctionDefinition();
        internal static readonly Function Empty = new Function();
        private static readonly Function TTEProxy = new MethodProxy(new Context(null, false, Empty), typeof(Function)
            .GetMethod("ThrowTypeError", BindingFlags.Static | BindingFlags.NonPublic))
        {
            _attributes = JSValueAttributesInternal.DoNotDelete
                | JSValueAttributesInternal.Immutable
                | JSValueAttributesInternal.DoNotEnumerate
                | JSValueAttributesInternal.ReadOnly
        };
        protected static void ThrowTypeError()
        {
            ExceptionHelper.Throw(new TypeError("Properties \"caller\", \"callee\" and \"arguments\" may not be accessed in strict mode."));
        }
        internal static readonly JSValue propertiesDummySM = new JSValue
        {
            _valueType = JSValueType.Property,
            _oValue = new PropertyPair { getter = TTEProxy, setter = TTEProxy },
            _attributes = JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.Immutable | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.NonConfigurable
        };

        private Dictionary<Type, Delegate> _delegateCache;

        internal readonly FunctionDefinition _functionDefinition;
        [Hidden]
        internal readonly Context _initialContext;
        [Hidden]
        public Context Context
        {
            [Hidden]
            get
            {
                return _initialContext;
            }
        }
        [Field]
        [DoNotDelete]
        [DoNotEnumerate]
        public virtual string name
        {
            [Hidden]
            get
            {
                return _functionDefinition.Name;
            }
        }

        [Hidden]
        internal Number _length;
        [Field]
        [global::Maddalena.Core.Javascript.Core.Interop.ReadOnlyAttribute]
        [DoNotDelete]
        [DoNotEnumerate]
        [NotConfigurable]
        public virtual JSValue length
        {
            [Hidden]
            get
            {
                if (_length != null) return _length;
                _length = new Number(0)
                {
                    _attributes =
                        JSValueAttributesInternal.ReadOnly
                        | JSValueAttributesInternal.DoNotDelete
                        | JSValueAttributesInternal.DoNotEnumerate
                        | JSValueAttributesInternal.NonConfigurable,
                    _iValue = _functionDefinition.Parameters.Length
                };

                return _length;
            }
        }
        [Hidden]
        public virtual bool Strict
        {
            [Hidden]
            get
            {
                return _functionDefinition?.Body._strict ?? true;
            }
        }
        [Hidden]
        public virtual CodeBlock Body
        {
            [Hidden]
            get
            {
                return _functionDefinition?.Body;
            }
        }

        [Hidden]
        public virtual FunctionKind Kind
        {
            [Hidden]
            get
            {
                return _functionDefinition.Kind;
            }
        }

        [Hidden]
        public virtual RequireNewKeywordLevel RequireNewKeywordLevel
        {
            [Hidden]
            get;
            [Hidden]
            protected internal set;
        }

        #region Runtime
        [Hidden]
        internal JSValue _prototype;
        [Field]
        [DoNotDelete]
        [DoNotEnumerate]
        [NotConfigurable]
        public virtual JSValue prototype
        {
            [Hidden]
            get
            {
                if (_prototype == null)
                {
                    if ((_attributes & JSValueAttributesInternal.ProxyPrototype) != 0)
                    {
                        // Вызывается в случае Function.prototype.prototype
                        // выдавать тут константу undefined нельзя, иначе будет падать на вызове defineProperty
                        // присваивание нужно для простановки атрибутов
                        prototype = new JSObject();
                        _prototype._attributes = JSValueAttributesInternal.None;
                    }
                    else
                    {
                        var res = CreateObject(true);
                        res._attributes = JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.NonConfigurable;
                        (res._fields["constructor"] = CloneImpl(false))._attributes = JSValueAttributesInternal.DoNotEnumerate;
                        _prototype = res;
                    }
                }

                return _prototype;
            }

            [Hidden]
            set
            {
                if (value == _prototype)
                    return;

                var oldValue = _prototype;

                if (value == null)
                    _prototype = @null;
                else
                    _prototype = value._valueType >= JSValueType.Object ? value._oValue as JSObject ?? value : value;

                _prototype = _prototype.CloneImpl(true);
                if (oldValue == null)
                {
                    _prototype._attributes =
                        JSValueAttributesInternal.Field
                        | JSValueAttributesInternal.DoNotDelete
                        | JSValueAttributesInternal.DoNotEnumerate
                        | JSValueAttributesInternal.NonConfigurable;
                }
                else
                {
                    _prototype._attributes = oldValue._attributes;
                }
            }
        }
        /// <summary>
        /// Объект, содержащий параметры вызова функции либо null если в данный момент функция не выполняется.
        /// </summary>
        [Field]
        [DoNotDelete]
        [DoNotEnumerate]
        public virtual JSValue arguments
        {
            [Hidden]
            get
            {
                var context = Context.GetRunningContextFor(this);
                if (context == null)
                    return null;

                if (_functionDefinition.Body._strict)
                    ExceptionHelper.Throw(new TypeError("Property \"arguments\" may not be accessed in strict mode."));

                if (context._arguments == null && _functionDefinition.recursionDepth > 0)
                    BuildArgumentsObject();

                return context._arguments;
            }
            [Hidden]
            set
            {
                var context = Context.GetRunningContextFor(this);
                if (context == null)
                    return;

                if (_functionDefinition.Body._strict)
                    ExceptionHelper.Throw(new TypeError("Property \"arguments\" may not be accessed in strict mode."));

                context._arguments = value;
            }
        }

        [Field]
        [DoNotDelete]
        [DoNotEnumerate]
        public virtual JSValue caller
        {
            [Hidden]
            get
            {
                Context oldContext;
                var context = Context.GetRunningContextFor(this, out oldContext);
                if (context == null || oldContext == null)
                    return null;

                if (context._strict || (oldContext._strict && oldContext._owner != null))
                    ExceptionHelper.Throw(new TypeError("Property \"caller\" may not be accessed in strict mode."));

                return oldContext._owner;
            }
            [Hidden]
            set
            {
                Context oldContext;
                var context = Context.GetRunningContextFor(this, out oldContext);
                if (context == null || oldContext == null)
                    return;

                if (context._strict || (oldContext._strict && oldContext._owner != null))
                    ExceptionHelper.Throw(new TypeError("Property \"caller\" may not be accessed in strict mode."));
            }
        }
        #endregion

        [DoNotEnumerate]
        public Function()
        {
            _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject;
            _functionDefinition = creatorDummy;
            _valueType = JSValueType.Function;
            _oValue = this;
            RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
        }

        [Hidden]
        public Function(Context context)
            : this()
        {
            _initialContext = context ?? throw new ArgumentNullException(nameof(context));
            RequireNewKeywordLevel = RequireNewKeywordLevel.Both;
        }

        [DoNotEnumerate]
        public Function(Arguments args)
        {
            _initialContext = (Context.CurrentContext ?? Context.DefaultGlobalContext).RootContext;
            if (_initialContext == Context._DefaultGlobalContext || _initialContext == null)
                throw new InvalidOperationException("Special Functions constructor can be called from javascript code only");

            _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject;

            var index = 0;
            int len = args.Length - 1;
            var argn = "";
            for (int i = 0; i < len; i++)
                argn += args[i] + (i + 1 < len ? "," : "");
            string code = "function (" + argn + "){" + Environment.NewLine + (len == -1 ? "undefined" : args[len]) + Environment.NewLine + "}";
            var func = FunctionDefinition.Parse(
                new ParseInfo(Parser.RemoveComments(code, 0), code, null)
                {
                    CodeContext = CodeContext.InExpression
                },
                ref index,
                FunctionKind.Function);

            if (func != null && code.Length == index)
            {
                Parser.Build(ref func, 0, new Dictionary<string, VariableDescriptor>(), _initialContext._strict ? CodeContext.Strict : CodeContext.None, null, null, Options.None);

                func.RebuildScope(null, null, 0);
                func.Optimize(ref func, null, null, Options.None, null);
                func.Decompose(ref func);

                _functionDefinition = func as FunctionDefinition;
            }
            else
                ExceptionHelper.Throw(new SyntaxError("Unknown syntax error"));

            _valueType = JSValueType.Function;
            _oValue = this;
        }

        [Hidden]
        internal Function(Context context, FunctionDefinition functionDefinition)
        {
            _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject;
            _initialContext = context;
            _functionDefinition = functionDefinition;
            _valueType = JSValueType.Function;
            _oValue = this;
        }

        [Hidden]
        public virtual JSValue Construct(Arguments arguments)
        {
            if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
            {
                ExceptionHelper.ThrowTypeError(string.Format(Messages.InvalidTryToCallWithNew, name));
            }

            JSValue targetObject = ConstructObject();
            targetObject._attributes |= JSValueAttributesInternal.ConstructingObject;
            JSValue result;
            try
            {
                result = Construct(targetObject, arguments);
            }
            finally
            {
                targetObject._attributes &= ~JSValueAttributesInternal.ConstructingObject;
            }
            return result;
        }

        [Hidden]
        public virtual JSValue Construct(JSValue targetObject, Arguments arguments)
        {
            if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
            {
                ExceptionHelper.ThrowTypeError(string.Format(Messages.InvalidTryToCallWithNew, name));
            }

            var res = Invoke(true, targetObject, arguments);
            if (res._valueType < JSValueType.Object || res._oValue == null)
                return targetObject;

            return res;
        }

        protected internal virtual JSValue ConstructObject()
        {
            JSValue targetObject = new JSObject { _valueType = JSValueType.Object };
            targetObject._oValue = targetObject;
            targetObject.__proto__ = prototype._valueType < JSValueType.Object ? Context.GlobalContext._globalPrototype : prototype._oValue as JSObject;

            return targetObject;
        }

        internal virtual JSValue InternalInvoke(JSValue targetObject, Expression[] arguments, Context initiator, bool withSpread, bool construct)
        {
            if (_functionDefinition.Body == null)
                return NotExists;

            Arguments argumentsObject = Tools.CreateArguments(arguments, initiator);

            initiator._objectSource = null;

            if (construct)
            {
                if (targetObject == null || targetObject._valueType < JSValueType.Object)
                    return Construct(argumentsObject);
                return Construct(targetObject, argumentsObject);
            }

            return Call(targetObject, argumentsObject);
        }

        [Hidden]
        [DebuggerStepThrough]
        public JSValue Call(Arguments args) => Call(undefined, args);

        [Hidden]
        public JSValue Call(JSValue targetObject, Arguments arguments)
        {
            if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithNewOnly)
            {
                ExceptionHelper.ThrowTypeError(string.Format(Messages.InvalidTryToCreateWithoutNew, name));
            }

            targetObject = correctTargetObject(targetObject, _functionDefinition.Body._strict);
            return Invoke(false, targetObject, arguments);
        }

        protected internal virtual JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            JSValue result;
            var body = _functionDefinition.Body;
            if (body._lines.Length == 0)
            {
                notExists._valueType = JSValueType.NotExists;
                return notExists;
            }

            var ceocw = _functionDefinition.FunctionInfo.ContainsEval || _functionDefinition.FunctionInfo.ContainsWith || _functionDefinition.FunctionInfo.NeedDecompose;
            if (_functionDefinition.recursionDepth > _functionDefinition.parametersStored) // рекурсивный вызов.
            {
                if (!ceocw)
                    storeParameters();
                _functionDefinition.parametersStored = _functionDefinition.recursionDepth;
            }

            if (arguments == null)
                arguments = new Arguments(Context.CurrentContext);

            for (;;) // tail recursion catcher
            {
                var internalContext = new Context(_initialContext, ceocw, this) {_definedVariables = body._variables};
                internalContext.Activate();

                try
                {
                    initContext(targetObject, arguments, ceocw, internalContext);
                    initParameters(arguments, internalContext);
                    _functionDefinition.recursionDepth++;
                    result = evaluateBody(internalContext);
                }
                finally
                {
                    _functionDefinition.recursionDepth--;
                    if (_functionDefinition.parametersStored > _functionDefinition.recursionDepth)
                        _functionDefinition.parametersStored = _functionDefinition.recursionDepth;

                    exit(internalContext);
                }

                if (result != null) // tail recursion
                    break;

                arguments = internalContext._executionInfo as Arguments;
                targetObject = correctTargetObject(internalContext._objectSource, body._strict);
            }

            return result;
        }

        internal JSValue evaluateBody(Context internalContext)
        {
            _functionDefinition.Body.Evaluate(internalContext);
            if (internalContext._executionMode == ExecutionMode.TailRecursion)
                return null;

            var ai = internalContext._executionInfo;
            if (ai == null || ai._valueType < JSValueType.Undefined)
            {
                notExists._valueType = JSValueType.NotExists;
                return notExists;
            }

            if (ai._valueType == JSValueType.Undefined)
            {
                return undefined;
            }

            return (ai._attributes & JSValueAttributesInternal.SystemObject) == 0 ? ai.CloneImpl(false) : ai;
        }

        internal void exit(Context internalContext)
        {
            _functionDefinition?.Body?.clearVariablesCache();
            internalContext._executionMode = ExecutionMode.Return;
            internalContext.Deactivate();
        }

        internal void BuildArgumentsObject()
        {
            var context = Context.GetRunningContextFor(this, out var oldContext);
            if (context != null && context._arguments == null)
            {
                var args = new Arguments
                {
                    caller = oldContext?._owner,
                    callee = this,
                    length = _functionDefinition.Parameters.Length
                };

                for (var i = 0; i < _functionDefinition.Parameters.Length; i++)
                {
                    if (_functionDefinition.Body._strict)
                        args[i] = _functionDefinition.Parameters[i].cacheRes.CloneImpl(false);
                    else
                        args[i] = _functionDefinition.Parameters[i].cacheRes;
                }

                context._arguments = args;
            }
        }

        internal void initContext(JSValue targetObject, Arguments arguments, bool storeArguments, Context internalContext)
        {
            if (_functionDefinition.Reference._descriptor != null && _functionDefinition.Reference._descriptor.cacheRes == null)
            {
                _functionDefinition.Reference._descriptor.cacheContext = internalContext._parent;
                _functionDefinition.Reference._descriptor.cacheRes = this;
            }

            internalContext._thisBind = targetObject;
            internalContext._strict |= _functionDefinition.Body._strict;
            if (_functionDefinition.Kind == FunctionKind.Arrow)
            {
                internalContext._arguments = internalContext._parent._arguments;
                internalContext._thisBind = internalContext._parent._thisBind;
            }
            else
            {
                internalContext._arguments = arguments;

                if (storeArguments)
                    internalContext._variables["arguments"] = arguments;

                if (_functionDefinition.Body._strict)
                {
                    arguments._attributes |= JSValueAttributesInternal.ReadOnly;
                    arguments.callee = propertiesDummySM;
                    arguments.caller = propertiesDummySM;
                }
                else
                {
                    arguments.callee = this;
                }
            }
        }

        internal void initParameters(Arguments args, Context internalContext)
        {
            var ceaw = _functionDefinition.FunctionInfo.ContainsEval || _functionDefinition.FunctionInfo.ContainsArguments || _functionDefinition.FunctionInfo.ContainsWith;
            int min = System.Math.Min(args.length, _functionDefinition.Parameters.Length - (_functionDefinition.FunctionInfo.ContainsRestParameters ? 1 : 0));

            JSValue[] defaultValues = null;
            Array restArray = null;
            if (_functionDefinition.FunctionInfo.ContainsRestParameters)
            {
                restArray = new Array();
            }

            for (var i = 0; i < _functionDefinition.Parameters.Length; i++)
            {
                JSValue t = args[i];
                var prm = _functionDefinition.Parameters[i];
                if (!t.Defined)
                {
                    if (prm.initializer != null)
                    {
                        if (defaultValues == null)
                            defaultValues = new JSValue[_functionDefinition.Parameters.Length];
                        defaultValues[i] = prm.initializer.Evaluate(internalContext);
                    }
                }
            }

            for (var i = 0; i < min; i++)
            {
                JSValue t = args[i];
                var prm = _functionDefinition.Parameters[i];
                if (!t.Defined)
                {
                    if (prm.initializer != null)
                        t = defaultValues?[i] ?? undefined;
                    else
                        t = undefined;
                }

                if (_functionDefinition.Body._strict)
                {
                    if (ceaw)
                    {
                        args[i] = t.CloneImpl(false);
                        t = t.CloneImpl(false);
                    }
                    else if (prm.assignments != null)
                    {
                        t = t.CloneImpl(false);
                        args[i] = t;
                    }
                }
                else
                {
                    if (prm.assignments != null
                        || ceaw
                        || (t._attributes & JSValueAttributesInternal.Temporary) != 0)
                    {
                        t = t.CloneImpl(false);
                        args[i] = t;
                        t._attributes |= JSValueAttributesInternal.Argument;
                    }
                }

                t._attributes &= ~JSValueAttributesInternal.Cloned;
                if (prm.captured || ceaw)
                    (internalContext._variables ?? (internalContext._variables = getFieldsContainer()))[prm.Name] = t;
                prm.cacheContext = internalContext;
                prm.cacheRes = t;
                if (string.CompareOrdinal(prm.name, "arguments") == 0)
                    internalContext._arguments = t;
            }

            for (var i = min; i < args.length; i++)
            {
                var t = args[i];
                if (ceaw)
                    args[i] = t = t.CloneImpl(false);
                t._attributes |= JSValueAttributesInternal.Argument;

                restArray?._data.Add(t);
            }

            for (var i = min; i < _functionDefinition.Parameters.Length; i++)
            {
                var parameter = _functionDefinition.Parameters[i];
                if (parameter.initializer != null)
                {
                    if (ceaw || parameter.assignments != null)
                    {
                        parameter.cacheRes = (defaultValues?[i] ?? undefined).CloneImpl(false);
                    }
                    else
                    {
                        parameter.cacheRes = defaultValues?[i] ?? undefined;
                        if (!parameter.cacheRes.Defined)
                            parameter.cacheRes = undefined;
                    }
                }
                else
                {
                    if (ceaw || parameter.assignments != null)
                    {
                        if (i == min && restArray != null)
                            parameter.cacheRes = restArray.CloneImpl(false);
                        else
                            parameter.cacheRes = new JSValue { _valueType = JSValueType.Undefined };
                        parameter.cacheRes._attributes = JSValueAttributesInternal.Argument;
                    }
                    else
                    {
                        if (i == min && restArray != null)
                            parameter.cacheRes = restArray;
                        else
                            parameter.cacheRes = undefined;
                    }
                }

                parameter.cacheContext = internalContext;
                if (parameter.Destructor == null && (parameter.captured || ceaw))
                {
                    if (internalContext._variables == null)
                        internalContext._variables = getFieldsContainer();
                    internalContext._variables[parameter.Name] = parameter.cacheRes;
                }

                if (string.CompareOrdinal(parameter.name, "arguments") == 0)
                    internalContext._arguments = parameter.cacheRes;
            }
        }

        internal JSValue correctTargetObject(JSValue thisBind, bool strict)
        {
            if (thisBind == null)
            {
                return strict ? undefined : _initialContext?.RootContext._thisBind;
            }

            if (_initialContext != null)
            {
                if (!strict) // Поправляем this
                {
                    if (thisBind._valueType > JSValueType.Undefined && thisBind._valueType < JSValueType.Object)
                        return thisBind.ToObject();
                    if (thisBind._valueType <= JSValueType.Undefined || thisBind._oValue == null)
                        return _initialContext.RootContext._thisBind;
                }
                else if (thisBind._valueType <= JSValueType.Undefined)
                    return undefined;
            }

            return thisBind;
        }

        internal void storeParameters()
        {
            if (_functionDefinition.Parameters.Length == 0) return;
            var context = _functionDefinition.Parameters[0].cacheContext;

            if (context._variables == null)
                context._variables = getFieldsContainer();

            foreach (var parameter in _functionDefinition.Parameters)
                context._variables[parameter.Name] = parameter.cacheRes;
        }

        [Hidden]
        protected internal override JSValue GetProperty(JSValue nameObj, bool forWrite, PropertyScope memberScope)
        {
            if (memberScope < PropertyScope.Super && nameObj._valueType != JSValueType.Symbol)
            {
                string name = nameObj.ToString();

                if (_functionDefinition.Body._strict && (name == "caller" || name == "arguments"))
                    return propertiesDummySM;

                if ((!forWrite || (_attributes & JSValueAttributesInternal.ProxyPrototype) != 0) && name == "prototype")
                {
                    return prototype;
                }

                if (nameObj._valueType != JSValueType.String)
                    nameObj = name;
            }

            return base.GetProperty(nameObj, forWrite, memberScope);
        }

       
        [DoNotEnumerate]
        [ArgumentsCount(0)]
        public new JSValue toString(Arguments args)
        {
            return ToString();
        }

        [Hidden]
        public sealed override string ToString()
        {
            return ToString(false);
        }

        [Hidden]
        public virtual string ToString(bool headerOnly)
        {
            return _functionDefinition.ToString(headerOnly);
        }

        [Hidden]
        public override JSValue valueOf()
        {
            return base.valueOf();
        }

        [DoNotEnumerate]
       
        [EditorBrowsable(EditorBrowsableState.Never)]
        public JSValue call(Arguments args)
        {
            var newThis = args[0];
            var prmlen = args.length - 1;
            if (prmlen >= 0)
            {
                for (var i = 0; i <= prmlen; i++)
                    args[i] = args[i + 1];
                args[prmlen] = null;
                args.length = prmlen;
            }
            else
                args[0] = null;
            return Call(newThis, args);
        }

        [DoNotEnumerate]
        [ArgumentsCount(2)]
        [AllowNullArguments]
        public JSValue apply(Arguments args)
        {
            var nargs = args ?? new Arguments();
            var argsSource = nargs[1];
            var self = nargs[0];
            if (args != null)
                nargs.Reset();
            if (argsSource.Defined)
            {
                if (argsSource._valueType < JSValueType.Object)
                    ExceptionHelper.Throw(new TypeError("Argument list has wrong type."));
                var len = argsSource["length"];
                if (len._valueType == JSValueType.Property)
                    len = (len._oValue as PropertyPair).getter.Call(argsSource, null);
                nargs.length = Tools.JSObjectToInt32(len);
                if (nargs.length >= 50000)
                    ExceptionHelper.Throw(new RangeError("Too many arguments."));
                for (var i = nargs.length; i-- > 0;)
                    nargs[i] = argsSource[Tools.Int32ToString(i)];
            }
            return Call(self, nargs);
        }

        [DoNotEnumerate]
        public virtual Function bind(Arguments args)
        {
            if (args.Length == 0)
                return this;

            var newThis = args[0];
            var strict = (_functionDefinition.Body != null && _functionDefinition.Body._strict) || Context.CurrentContext._strict;
            return new BindedFunction(this, args);
        }

        [Hidden]
        public T MakeDelegate<T>()
        {
            return (T)(object)MakeDelegate(typeof(T));
        }

        [Hidden]
        public virtual Delegate MakeDelegate(Type delegateType)
        {
            if (_delegateCache != null)
            {
                if (_delegateCache.TryGetValue(delegateType, out var cachedDelegate))
                    return cachedDelegate;
            }

            MethodInfo invokeMethod = delegateType.GetMethod("Invoke");
            var @delegate = Tools.BuildJsCallTree("<delegate>" + name, linqEx.Expression.Constant(this), null, invokeMethod, delegateType).Compile();

            if (_delegateCache == null)
                _delegateCache = new Dictionary<Type, Delegate>();
            _delegateCache.Add(delegateType, @delegate);

            return @delegate;
        }
    }
}
