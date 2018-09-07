﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Interop;
using Array = Maddalena.Core.Javascript.BaseLibrary.Array;
using Math = System.Math;

namespace Maddalena.Core.Javascript.Core.Functions
{
#if !(PORTABLE)
    [Serializable]
#endif
    [Prototype(typeof(Function), true)]
    internal class ConstructorProxy : Function
    {
        /// <summary>
        /// На первом проходе будут выбираться методы со строгим соответствием типов
        ///
        /// На втором проходе будут выбираться методы, для которых
        /// получится преобразовать входные аргументы.
        ///
        /// На третьем проходе будет выбираться первый метод,
        /// для которого получится сгенерировать параметры по-умолчанию.
        ///
        /// Если нужен более строгий подбор, то количество проходов нужно
        /// уменьшить до одного
        /// </summary>
        private const int passesCount = 3;

        private static readonly object[] _emptyObjectArray = new object[0];
        internal readonly StaticProxy _staticProxy;
        private readonly MethodProxy[] constructors;

        public override string name => _staticProxy._hostedType.Name;

        public override JSValue prototype
        {
            get => _prototype;
            set
            {
                _prototype = value;
            }
        }

        public ConstructorProxy(Context context, StaticProxy staticProxy, JSObject prototype)
            : base(context)
        {
            if (staticProxy == null)
                throw new ArgumentNullException(nameof(staticProxy));
            if (prototype == null)
                throw new ArgumentNullException(nameof(prototype));

            _fields = staticProxy._fields;
            _staticProxy = staticProxy;
            _prototype = prototype;

#if (PORTABLE)
            if (_staticProxy._hostedType.GetTypeInfo().ContainsGenericParameters)
                ExceptionHelper.Throw(new TypeError(_staticProxy._hostedType.Name + " can't be created because it's generic type."));
#else
            if (_staticProxy._hostedType.ContainsGenericParameters)
                ExceptionHelper.ThrowTypeError(_staticProxy._hostedType.Name + " can't be created because it's generic type.");
#endif
            var withNewOnly = staticProxy._hostedType.GetTypeInfo().IsDefined(typeof(RequireNewKeywordAttribute), true);
            var withoutNewOnly = staticProxy._hostedType.GetTypeInfo().IsDefined(typeof(DisallowNewKeywordAttribute), true);

            if (withNewOnly && withoutNewOnly)
                ExceptionHelper.Throw(new InvalidOperationException("Unacceptably use of " + typeof(RequireNewKeywordAttribute).Name + " and " + typeof(DisallowNewKeywordAttribute).Name + " for same type."));

            if (withNewOnly)
                RequireNewKeywordLevel = RequireNewKeywordLevel.WithNewOnly;
            if (withoutNewOnly)
                RequireNewKeywordLevel = RequireNewKeywordLevel.WithoutNewOnly;

            if (_length == null)
                _length = new Number(0) { _attributes = JSValueAttributesInternal.ReadOnly | JSValueAttributesInternal.DoNotDelete | JSValueAttributesInternal.DoNotEnumerate };

#if (PORTABLE)
            var ctors = System.Linq.Enumerable.ToArray(staticProxy._hostedType.GetTypeInfo().DeclaredConstructors);
            var ctorsL = new List<MethodProxy>(ctors.Length + (staticProxy._hostedType.GetTypeInfo().IsValueType ? 1 : 0));
#else
            var ctors = staticProxy._hostedType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var ctorsL = new List<MethodProxy>(ctors.Length + (staticProxy._hostedType.IsValueType ? 1 : 0));
#endif
            for (int i = 0; i < ctors.Length; i++)
            {
                if (ctors[i].IsStatic)
                    continue;

                if (!ctors[i].IsDefined(typeof(HiddenAttribute), false) || ctors[i].IsDefined(typeof(ForceUseAttribute), true))
                {
                    ctorsL.Add(new MethodProxy(context, ctors[i]));
                    length._iValue = Math.Max(ctorsL[ctorsL.Count - 1]._length._iValue, _length._iValue);
                }
            }

            ctorsL.Sort((x, y) =>
                x.Parameters.Length == 1 && x.Parameters[0].ParameterType == typeof(Arguments) ? 1 :
                y.Parameters.Length == 1 && y.Parameters[0].ParameterType == typeof(Arguments) ? -1 :
                x.Parameters.Length - y.Parameters.Length);

            constructors = ctorsL.ToArray();
        }

        protected internal override JSValue GetProperty(JSValue key, bool forWrite, PropertyScope memberScope)
        {
            if (memberScope < PropertyScope.Super && key._valueType != JSValueType.Symbol)
            {
                var keyString = key.ToString();

                if (keyString == "prototype") // Все прокси-прототипы read-only и non-configurable. Это и оптимизация, и устранение необходимости навешивания атрибутов
                    return prototype;

                if (key._valueType != JSValueType.String)
                    key = keyString;

                JSValue res;
                if (forWrite || (keyString != "toString" && keyString != "constructor"))
                {
                    res = _staticProxy.GetProperty(key, forWrite && memberScope == PropertyScope.Own, memberScope);
                    if (res.Exists || (memberScope == PropertyScope.Own && forWrite))
                    {
                        if (forWrite && res.NeedClone)
                            res = _staticProxy.GetProperty(key, true, memberScope);

                        return res;
                    }

                    res = __proto__.GetProperty(key, forWrite, memberScope);
                    if (memberScope == PropertyScope.Own && (res._valueType != JSValueType.Property || (res._attributes & JSValueAttributesInternal.Field) == 0))
                        return notExists; // если для записи, то первая ветка всё разрулит и сюда выполнение не придёт

                    return res;
                }
            }

            return base.GetProperty(key, forWrite, memberScope);
        }

