using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Functions;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Extensions;
using Array = Maddalena.Core.Javascript.BaseLibrary.Array;
using Boolean = Maddalena.Core.Javascript.BaseLibrary.Boolean;
using Math = Maddalena.Core.Javascript.BaseLibrary.Math;
using String = Maddalena.Core.Javascript.BaseLibrary.String;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public enum IndexersSupport
    {
        WithAttributeOnly = 0,
        ForceEnable,
        ForceDisable
    }

    [Serializable]
    public sealed class GlobalContext : Context
    {
        internal int _callDepth;
        internal JSObject _globalPrototype;
        private readonly Dictionary<Type, JSObject> _proxies;

        public string Name { get; }
        public IndexersSupport IndexersSupport { get; set; }
        public JsonSerializersRegistry JsonSerializersRegistry { get; set; }

        public GlobalContext()
            : this("")
        {
            Name = null;
        }

        public GlobalContext(string name)
            : base(null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            _proxies = new Dictionary<Type, JSObject>();
            JsonSerializersRegistry = new JsonSerializersRegistry();

            ResetContext();
        }

        internal void ResetContext()
        {
            if (_parent != null)
                throw new InvalidOperationException("Try to reset non-global context");

            ActivateInCurrentThread();
            try
            {
                if (_variables == null)
                    _variables = JSObject.getFieldsContainer();
                else
                    _variables.Clear();

                _proxies.Clear();
                _globalPrototype = null;

                var objectConstructor = GetConstructor(typeof(JSObject)) as Function;
                _variables.Add("Object", objectConstructor);
                objectConstructor._attributes |= JsValueAttributesInternal.DoNotDelete;

                _globalPrototype = objectConstructor.prototype as JSObject;
                _globalPrototype._objectPrototype = JSValue.@null;

                DefineConstructor(typeof(Math));
                DefineConstructor(typeof(Array));
                DefineConstructor(typeof(JSON));
                DefineConstructor(typeof(String));
                DefineConstructor(typeof(Function));
                DefineConstructor(typeof(Date));
                DefineConstructor(typeof(Number));
                DefineConstructor(typeof(Symbol));
                DefineConstructor(typeof(Boolean));
                DefineConstructor(typeof(Error));
                DefineConstructor(typeof(TypeError));
                DefineConstructor(typeof(ReferenceError));
                DefineConstructor(typeof(EvalError));
                DefineConstructor(typeof(RangeError));
                DefineConstructor(typeof(URIError));
                DefineConstructor(typeof(SyntaxError));
                DefineConstructor(typeof(RegExp));
                DefineConstructor(typeof(ArrayBuffer));
                DefineConstructor(typeof(Int8Array));
                DefineConstructor(typeof(Uint8Array));
                DefineConstructor(typeof(Uint8ClampedArray));
                DefineConstructor(typeof(Int16Array));
                DefineConstructor(typeof(Uint16Array));
                DefineConstructor(typeof(Int32Array));
                DefineConstructor(typeof(Uint32Array));
                DefineConstructor(typeof(Float32Array));
                DefineConstructor(typeof(Float64Array));
                DefineConstructor(typeof(Promise));
                DefineConstructor(typeof(Map));
                DefineConstructor(typeof(Set));

                DefineConstructor(typeof(Debug));
                DefineVariable("console").Assign(JSValue.Marshal(new JSConsole()));

                DefineVariable("eval").Assign(new EvalFunction());
                _variables["eval"]._attributes |= JsValueAttributesInternal.Eval;
                DefineVariable("isNaN").Assign(new ExternalFunction(GlobalFunctions.isNaN));
                DefineVariable("unescape").Assign(new ExternalFunction(GlobalFunctions.unescape));
                DefineVariable("escape").Assign(new ExternalFunction(GlobalFunctions.escape));
                DefineVariable("encodeURI").Assign(new ExternalFunction(GlobalFunctions.encodeURI));
                DefineVariable("encodeURIComponent").Assign(new ExternalFunction(GlobalFunctions.encodeURIComponent));
                DefineVariable("decodeURI").Assign(new ExternalFunction(GlobalFunctions.decodeURI));
                DefineVariable("decodeURIComponent").Assign(new ExternalFunction(GlobalFunctions.decodeURIComponent));
                DefineVariable("isFinite").Assign(new ExternalFunction(GlobalFunctions.isFinite));
                DefineVariable("parseFloat").Assign(new ExternalFunction(GlobalFunctions.parseFloat));
                DefineVariable("parseInt").Assign(new ExternalFunction(GlobalFunctions.parseInt));


                DefineVariable("__pinvoke").Assign(new ExternalFunction(GlobalFunctions.__pinvoke));

                #region Consts
                _variables["undefined"] = JSValue.undefined;
                _variables["Infinity"] = Number.POSITIVE_INFINITY;
                _variables["NaN"] = Number.NaN;
                _variables["null"] = JSValue.@null;
                #endregion

                foreach (var v in _variables.Values)
                    v._attributes |= JsValueAttributesInternal.DoNotEnumerate;
            }
            finally
            {
                Deactivate();
            }
        }

        public void ActivateInCurrentThread()
        {
            if (CurrentContext != null)
                throw new InvalidOperationException();

            if (!Activate())
                throw new Exception("Unable to activate base context");
        }

        public new void Deactivate()
        {
            if (CurrentContext != this)
                throw new InvalidOperationException();

            if (base.Deactivate() != null)
                throw new InvalidOperationException("Invalid state");
        }

        internal JSObject GetPrototype(Type type)
        {
            return (GetConstructor(type) as Function)?.prototype as JSObject;
        }

        public JSObject GetConstructor(Type type)
        {
            if (_proxies.TryGetValue(type, out var constructor)) return constructor;

            lock (_proxies)
            {
                JSObject dynamicProxy = null;

                if (type.GetTypeInfo().ContainsGenericParameters)
                {
                    constructor = GetGenericTypeSelector(new[] { type });
                }
                else
                {
                    var indexerSupport = IndexersSupport == IndexersSupport.ForceEnable
                                         || (IndexersSupport == IndexersSupport.WithAttributeOnly && type.GetTypeInfo().IsDefined(typeof(UseIndexersAttribute), false));

                    var staticProxy = new StaticProxy(this, type, indexerSupport);
                    if (type.GetTypeInfo().IsAbstract)
                    {
                        _proxies[type] = staticProxy;
                        return staticProxy;
                    }

                    JSObject parentPrototype = null;

                    var pa = type.GetTypeInfo().GetCustomAttributes(typeof(PrototypeAttribute), true).ToArray();

                    if (pa.Length != 0 && (pa[0] as PrototypeAttribute).PrototypeType != type)
                    {
                        var protoAtt = (PrototypeAttribute) pa[0];
                        var parentType = protoAtt.PrototypeType;
                        parentPrototype = (GetConstructor(parentType) as Function).prototype as JSObject;

                        dynamicProxy = protoAtt.Replace && parentType.IsAssignableFrom(type)
                            ? parentPrototype
                            : new PrototypeProxy(this, type, indexerSupport)
                            {
                                _objectPrototype = parentPrototype
                            };
                    }
                    else
                    {
                        dynamicProxy = new PrototypeProxy(this, type, indexerSupport);
                    }

                    constructor = type == typeof(JSObject) ? new ObjectConstructor(this, staticProxy, dynamicProxy) : new ConstructorProxy(this, staticProxy, dynamicProxy);

                    if (type.GetTypeInfo().IsDefined(typeof(ImmutableAttribute), false))
                        dynamicProxy._attributes |= JsValueAttributesInternal.Immutable;
                    constructor._attributes = dynamicProxy._attributes;
                    dynamicProxy._attributes |= JsValueAttributesInternal.DoNotDelete | JsValueAttributesInternal.DoNotEnumerate | JsValueAttributesInternal.NonConfigurable | JsValueAttributesInternal.ReadOnly;

                    if (dynamicProxy != parentPrototype && type != typeof(ConstructorProxy))
                    {
                        dynamicProxy._fields["constructor"] = constructor;
                    }
                }

                _proxies[type] = constructor;

                if (dynamicProxy == null || !typeof(JSValue).IsAssignableFrom(type)) return constructor;

                if (dynamicProxy._objectPrototype == null)
                    dynamicProxy._objectPrototype = _globalPrototype ?? JSValue.@null;
            }

            return constructor;
        }

        public Function GetGenericTypeSelector(IList<Type> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                for (var j = i + 1; j < types.Count; j++)
                {
                    if (types[i].GetGenericArguments().Length == types[j].GetGenericArguments().Length)
                        ExceptionHelper.Throw(new InvalidOperationException("Types have the same arguments"));
                }
            }

            return new ExternalFunction((_this, args) =>
            {
                var type = types.FirstOrDefault(t => t.GetGenericArguments().Length == args.length);

                if (type == null)
                    ExceptionHelper.ThrowTypeError("Invalid arguments count for generic constructor");

                if (args.length == 0)
                    return GetConstructor(type);

                var parameters = new Type[args.length];
                for (var i = 0; i < args.length; i++)
                {
                    parameters[i] = args[i].As<Type>();
                    if (parameters[i] == null)
                        ExceptionHelper.ThrowTypeError("Invalid argument #" + i + " for generic constructor");
                }

                return GetConstructor(type.MakeGenericType(parameters));
            });
        }

        public JSValue ProxyValue(object value)
        {
            if (value == null) return JSValue.NotExists;

            if (value is JSValue jsvalue) return jsvalue;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    {
                        return new JSValue
                        {
                            _iValue = (bool)value ? 1 : 0,
                            _valueType = JSValueType.Boolean
                        };
                    }
                case TypeCode.Byte:
                    {
                        return new JSValue
                        {
                            _iValue = (byte)value,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.Char:
                    {
                        return new JSValue
                        {
                            _oValue = ((char)value).ToString(),
                            _valueType = JSValueType.String
                        };
                    }
                case TypeCode.DateTime:
                    {
                        var dateTime = (DateTime)value;
                        return new ObjectWrapper(new Date(dateTime));
                    }
                case TypeCode.Decimal:
                    {
                        return new JSValue
                        {
                            _dValue = (double)(decimal)value,
                            _valueType = JSValueType.Double
                        };
                    }
                case TypeCode.Double:
                    {
                        return new JSValue
                        {
                            _dValue = (double)value,
                            _valueType = JSValueType.Double
                        };
                    }
                case TypeCode.Int16:
                    {
                        return new JSValue
                        {
                            _iValue = (short)value,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.Int32:
                    {
                        return new JSValue
                        {
                            _iValue = (int)value,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.Int64:
                    {
                        return new JSValue
                        {
                            _dValue = (long)value,
                            _valueType = JSValueType.Double
                        };
                    }
                case TypeCode.SByte:
                    {
                        return new JSValue
                        {
                            _iValue = (sbyte)value,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.Single:
                    {
                        return new JSValue
                        {
                            _dValue = (float)value,
                            _valueType = JSValueType.Double
                        };
                    }
                case TypeCode.String:
                    {
                        return new JSValue
                        {
                            _oValue = value,
                            _valueType = JSValueType.String
                        };
                    }
                case TypeCode.UInt16:
                    {
                        return new JSValue
                        {
                            _iValue = (ushort)value,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.UInt32:
                    {
                        var v = (uint)value;
                        if ((int)v != v)
                        {
                            return new JSValue
                            {
                                _dValue = v,
                                _valueType = JSValueType.Double
                            };
                        }

                        return new JSValue
                        {
                            _iValue = (int)v,
                            _valueType = JSValueType.Integer
                        };
                    }
                case TypeCode.UInt64:
                    {
                        var v = (long)value;
                        if ((int)v != v)
                        {
                            return new JSValue
                            {
                                _dValue = v,
                                _valueType = JSValueType.Double
                            };
                        }

                        return new JSValue
                        {
                            _iValue = (int)v,
                            _valueType = JSValueType.Integer
                        };
                    }
                default:
                {
                    switch (value)
                    {
                        case Delegate @delegate:
                            if (@delegate is ExternalFunctionDelegate)
                                return new ExternalFunction(@delegate as ExternalFunctionDelegate);
                            return new MethodProxy(this, @delegate.GetMethodInfo(), @delegate.Target);
                        case IList list:
                            return new NativeList(list);
                        case ExpandoObject _:
                            return new ExpandoObjectWrapper(value as ExpandoObject);
                    }

                    if (!(value is Task)) return new ObjectWrapper(value);

                    Task<JSValue> result;
                    if (value.GetType().GetTypeInfo().IsGenericType && typeof(Task<>).IsAssignableFrom(value.GetType().GetGenericTypeDefinition()))
                    {
                        result = new Task<JSValue>(() => ProxyValue(value.GetType().GetMethod("get_Result", new Type[0]).Invoke(value, null)));
                    }
                    else
                    {
                        result = new Task<JSValue>(() => JSValue.NotExists);
                    }

                    ((Task) value).ContinueWith(task => result.Start());
                    return new ObjectWrapper(new Promise(result));

                }
            }
        }

        public override string ToString()
        {
            const string result = "Global Context";
            return string.IsNullOrEmpty(Name) ? result : $"{result} \"{Name}\"";
        }
    }
}
