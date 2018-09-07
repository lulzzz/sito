using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JS.Core.Core.Interop;
using JS.Core.Expressions;
using NiL.JS;
using NiL.JS.BaseLibrary;

namespace JS.Core.Core.Functions
{
    [Flags]
    internal enum ConvertArgsOptions
    {
        Default = 0,
        ThrowOnError = 1,
        StrictConversion = 2,
        DummyValues = 4
    }

    [Prototype(typeof(Function), true)]
    internal sealed class MethodProxy : Function
    {
        private delegate object WrapperDelegate(object target, Context initiator, Expression[] arguments, Arguments argumentsObject);

        private static readonly Dictionary<MethodBase, WrapperDelegate> WrapperCache = new Dictionary<MethodBase, WrapperDelegate>();
        private static readonly MethodInfo ArgumentsGetItemMethod = typeof(Arguments).GetMethod("get_Item", new[] { typeof(int) });

        private readonly WrapperDelegate _fastWrapper;
        private readonly bool _forceInstance;
        private readonly bool _strictConversion;
        private readonly ConvertValueAttribute[] _paramsConverters;
        private readonly string _name;

        internal readonly ParameterInfo[] _parameters;
        internal readonly bool _raw;
        internal readonly MethodBase _method;
        internal readonly object _hardTarget;
        internal readonly ConvertValueAttribute _returnConverter;

        public ParameterInfo[] Parameters => _parameters;

        public override string name => _name;

        public override JSValue prototype
        {
            get => null;
            set
            {

            }
        }

        public MethodProxy(Context context, MethodBase methodBase)
            : this(context, methodBase, null)
        {
        }

        public MethodProxy(Context context, MethodBase methodBase, object hardTarget)
            : base(context)
        {
            _method = methodBase;
            _hardTarget = hardTarget;
            _parameters = methodBase.GetParameters();
            _strictConversion = methodBase.IsDefined(typeof(StrictConversionAttribute), true);
            _name = methodBase.Name;

            if (methodBase.IsDefined(typeof(JavaScriptNameAttribute), false))
            {
                _name = (methodBase.GetCustomAttributes(typeof(JavaScriptNameAttribute), false).First() as JavaScriptNameAttribute).Name;
                if (_name.StartsWith("@@"))
                    _name = _name.Substring(2);
            }

            if (_length == null)
                _length = new Number(0) { _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate | JSValueAttributesInternal.SystemObject };

            if (methodBase.IsDefined(typeof(ArgumentsCountAttribute), false))
            {
                var argsCountAttribute = methodBase.GetCustomAttributes(typeof(ArgumentsCountAttribute), false).First() as ArgumentsCountAttribute;
                _length._iValue = argsCountAttribute.Count;
            }
            else
            {
                _length._iValue = _parameters.Length;
            }

            for (int i = 0; i < _parameters.Length; i++)
            {
                if (_parameters[i].IsDefined(typeof(ConvertValueAttribute), false))
                {
                    var t = _parameters[i].GetCustomAttributes(typeof(ConvertValueAttribute), false).First();
                    if (_paramsConverters == null)
                        _paramsConverters = new ConvertValueAttribute[_parameters.Length];
                    _paramsConverters[i] = t as ConvertValueAttribute;
                }
            }

            var methodInfo = methodBase as MethodInfo;
            if (methodInfo != null)
            {
                _returnConverter = methodInfo.ReturnParameter.GetCustomAttribute(typeof(ConvertValueAttribute), false) as ConvertValueAttribute;

                _forceInstance = methodBase.IsDefined(typeof(InstanceMemberAttribute), false);

                if (_forceInstance)
                {
                    if (!methodInfo.IsStatic
                        || (_parameters.Length == 0)
                        || (_parameters.Length > 2)
                        || (_parameters[0].ParameterType != typeof(JSValue))
                        || (_parameters.Length > 1 && _parameters[1].ParameterType != typeof(Arguments)))
                        throw new ArgumentException("Force-instance method \"" + methodBase + "\" has invalid signature");

                    _raw = true;
                }

                if (!WrapperCache.TryGetValue(methodBase, out _fastWrapper))
                    WrapperCache[methodBase] = _fastWrapper = makeFastWrapper(methodInfo);

                _raw |= _parameters.Length == 0
                    || (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(Arguments));

                RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
            }
            else if (methodBase is ConstructorInfo)
            {
                if (!WrapperCache.TryGetValue(methodBase, out _fastWrapper))
                    WrapperCache[methodBase] = _fastWrapper = makeFastWrapper(methodBase as ConstructorInfo);

                _raw |= _parameters.Length == 0
                    || (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(Arguments));
            }
            else
                throw new NotImplementedException();
        }