        protected internal override bool DeleteProperty(JSValue name)
        {
            return _staticProxy.DeleteProperty(name) && __proto__.DeleteProperty(name);
        }

        protected internal override JSValue Invoke(bool construct, JSValue targetObject, Arguments arguments)
        {
            var objc = targetObject as ObjectWrapper;
            if (construct) // new
            {

            }
            else
            {
                if (_staticProxy._hostedType == typeof(Date))
                    return new Date().ToString();
            }

            try
            {
                object obj;
                if (_staticProxy._hostedType == typeof(Array))
                {
                    if (arguments == null)
                    {
                        obj = new Array();
                    }
                    else
                    {
                        switch (arguments.length)
                        {
                            case 0:
                                obj = new Array();
                                break;
                            case 1:
                                {
                                    var a0 = arguments[0];
                                    switch (a0._valueType)
                                    {
                                        case JSValueType.Integer:
                                            obj = new Array(a0._iValue);
                                            break;
                                        case JSValueType.Double:
                                            obj = new Array(a0._dValue);
                                            break;
                                        default:
                                            obj = new Array(arguments);
                                            break;
                                    }
                                    break;
                                }
                            default:
                                obj = new Array(arguments);
                                break;
                        }
                    }
                }
                else
                {
                    if ((arguments == null || arguments.length == 0) && _staticProxy._hostedType.IsValueType)
                    {
                        obj = Activator.CreateInstance(_staticProxy._hostedType);
                    }
                    else
                    {
                        object[] args = null;
                        var constructor = findConstructor(arguments, ref args);

                        if (constructor == null)
                            ExceptionHelper.ThrowTypeError(_staticProxy._hostedType.Name + " can't be created.");

                        if (args == null)
                            args = new object[] { arguments };

                        var target = constructor.GetTargetObject(targetObject, null);
                        obj = target != null ? constructor._method.Invoke(target, args) : (constructor._method as ConstructorInfo).Invoke(args);
                    }
                }

                var res = obj as JSValue;

                if (construct)
                {
                    if (res != null)
                    {
                        // Для Number, Boolean и String
                        if (res._valueType < JSValueType.Object)
                        {
                            objc.instance = obj;
                            if (objc._objectPrototype == null)
                                objc._objectPrototype = res.__proto__;
                            res = objc;
                        }
                        else if (res._oValue is JSValue)
                        {
                            res._oValue = res;
                        }
                    }
                    else
                    {
                        objc.instance = obj;

                        objc._attributes |= _staticProxy._hostedType.GetTypeInfo().IsDefined(typeof(ImmutableAttribute), false) ? JSValueAttributesInternal.Immutable : JSValueAttributesInternal.None;
                        if (obj.GetType() == typeof(Date))
                            objc._valueType = JSValueType.Date;

                        res = objc;
                    }
                }
                else
                {
                    if (_staticProxy._hostedType == typeof(JSValue))
                    {
                        if ((res._oValue is JSValue) && (res._oValue as JSValue)._valueType >= JSValueType.Object)
                            return res._oValue as JSValue;
                    }

                    res = res ?? new ObjectWrapper(obj)
                    {
                        _attributes = JSValueAttributesInternal.SystemObject | (_staticProxy._hostedType.GetTypeInfo().IsDefined(typeof(ImmutableAttribute), false) ? JSValueAttributesInternal.Immutable : JSValueAttributesInternal.None)
                    };
                }

                return res;
            }
            catch (TargetInvocationException e)
            {
#if !(PORTABLE)
                if (Debugger.IsAttached)
                    Debugger.Log(10, "Exception", e.ToString());
#endif
                throw e.GetBaseException();
            }
        }

        protected internal override JSValue ConstructObject()
        {
            return new ObjectWrapper(null)
            {
                _objectPrototype = Context.GlobalContext.GetPrototype(_staticProxy._hostedType)
            };
        }

        private MethodProxy findConstructor(Arguments arguments, ref object[] args)
        {
            args = null;
            var len = arguments == null ? 0 : arguments.length;
            for (var pass = 0; pass < passesCount; pass++)
            {
                for (int i = 0; i < constructors.Length; i++)
                {
                    if (constructors[i]._parameters.Length == 1 && constructors[i]._raw)
                        return constructors[i];

                    if (pass == 1 || constructors[i]._parameters.Length == len)
                    {
                        if (len == 0)
                        {
                            args = _emptyObjectArray;
                        }
                        else
                        {
                            args = constructors[i].ConvertArguments(
                                arguments,
                                (pass >= 1 ? 0 : ConvertArgsOptions.StrictConversion)
                                | (pass >= 2 ? ConvertArgsOptions.DummyValues : 0));

                            if (args == null)
                                continue;

                            for (var j = args.Length; j-- > 0;)
                            {
                                if (args[j] != null ?
                                    !constructors[i]._parameters[j].ParameterType.IsAssignableFrom(args[j].GetType())
                                    :
#if (PORTABLE)
                                    constructors[i]._parameters[j].ParameterType.GetTypeInfo().IsValueType)
#else
                                    constructors[i]._parameters[j].ParameterType.IsValueType)
#endif
                                {
                                    j = 0;
                                    args = null;
                                }
                            }

                            if (args == null)
                                continue;
                        }

                        return constructors[i];
                    }
                }
            }

            return null;
        }

        protected internal override IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnumerable, EnumerationMode enumerationMode)
        {
            var e = __proto__.GetEnumerator(hideNonEnumerable, enumerationMode);
            while (e.MoveNext())
                yield return e.Current;
            e = _staticProxy.GetEnumerator(hideNonEnumerable, enumerationMode);
            while (e.MoveNext())
                yield return e.Current;
        }

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
