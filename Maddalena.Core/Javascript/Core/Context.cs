using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Functions;
using Maddalena.Core.Javascript.Statements;
using Debugger = System.Diagnostics.Debugger;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public enum ExecutionMode
    {
        None = 0,
        Continue,
        Break,
        Return,
        TailRecursion,
        Exception,
        Suspend,
        Resume,
        ResumeThrow
    }

    /// <summary>
    /// Контекст выполнения скрипта. Хранит состояние выполнения сценария.
    /// </summary>
    [Serializable]
    public class Context : IEnumerable<string>
    {
        internal const int MaxConcurentContexts = 65535;
        internal static readonly List<Context>[] RunningContexts = new List<Context>[MaxConcurentContexts];
        private static readonly int[] _ThreadIds = new int[MaxConcurentContexts];

        internal static List<Context> GetCurrectContextStack()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            for (var i = 0; i < MaxConcurentContexts; i++)
            {
                if (_ThreadIds[i] == 0)
                    break;

                if (_ThreadIds[i] == threadId)
                    return RunningContexts[i];
            }

            return null;
        }

        public static Context CurrentContext
        {
            get
            {
                var stack = GetCurrectContextStack();
                if (stack == null || stack.Count == 0)
                    return null;

                return stack[stack.Count - 1];
            }
        }

        internal static readonly GlobalContext _DefaultGlobalContext = new GlobalContext { _strict = true };
        public static GlobalContext DefaultGlobalContext => _DefaultGlobalContext;

        internal ExecutionMode _executionMode;
        internal JSValue _objectSource;
        internal JSValue _executionInfo;
        internal JSValue _lastResult;
        internal JSValue _arguments;
        internal JSValue _thisBind;
        internal Function _owner;
        internal Context _parent;
        internal IDictionary<string, JSValue> _variables;
        internal bool _strict;
        internal VariableDescriptor[] _definedVariables;
        internal Module _module;
        private Dictionary<CodeNode, object> _suspendData;

        public Context RootContext
        {
            get
            {
                var res = this;

                while (res._parent?._parent != null)
                    res = res._parent;

                return res;
            }
        }

        public GlobalContext GlobalContext
        {
            get
            {
                var iter = this;
                if (iter._parent != null)
                {
                    do
                        iter = iter._parent;
                    while (iter._parent != null);
                }

                if (!(iter is GlobalContext result))
                    throw new Exception("Incorrect state");

                return result;
            }
        }

        public static GlobalContext CurrentGlobalContext => (CurrentContext ?? _DefaultGlobalContext).GlobalContext;

        public JSValue ThisBind
        {
            get
            {
                if (_parent == null)
                    ExceptionHelper.Throw(new InvalidOperationException("Unable to get this-binding for Global Context"));

                var c = this;
                if (_thisBind != null) return _thisBind;

                if (_strict)
                    return JSValue.undefined;

                for (; c._thisBind == null;)
                {
                    if (c._parent._parent == null)
                    {
                        _thisBind = new GlobalObject(c);
                        c._thisBind = _thisBind;
                        break;
                    }

                    c = c._parent;
                }

                _thisBind = c._thisBind;

                return _thisBind;
            }
        }

        public bool Debugging { get; set; }

        public event DebuggerCallback DebuggerCallback;

        public bool Running => GetCurrectContextStack().Contains(this);

        public ExecutionMode AbortReason => _executionMode;

        public JSValue AbortInfo => _executionInfo;

        public Dictionary<CodeNode, object> SuspendData
        {
            get => _suspendData ?? (_suspendData = new Dictionary<CodeNode, object>());
            internal set => _suspendData = value;
        }

        static Context()
        {
        }

        public Context()
            : this(CurrentGlobalContext, true, Function.Empty)
        {
        }

        public Context(Context prototype)
            : this(prototype, true, Function.Empty)
        {
        }

        public Context(Context prototype, bool strict)
            : this(prototype, true, Function.Empty)
        {
            _strict = strict;
        }


        public Context(bool strict)
            : this(CurrentGlobalContext, strict)
        {
        }

        internal Context(Context prototype, bool createFields, Function owner)
        {
            _owner = owner;
            if (prototype != null)
            {
                if (owner == prototype._owner)
                    _arguments = prototype._arguments;

                _definedVariables = _owner?.Body?._variables;
                _parent = prototype;
                _thisBind = prototype._thisBind;
                Debugging = prototype.Debugging;
            }

            if (createFields)
                _variables = JSObject.getFieldsContainer();

            _executionInfo = JSValue.notExists;
        }

        public static void ResetGlobalContext()
        {
            _DefaultGlobalContext.ResetContext();
        }

        internal bool Activate()
        {
#if (PORTABLE)
            if (currentContextStack == null)
                currentContextStack = new List<Context>();

            if (currentContextStack.Count > 0 && currentContextStack[currentContextStack.Count - 1] == this)
                return false;

            currentContextStack.Add(this);
            return true;
#else
            int threadId = Thread.CurrentThread.ManagedThreadId;
            var firstEmptyIndex = -1;
            var i = 0;
            bool entered = false;
            do
            {
                if (_ThreadIds[i] == threadId)
                {
                    if (RunningContexts[i].Count > 0 && RunningContexts[i][RunningContexts[i].Count - 1] == this)
                    {
                        if (entered)
                            Monitor.Exit(RunningContexts);

                        return false;
                    }

                    // Бьёт по производительности
                    //if (RunnedContexts[i].Contains(this))
                    //    ExceptionsHelper.Throw(new ApplicationException("Try to reactivate context"));

                    RunningContexts[i].Add(this);

                    if (entered)
                        Monitor.Exit(RunningContexts);

                    return true;
                }

                if (!entered)
                {
                    Monitor.Enter(RunningContexts);
                    entered = true;
                }

                if (_ThreadIds[i] == 0)
                {
                    if (firstEmptyIndex == -1)
                        firstEmptyIndex = i;

                    break;
                }

                if (_ThreadIds[i] == -1 && firstEmptyIndex == -1)
                {
                    firstEmptyIndex = i;
                }

                i++;
            }
            while (i < MaxConcurentContexts);

            if (firstEmptyIndex != -1)
            {
                if (RunningContexts[firstEmptyIndex] == null)
                    RunningContexts[firstEmptyIndex] = new List<Context>();

                _ThreadIds[firstEmptyIndex] = threadId;
                RunningContexts[firstEmptyIndex].Add(this);

                Monitor.Exit(RunningContexts);
                return true;
            }

            Monitor.Exit(RunningContexts);

            ExceptionHelper.Throw(new InvalidOperationException("Too many concurrent contexts."));

            return false;
#endif
        }

        internal Context Deactivate()
        {
#if (PORTABLE)
            if (currentContextStack[currentContextStack.Count - 1] != this)
                throw new InvalidOperationException("Context is not running");

            currentContextStack.RemoveAt(currentContextStack.Count - 1);
            return CurrentContext;
#else
            int threadId = Thread.CurrentThread.ManagedThreadId;

            var i = 0;
            for (; i < MaxConcurentContexts; i++)
            {
                if (_ThreadIds[i] == 0)
                    throw new InvalidOperationException("Context is not running");

                if (_ThreadIds[i] == threadId)
                {
                    if (RunningContexts[i][RunningContexts[i].Count - 1] != this)
                        throw new InvalidOperationException("Context is not running");

                    _module = null;
                    RunningContexts[i].RemoveAt(RunningContexts[i].Count - 1);
                    if (RunningContexts[i].Count == 0)
                        _ThreadIds[i] = -1;

                    break;
                }
            }

            return RunningContexts[i].Count > 0 ? RunningContexts[i][RunningContexts[i].Count - 1] : null;
#endif
        }

        internal Context GetRunningContextFor(Function function)
        {
            Context context = null;
            return GetRunningContextFor(function, out context);
        }

        internal Context GetRunningContextFor(Function function, out Context prevContext)
        {
            prevContext = null;

            if (function == null)
                return null;

            var stack = GetCurrectContextStack();

            for (var i = stack.Count; i-- > 0;)
            {
                if (stack[i]._owner == function)
                {
                    if (i > 0)
                        prevContext = stack[i - 1];
                    return stack[i];
                }
            }

            return null;
        }

        internal virtual void ReplaceVariableInstance(string name, JSValue instance)
        {
            if (_variables != null && _variables.ContainsKey(name))
                _variables[name] = instance;
            else
                _parent?.ReplaceVariableInstance(name, instance);
        }

        public virtual JSValue DefineVariable(string name, bool deletable = false)
        {
            JSValue res = null;
            if (_variables == null || !_variables.TryGetValue(name, out res))
            {
                if (_variables == null)
                    _variables = JSObject.getFieldsContainer();

                res = new JSValue();
                _variables[name] = res;

                if (!deletable)
                    res._attributes = JSValueAttributesInternal.DoNotDelete;
            }
            else if (res.NeedClone)
            {
                res = res.CloneImpl(false);
                _variables[name] = res;
            }

            res._valueType |= JSValueType.Undefined;

            return res;
        }

        /// <summary>
        /// Creates new property with Getter and Setter in the object
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="getter">Function called when there is an attempt to get a value. Can be null</param>
        /// <param name="setter">Function called when there is an attempt to set a value. Can be null</param>
        /// <exception cref="System.ArgumentException">if property already exists</exception>
        /// <exception cref="System.InvalidOperationException">if unable to create property</exception>
        public void DefineGetSetVariable(string name, Func<object> getter, Action<object> setter)
        {
            var property = GetVariable(name);
            if (property.ValueType >= JSValueType.Undefined)
                throw new ArgumentException();

            property = DefineVariable(name);
            if (property.ValueType < JSValueType.Undefined)
                throw new InvalidOperationException();

            property._valueType = JSValueType.Property;

            Function jsGetter = null;
            if (getter != null)
            {
                jsGetter = new MethodProxy(this, getter.GetMethodInfo(), getter.Target);
            }

            Function jsSetter = null;
            if (setter != null)
            {
                jsSetter = new MethodProxy(this, setter.GetMethodInfo(), setter.Target);
            }

            property._oValue = new PropertyPair(jsGetter, jsSetter);
        }

        public JSValue GetVariable(string name)
        {
            return GetVariable(name, false);
        }

        protected internal virtual JSValue GetVariable(string name, bool forWrite)
        {
            JSValue res = null;

            bool fromProto = _variables == null || (!_variables.TryGetValue(name, out res) && (_parent != null));
            if (fromProto)
                res = _parent.GetVariable(name, forWrite);

            if (res == null) // значит вышли из глобального контекста
            {
                if (_parent == null)
                {
                    return null;
                }

                if (forWrite)
                {
                    res = new JSValue { _valueType = JSValueType.NotExists };
                    _variables[name] = res;
                }
                else
                {
                    res = GlobalContext._globalPrototype.GetProperty(name, false, PropertyScope.Common);
                    if (res._valueType == JSValueType.NotExistsInObject)
                        res._valueType = JSValueType.NotExists;
                }
            }
            else if (fromProto)
            {
                _objectSource = _parent._objectSource;
            }
            else
            {
                if (!forWrite || !res.NeedClone) return res;

                res = res.CloneImpl(false);
                _variables[name] = res;
            }

            return res;
        }

        internal void raiseDebugger(CodeNode nextStatement)
        {
            var p = this;
            while (p != null)
            {
                if (p.DebuggerCallback != null)
                {
                    p.DebuggerCallback(this, new DebuggerCallbackEventArgs { Statement = nextStatement });
                    return;
                }
                p = p._parent;
            }
            if (Debugger.IsAttached)
                Debugger.Break();
        }

        public void DefineConstructor(Type moduleType)
        {
            if (_variables == null)
                _variables = JSObject.getFieldsContainer();

            var name = moduleType.GetTypeInfo().IsGenericType ? moduleType.Name.Substring(0, moduleType.Name.LastIndexOf('`')) : moduleType.Name;

            DefineConstructor(moduleType, name);
        }

        public void DefineConstructor(Type type, string name)
        {
            var ctor = GlobalContext.GetConstructor(type);
            _variables.Add(name, ctor);
            ctor._attributes |= JSValueAttributesInternal.DoNotEnumerate;
        }

        public virtual bool DeleteVariable(string variableName)
        {
            return _variables.Remove(variableName);
        }

        internal void SetAbortState(ExecutionMode abortReason, JSValue abortInfo)
        {
            _executionMode = abortReason;
            _executionInfo = abortInfo;
        }

        /// <summary>
        /// Evaluate script
        /// </summary>
        /// <param name="code">Code in JavaScript</param>
        /// <returns>Result of last evaluated operation</returns>
        public JSValue Eval(string code, bool suppressScopeCreation = false)
        {
            return Eval(code, ThisBind, suppressScopeCreation);
        }

        /// <summary>
        /// Evaluate script
        /// </summary>
        /// <param name="code">Code in JavaScript</param>
        /// <param name="suppressScopeCreation">If true, scope will not be created. All variables, which will be defined via let, const or class will not be destructed after evalution</param>
        /// <returns>Result of last evaluated operation</returns>
        public JSValue Eval(string code, JSValue thisBind, bool suppressScopeCreation = false)
        {
            if (_parent == null)
                throw new InvalidOperationException("Cannot execute script in global context");

            if (string.IsNullOrEmpty(code))
                return JSValue.undefined;

            var mainFunctionContext = this;
            var stack = GetCurrectContextStack();
            while (stack != null
                && stack.Count > 1
                && stack[stack.Count - 2] == mainFunctionContext._parent
                && stack[stack.Count - 2]._owner == mainFunctionContext._owner)
            {
                mainFunctionContext = mainFunctionContext._parent;
            }

            int index = 0;
            string c = Parser.RemoveComments(code, 0);
            var ps = new ParseInfo(c, code, null)
            {
                strict = _strict,
                AllowDirectives = true,
                CodeContext = CodeContext.InEval
            };

            var body = CodeBlock.Parse(ps, ref index) as CodeBlock;
            if (index < c.Length)
                throw new ArgumentException("Invalid char");
            var variables = new Dictionary<string, VariableDescriptor>();
            var stats = new FunctionInfo();

            CodeNode cb = body;
            Parser.Build(ref cb, 0, variables, (_strict ? CodeContext.Strict : CodeContext.None) | CodeContext.InEval, null, stats, Options.None);

            var tv = stats.WithLexicalEnvironment ? null : new Dictionary<string, VariableDescriptor>();
            body.RebuildScope(stats, tv, body._variables.Length == 0 || !stats.WithLexicalEnvironment ? 1 : 0);
            if (tv != null)
            {
                var newVarDescs = new VariableDescriptor[tv.Values.Count];
                tv.Values.CopyTo(newVarDescs, 0);
                body._variables = newVarDescs;
                body._suppressScopeIsolation = SuppressScopeIsolationMode.DoNotSuppress;
            }

            body.Optimize(ref cb, null, null, Options.SuppressUselessExpressionsElimination | Options.SuppressConstantPropogation, stats);
            body = cb as CodeBlock ?? body;

            if (stats.NeedDecompose)
                body.Decompose(ref cb);

            body._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;

            var debugging = Debugging;
            Debugging = false;
            var runned = Activate();

            try
            {
                var context = suppressScopeCreation || (!stats.WithLexicalEnvironment && !body._strict && !_strict) ? this : new Context(this, false, _owner)
                {
                    _strict = _strict || body._strict
                };

                if (suppressScopeCreation || (!_strict && !body._strict))
                {
                    foreach (var var in body._variables)
                    {
                        if (var.lexicalScope) continue;

                        JSValue variable;
                        var cc = mainFunctionContext;
                        while (cc._parent._parent != null
                               && (cc._variables == null || !cc._variables.TryGetValue(var.name, out variable)))
                        {
                            cc = cc._parent;
                        }

                        if (cc._definedVariables != null)
                        {
                            for (var j = 0; j < cc._definedVariables.Length; j++)
                            {
                                if (cc._definedVariables[j].name == var.name)
                                {
                                    cc._definedVariables[j].DefinitionScopeLevel = -1;
                                }
                            }
                        }

                        variable = mainFunctionContext.DefineVariable(var.name, !suppressScopeCreation);

                        if (var.initializer != null)
                        {
                            variable.Assign(var.initializer.Evaluate(context));
                        }

                        // блокирует создание переменной в конктексте eval
                        var.lexicalScope = true;

                        // блокирует кеширование
                        var.DefinitionScopeLevel = -1;
                    }
                }

                if (body._lines.Length == 0)
                    return JSValue.undefined;

                var oldThisBind = ThisBind;
                var runContextOfEval = context.Activate();
                context._thisBind = thisBind;
                try
                {
                    return body.Evaluate(context) ?? context._lastResult ?? JSValue.notExists;
                }
                catch (JSException e)
                {
                    if ((e.Code == null || e.CodeCoordinates == null) && e.ExceptionMaker != null)
                    {
                        e.Code = code;
                        e.CodeCoordinates = CodeCoordinates.FromTextPosition(code, e.ExceptionMaker.Position, e.ExceptionMaker.Length);
                    }

                    throw;
                }
                finally
                {
                    context._thisBind = oldThisBind;
                    if (runContextOfEval)
                        context.Deactivate();
                }
            }
            finally
            {
                if (runned)
                    Deactivate();
                Debugging = debugging;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _variables.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)this).GetEnumerator();
        }

        public override string ToString()
        {
            return "Context of " + _owner.name;
        }
    }
}