        private MethodProxy(Context context, bool raw, object hardTarget, MethodBase method, ParameterInfo[] parameters, WrapperDelegate fastWrapper, bool forceInstance)
            : base(context)
        {
            _raw = raw;
            _hardTarget = hardTarget;
            _method = method;
            _parameters = parameters;
            _fastWrapper = fastWrapper;
            _forceInstance = forceInstance;
            RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;
        }

        private WrapperDelegate makeFastWrapper(MethodInfo methodInfo)
        {
            System.Linq.Expressions.Expression tree = null;
            var target = System.Linq.Expressions.Expression.Parameter(typeof(object), "target");
            var context = System.Linq.Expressions.Expression.Parameter(typeof(Context), "context");
            var arguments = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]), "arguments");
            var argumentsObjectPrm = System.Linq.Expressions.Expression.Parameter(typeof(Arguments), "argumentsObject");
            var argumentsObject = System.Linq.Expressions.Expression.Condition(
                System.Linq.Expressions.Expression.NotEqual(argumentsObjectPrm, System.Linq.Expressions.Expression.Constant(null)),
                argumentsObjectPrm,
                System.Linq.Expressions.Expression.Assign(argumentsObjectPrm, System.Linq.Expressions.Expression.Call(((Func<Expression[], Context, Arguments>)Tools.CreateArguments).GetMethodInfo(), arguments, context)));

            if (_forceInstance)
            {
                for (;;)
                {
                    if (methodInfo.IsStatic && _parameters[0].ParameterType == typeof(JSValue))
                    {
                        if (_parameters.Length == 1)
                        {
                            tree = System.Linq.Expressions.Expression.Call(methodInfo, System.Linq.Expressions.Expression.Convert(target, typeof(JSValue)));
                            break;
                        }

                        if (_parameters.Length == 2 && _parameters[1].ParameterType == typeof(Arguments))
                        {
                            tree = System.Linq.Expressions.Expression.Call(
                                methodInfo,
                                System.Linq.Expressions.Expression.Convert(target, typeof(JSValue)),
                                argumentsObject);
                            break;
                        }
                    }

                    throw new ArgumentException("Invalid method signature");
                }
            }
            else switch (_parameters.Length)
            {
                case 0:
                    tree = methodInfo.IsStatic ? System.Linq.Expressions.Expression.Call(methodInfo) : System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo);
                    break;
                case 1 when _parameters[0].ParameterType == typeof(Arguments):
                    tree = methodInfo.IsStatic ? System.Linq.Expressions.Expression.Call(methodInfo, argumentsObject) : System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo, argumentsObject);
                    break;
                default:
                    var processArg = ((Func<Expression[], Context, int, object>)processArgument).GetMethodInfo();
                    var processArgTail = ((Func<Expression[], Context, int, object>)processArgumentsTail).GetMethodInfo();
                    var convertArg = ((Func<int, JSValue, object>)convertArgument).GetMethodInfo();

                    var prms = new System.Linq.Expressions.Expression[_parameters.Length];
                    for (var i = 0; i < prms.Length; i++)
                    {
                        prms[i] = System.Linq.Expressions.Expression.Convert(
                            System.Linq.Expressions.Expression.Call(
                                System.Linq.Expressions.Expression.Constant(this),
                                i + 1 < prms.Length ? processArg : processArgTail,
                                arguments,
                                context,
                                System.Linq.Expressions.Expression.Constant(i)),
                            _parameters[i].ParameterType);
                    }

                    if (methodInfo.IsStatic)
                        tree = System.Linq.Expressions.Expression.Call(methodInfo, prms);
                    else
                        tree = System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo, prms);

                    for (var i = 0; i < prms.Length; i++)
                    {
                        prms[i] = System.Linq.Expressions.Expression.Convert(
                            System.Linq.Expressions.Expression.Call(
                                System.Linq.Expressions.Expression.Constant(this),
                                convertArg,
                                System.Linq.Expressions.Expression.Constant(i),
                                System.Linq.Expressions.Expression.Call(argumentsObjectPrm, ArgumentsGetItemMethod, System.Linq.Expressions.Expression.Constant(i))),
                            _parameters[i].ParameterType);
                    }

                    System.Linq.Expressions.Expression treeWithObjectAsSource;
                    treeWithObjectAsSource = methodInfo.IsStatic ? System.Linq.Expressions.Expression.Call(methodInfo, prms) : System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Convert(target, methodInfo.DeclaringType), methodInfo, prms);

                    tree = System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Equal(argumentsObjectPrm, System.Linq.Expressions.Expression.Constant(null)),
                        tree,
                        treeWithObjectAsSource);
                    break;
            }

            if (methodInfo.ReturnType == typeof(void))
                tree = System.Linq.Expressions.Expression.Block(tree, System.Linq.Expressions.Expression.Constant(null));

            try
            {
                return System.Linq.Expressions.Expression
                    .Lambda<WrapperDelegate>(
                        System.Linq.Expressions.Expression.Convert(tree, typeof(object)),
                        methodInfo.Name,
                        new[]
                        {
                            target,
                            context,
                            arguments,
                            argumentsObjectPrm
                        })
                    .Compile();
            }
            catch
            {
                throw;
            }
        }

        private WrapperDelegate makeFastWrapper(ConstructorInfo constructorInfo)
        {
            System.Linq.Expressions.Expression tree = null;
            var target = System.Linq.Expressions.Expression.Parameter(typeof(object), "target");
            var context = System.Linq.Expressions.Expression.Parameter(typeof(Context), "context");
            var arguments = System.Linq.Expressions.Expression.Parameter(typeof(Expression[]), "arguments");
            var argumentsObjectPrm = System.Linq.Expressions.Expression.Parameter(typeof(Arguments), "argumentsObject");
            var argumentsObject = System.Linq.Expressions.Expression.Condition(
                System.Linq.Expressions.Expression.NotEqual(argumentsObjectPrm, System.Linq.Expressions.Expression.Constant(null)),
                argumentsObjectPrm,
                System.Linq.Expressions.Expression.Assign(argumentsObjectPrm, System.Linq.Expressions.Expression.Call(((Func<Expression[], Context, Arguments>)Tools.CreateArguments).GetMethodInfo(), arguments, context)));

            if (_parameters.Length == 0)
            {
                tree = System.Linq.Expressions.Expression.New(constructorInfo);
            }
            else
            {
                if (_parameters.Length == 1 && _parameters[0].ParameterType == typeof(Arguments))
                {
                    tree = System.Linq.Expressions.Expression.New(constructorInfo, argumentsObject);
                }
                else
                {
                    Func<Expression[], Context, int, object> processArg = processArgument;
                    Func<Expression[], Context, int, object> processArgTail = processArgumentsTail;
                    Func<int, JSValue, object> convertArg = convertArgument;

                    var prms = new System.Linq.Expressions.Expression[_parameters.Length];
                    for (var i = 0; i < prms.Length; i++)
                    {
                        prms[i] = System.Linq.Expressions.Expression.Convert(
                            System.Linq.Expressions.Expression.Call(
                                System.Linq.Expressions.Expression.Constant(this),
                                i + 1 < prms.Length ? processArg.GetMethodInfo() : processArgTail.GetMethodInfo(),
                                arguments,
                                context,
                                System.Linq.Expressions.Expression.Constant(i)),
                            _parameters[i].ParameterType);
                    }

                    tree = System.Linq.Expressions.Expression.New(constructorInfo, prms);

                    for (var i = 0; i < prms.Length; i++)
                    {
                        prms[i] = System.Linq.Expressions.Expression.Convert(
                            System.Linq.Expressions.Expression.Call(
                                System.Linq.Expressions.Expression.Constant(this),
                                convertArg.GetMethodInfo(),
                                System.Linq.Expressions.Expression.Constant(i),
                                System.Linq.Expressions.Expression.Call(argumentsObject, ArgumentsGetItemMethod, System.Linq.Expressions.Expression.Constant(i))),
                            _parameters[i].ParameterType);
                    }

                    System.Linq.Expressions.Expression treeWithObjectAsSource;
                    treeWithObjectAsSource = System.Linq.Expressions.Expression.New(constructorInfo, prms);

                    tree = System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Equal(argumentsObjectPrm, System.Linq.Expressions.Expression.Constant(null)),
                                                 tree,
                                                 treeWithObjectAsSource);
                }
            }

            try
            {
                return System.Linq.Expressions.Expression
                    .Lambda<WrapperDelegate>(
                        System.Linq.Expressions.Expression.Convert(tree, typeof(object)),
                        constructorInfo.DeclaringType.Name,
                        new[]
                        {
                            target,
                            context,
                            arguments,
                            argumentsObjectPrm
                        })
                    .Compile();
            }
            catch
            {
                throw;
            }
        }

        internal override JSValue InternalInvoke(JSValue targetValue, Expression[] argumentsSource, Context initiator, bool withSpread, bool withNew)
        {
            if (withNew)
            {
                if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithoutNewOnly)
                {
                    ExceptionHelper.ThrowTypeError(string.Format(Messages.InvalidTryToCreateWithoutNew, name));
                }
            }
            else
            {
                if (RequireNewKeywordLevel == RequireNewKeywordLevel.WithNewOnly)
                {
                    ExceptionHelper.ThrowTypeError(string.Format(Messages.InvalidTryToCreateWithoutNew, name));
                }
            }

            object value = invokeMethod(targetValue, argumentsSource, null, initiator);
            return Context.GlobalContext.ProxyValue(value);
        }

        private object invokeMethod(JSValue targetValue, Expression[] argumentsSource, Arguments argumentsObject, Context initiator)
        {
            object value;
            var target = GetTargetObject(targetValue, _hardTarget);
            try
            {
                if (_parameters.Length == 0 && argumentsSource != null)
                {
                    for (var i = 0; i < argumentsSource.Length; i++)
                        argumentsSource[i].Evaluate(initiator);
                }

                value = _fastWrapper(target, initiator, argumentsSource, argumentsObject);

                if (_returnConverter != null)
                    value = _returnConverter.From(value);
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                    e = e.InnerException;

                if (e is JSException)
                    throw e;

                ExceptionHelper.Throw(new TypeError(e.Message), e);
                throw;
            }

            return value;
        }

        private object processArgumentsTail(Expression[] arguments, Context context, int index)
        {
            var result = processArgument(arguments, context, index);

            while (++index < arguments.Length)
                arguments[index].Evaluate(context);

            return result;
        }

        internal object GetTargetObject(JSValue targetValue, object hardTarget)
        {
            var target = hardTarget;
            if (target == null)
            {
                if (_forceInstance)
                {
                    if (targetValue != null && targetValue._valueType >= JSValueType.Object)
                    {
                        // Объект нужно развернуть до основного значения. Даже если это обёртка над примитивным значением
                        target = targetValue.Value;

                        var proxy = target as Proxy;
                        if (proxy != null)
                            target = proxy.PrototypeInstance ?? targetValue.Value;

                        // ForceInstance работает только если первый аргумент типа JSValue
                        if (!(target is JSValue))
                            target = targetValue;
                    }
                    else
                        target = targetValue ?? undefined;
                }
                else if (!_method.IsStatic && !_method.IsConstructor)
                {
                    target = convertTargetObject(targetValue ?? undefined, _method.DeclaringType);
                    if (target == null)
                    {
                        // Исключительная ситуация. Я не знаю почему Function.length обобщённое свойство, а не константа. Array.length работает по-другому.
                        if (_method.Name == "get_length" && typeof(Function).IsAssignableFrom(_method.DeclaringType))
                            return Empty;

                        ExceptionHelper.Throw(new TypeError("Can not call function \"" + name + "\" for object of another type."));
                    }
                }
            }

            return target;
        }

        private static object convertTargetObject(JSValue target, Type targetType)
        {
            if (target == null)
                return null;

            target = target._oValue as JSValue ?? target; // это может быть лишь ссылка на какой-то другой контейнер
            var res = Tools.convertJStoObj(target, targetType, false);
            return res;
        }

        private object processArgument(Expression[] arguments, Context initiator, int index)
        {
            var value = arguments.Length > index ? Tools.EvalExpressionSafe(initiator, arguments[index]) : notExists;

            return convertArgument(index, value);
        }

        private object convertArgument(int index, JSValue value)
        {
            var cvtArgs = ConvertArgsOptions.ThrowOnError;
            if (_strictConversion)
                cvtArgs |= ConvertArgsOptions.StrictConversion;

            return convertArgument(
                index,
                value,
                cvtArgs);
        }

        private object convertArgument(int index, JSValue value, ConvertArgsOptions options)
        {
            if (_paramsConverters?[index] != null)
                return _paramsConverters[index].To(value);

            var strictConversion = options.HasFlag(ConvertArgsOptions.StrictConversion);
            var parameterInfo = _parameters[index];
            object result = null;

            if (value.IsNull && parameterInfo.ParameterType.GetTypeInfo().IsClass)
            {
                return null;
            }

            if (value.Defined)
            {
                result = Tools.convertJStoObj(value, parameterInfo.ParameterType, !strictConversion);
                if (strictConversion && result == null)
                {
                    if (options.HasFlag(ConvertArgsOptions.ThrowOnError))
                        ExceptionHelper.ThrowTypeError("Unable to convert " + value + " to type " + parameterInfo.ParameterType);

                    if (!options.HasFlag(ConvertArgsOptions.DummyValues))
                        return null;
                }
            }
            else
            {
                if (parameterInfo.ParameterType.IsInstanceOfType(value))
                    return value;
            }

            if (result == null && (options.HasFlag(ConvertArgsOptions.DummyValues) || parameterInfo.Attributes.HasFlag(ParameterAttributes.HasDefault)))
            {
                result = parameterInfo.DefaultValue;

                if (result is DBNull)
                {
                    if (strictConversion && options.HasFlag(ConvertArgsOptions.ThrowOnError))
                        ExceptionHelper.ThrowTypeError("Unable to convert " + value + " to type " + parameterInfo.ParameterType);

                    result = parameterInfo.ParameterType.GetTypeInfo().IsValueType ? Activator.CreateInstance(parameterInfo.ParameterType) : null;
                }
            }

            return result;
        }

        internal object[] ConvertArguments(Arguments arguments, ConvertArgsOptions options)
        {
            if (_parameters.Length == 0)
                return null;

            if (_forceInstance)
                ExceptionHelper.Throw(new InvalidOperationException());

            object[] res = null;
            int targetCount = _parameters.Length;
            for (int i = targetCount; i-- > 0;)
            {
                var jsValue = arguments?[i] ?? undefined;

                var value = convertArgument(i, jsValue, options);

                if (value == null && !jsValue.IsNull)
                    return null;

                if (res == null)
                    res = new object[targetCount];

                res[i] = value;
            }

            return res;
        }

        protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            if (arguments == null)
                arguments = new Arguments();

            var result = invokeMethod(targetObject, null, arguments, Context);

            if (result == null)
                return undefined;

            return result as JSValue ?? Context.GlobalContext.ProxyValue(result);
        }

        public override Function bind(Arguments args)
        {
            if (_hardTarget != null || args.Length == 0)
                return this;

            return new MethodProxy(
                Context,
                _raw,
                convertTargetObject(args[0], _method.DeclaringType) ?? args[0].Value as JSObject ?? args[0],
                _method,
                _parameters,
                _fastWrapper,
                _forceInstance);
        }

#if !NET40
        public override Delegate MakeDelegate(Type delegateType)
        {
            try
            {
                var methodInfo = _method as MethodInfo;
                return methodInfo.CreateDelegate(delegateType, _hardTarget);
            }
            catch
            {
            }

            return base.MakeDelegate(delegateType);
        }
#endif

        public override string ToString(bool headerOnly)
        {
            var result = "function " + name + "()";

            if (!headerOnly)
            {
                result += " { [native code] }";
            }

            return result;
        }
    }
}
