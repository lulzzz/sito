﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Functions;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Expressions;
using Boolean = Maddalena.Core.Javascript.BaseLibrary.Boolean;
using Math = System.Math;
using String = Maddalena.Core.Javascript.BaseLibrary.String;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    [Flags]
    internal enum JsValueAttributesInternal : uint
    {
        None = 0,
        DoNotEnumerate = 1 << 0,
        DoNotDelete = 1 << 1,
        ReadOnly = 1 << 2,
        Immutable = 1 << 3,
        NonConfigurable = 1 << 4,
        Argument = 1 << 16,
        SystemObject = 1 << 17,
        ProxyPrototype = 1 << 18,
        Field = 1 << 19,
        Eval = 1 << 20,
        Temporary = 1 << 21,
        Cloned = 1 << 22,
        Reassign = 1 << 25,
        IntrinsicFunction = 1 << 26,
        ConstructingObject = 1 << 27
    }

    [Serializable]
    [Flags]
    public enum JSAttributes
    {
        None = 0,
        DoNotEnumerate = 1 << 0,
        DoNotDelete = 1 << 1,
        ReadOnly = 1 << 2,
        Immutable = 1 << 3,
        NonConfigurable = 1 << 4
    }

    internal sealed class JSObjectDebugView
    {
        private readonly JSObject _jsObject;

        public JSObjectDebugView(JSValue jsValue)
        {
            _jsObject = jsValue as JSObject;
            if (_jsObject != null && _jsObject._valueType >= JSValueType.Object)
                _jsObject = (_jsObject._oValue as JSObject) ?? _jsObject;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<string, JSValue>[] Items => _jsObject?._fields?.ToArray();
    }

    [Serializable]
    [DebuggerTypeProxy(typeof(JSObjectDebugView))]
    [DebuggerDisplay("Value = {Value} ({ValueType})")]
    public class JSValue : IEnumerable<KeyValuePair<string, JSValue>>, IComparable<JSValue>, ICloneable, IConvertible
    {
        internal const int publicAttributesMask = 0x1f | (int)JsValueAttributesInternal.Reassign;

        internal static readonly JSValue numberString = "number";
        internal static readonly JSValue undefinedString = "undefined";
        internal static readonly JSValue stringString = "string";
        internal static readonly JSValue symbolString = "symbol";
        internal static readonly JSValue booleanString = "boolean";
        internal static readonly JSValue functionString = "function";
        internal static readonly JSValue objectString = "object";

        internal static readonly JSValue undefined = new JSValue { _valueType = JSValueType.Undefined, _attributes = JsValueAttributesInternal.DoNotDelete | JsValueAttributesInternal.DoNotEnumerate | JsValueAttributesInternal.ReadOnly | JsValueAttributesInternal.NonConfigurable | JsValueAttributesInternal.SystemObject };
        internal static readonly JSValue notExists = new JSValue { _valueType = JSValueType.NotExists, _attributes = JsValueAttributesInternal.DoNotDelete | JsValueAttributesInternal.DoNotEnumerate | JsValueAttributesInternal.ReadOnly | JsValueAttributesInternal.NonConfigurable | JsValueAttributesInternal.SystemObject };
        internal static readonly JSObject @null = new JSObject { _valueType = JSValueType.Object, _oValue = null, _attributes = JsValueAttributesInternal.DoNotEnumerate | JsValueAttributesInternal.SystemObject };
        internal static readonly JSValue nullString = new JSValue { _valueType = JSValueType.String, _oValue = "null", _attributes = JsValueAttributesInternal.DoNotDelete | JsValueAttributesInternal.DoNotEnumerate | JsValueAttributesInternal.SystemObject };

        [Hidden]
        public static JSValue Undefined { [Hidden] get { return undefined; } }
        [Hidden]
        public static JSValue NotExists
        {
            [Hidden]
            get
            {
                notExists._valueType = JSValueType.NotExists;
                return notExists;
            }
        }
        [Hidden]
        public static JSValue NotExistsInObject
        {
            [Hidden]
            get
            {
                notExists._valueType = JSValueType.NotExistsInObject;
                return notExists;
            }
        }
        [Hidden]
        public static JSValue Null { [Hidden] get { return @null; } }

        [Hidden]
        public virtual JSValue this[string name]
        {
            [Hidden]
            get
            {
                return GetProperty(name);
            }
            [Hidden]
            set
            {
                SetProperty(name, value ?? undefined, true);
            }
        }

        internal JsValueAttributesInternal _attributes;
        internal JSValueType _valueType;
        internal int _iValue;
        internal double _dValue;
        internal object _oValue;

        [Hidden]
        public virtual object Value
        {
            [Hidden]
            get
            {
                switch (_valueType)
                {
                    case JSValueType.Boolean:
                        return _iValue != 0;
                    case JSValueType.Integer:
                        return _iValue;
                    case JSValueType.Double:
                        return _dValue;
                    case JSValueType.String:
                        return _oValue.ToString();
                    case JSValueType.Symbol:
                        return _oValue;
                    case JSValueType.Object:
                    case JSValueType.Function:
                    case JSValueType.Property:
                    case JSValueType.SpreadOperatorResult:
                    case JSValueType.Date:
                        {
                            return _oValue != this && _oValue is JSObject ? (_oValue as JSObject).Value : _oValue;
                        }
                    default:
                        return null;
                }
            }
            protected set
            {
                switch (_valueType)
                {
                    case JSValueType.Boolean:
                        {
                            _iValue = (bool)value ? 1 : 0;
                            break;
                        }
                    case JSValueType.Integer:
                        {
                            _iValue = (int)value;
                            break;
                        }
                    case JSValueType.Double:
                        {
                            _dValue = (double)value;
                            break;
                        }
                    case JSValueType.String:
                        {
                            _oValue = (string)value;
                            break;
                        }
                    case JSValueType.Object:
                    case JSValueType.Function:
                    case JSValueType.Property:
                    case JSValueType.Date:
                        {
                            _oValue = value;
                            break;
                        }
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        [Hidden]
        public JSValueType ValueType
        {
            [Hidden]
            get
            {
                return _valueType;
            }
            protected set
            {
                _valueType = value;
            }
        }

        [Hidden]
        public JSAttributes Attributes
        {
            [Hidden]
            get
            {
                return (JSAttributes)((int)_attributes & publicAttributesMask);
            }
        }

        protected bool Reassign
        {
            get => (_attributes & JsValueAttributesInternal.Reassign) != 0;
            set
            {
                if (value)
                    _attributes |= JsValueAttributesInternal.Reassign;
                else
                    _attributes &= ~JsValueAttributesInternal.Reassign;
            }
        }

        [DoNotDelete]
        [DoNotEnumerate]
        [NotConfigurable]
       
        public virtual JSObject __proto__
        {
            [Hidden]
            get
            {
                if (_valueType >= JSValueType.Object
                    && _oValue != this
                    && (_oValue as JSObject) != null)
                    return (_oValue as JSObject).__proto__;

                if (!Defined || IsNull)
                    ExceptionHelper.Throw(new TypeError("Can not get prototype of null or undefined"));

                return GetDefaultPrototype();
            }
            [Hidden]
            set
            {
                if ((_attributes & JsValueAttributesInternal.Immutable) != 0)
                    return;
                if (_valueType < JSValueType.Object)
                    return;
                if (_oValue == this)
                    throw new InvalidOperationException();
                if (_oValue == null)
                    ExceptionHelper.Throw(new ReferenceError("Cannot set __proto__ of null"));
                (_oValue as JSObject).__proto__ = value;
            }
        }

        [Hidden]
        public bool Exists
        {
            [Hidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            { return _valueType >= JSValueType.Undefined; }
        }

        [Hidden]
        public bool Defined
        {
            [Hidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _valueType > JSValueType.Undefined;
            }
        }

        [Hidden]
        public bool IsNull
        {
            [Hidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _valueType >= JSValueType.Object && _oValue == null;
            }
        }

        [Hidden]
        public bool IsNumber
        {
            [Hidden]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            { return _valueType == JSValueType.Integer || _valueType == JSValueType.Double; }
        }

        internal bool NeedClone
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (_attributes & (JsValueAttributesInternal.ReadOnly | JsValueAttributesInternal.SystemObject)) == JsValueAttributesInternal.SystemObject;
            }
        }

        internal bool IsBox
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _valueType >= JSValueType.Object && _oValue != null && _oValue != this;
            }
        }

        internal JSObject GetDefaultPrototype()
        {
            switch (_valueType)
            {
                case JSValueType.Boolean:
                    return Context.CurrentGlobalContext.GetPrototype(typeof(Boolean));
                case JSValueType.Double:
                case JSValueType.Integer:
                    return Context.CurrentGlobalContext.GetPrototype(typeof(Number));
                case JSValueType.String:
                    return Context.CurrentGlobalContext.GetPrototype(typeof(String));
            }

            if (_oValue != null && _oValue != this)
            {
                var oValueAsJsObject = _oValue as JSValue;
                if (oValueAsJsObject != null)
                    return oValueAsJsObject.GetDefaultPrototype() ?? @null;
                return Context.CurrentGlobalContext.GetPrototype(_oValue.GetType());
            }

            return Context.CurrentGlobalContext.GetPrototype(GetType());
        }

        [Hidden]
        public JSValue GetProperty(string name)
        {
            return GetProperty((JSValue)name, false, PropertyScope.Common);
        }

        [Hidden]
        public JSValue GetProperty(string name, PropertyScope propertyScope)
        {
            return GetProperty((JSValue)name, false, propertyScope);
        }

        [Hidden]
        public JSValue DefineProperty(string name)
        {
            return GetProperty((JSValue)name, true, PropertyScope.Own);
        }

        /// <summary>
        /// Creates new property with Getter and Setter in the object in scope of current context
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="getter">Function called when there is an attempt to get a value. Can be null</param>
        /// <param name="setter">Function called when there is an attempt to set a value. Can be null</param>
        /// <exception cref="System.ArgumentException">if property already exists</exception>
        /// <exception cref="System.InvalidOperationException">if unable to create property</exception>
        [Hidden]
        public void DefineGetSetProperty(string name, Func<object> getter, Action<object> setter)
        {
            DefineGetSetProperty(Context.CurrentGlobalContext, name, getter, setter);
        }

        [Hidden]
        public void DefineGetSetProperty(Context context, string name, Func<object> getter, Action<object> setter)
        {
            var property = GetProperty(name);
            if (property.ValueType >= JSValueType.Undefined)
                throw new ArgumentException();

            property = DefineProperty(name);
            if (property.ValueType < JSValueType.Undefined)
                throw new InvalidOperationException();

            property._valueType = JSValueType.Property;

            Function jsGetter = null;
            if (getter != null)
            {
#if NET40
                jsGetter = new MethodProxy(context, getter.Method, getter.Target);
#else
                jsGetter = new MethodProxy(context, getter.GetMethodInfo(), getter.Target);
#endif
            }

            Function jsSetter = null;
            if (setter != null)
            {
#if NET40
                jsSetter = new MethodProxy(context, setter.Method, setter.Target);
#else
                jsSetter = new MethodProxy(context, setter.GetMethodInfo(), setter.Target);
#endif
            }

            property._oValue = new PropertyPair(jsGetter, jsSetter);
        }

        [Hidden]
        public bool DeleteProperty(string name)
        {
            if (name == null)
                throw new ArgumentNullException("memberName");
            return DeleteProperty((JSObject)name);
        }

        protected internal JSValue GetProperty(string name, bool forWrite, PropertyScope propertyScope)
        {
            return GetProperty((JSValue)name, forWrite, propertyScope);
        }

        protected internal virtual JSValue GetProperty(JSValue key, bool forWrite, PropertyScope propertyScope)
        {
            switch (_valueType)
            {
                case JSValueType.Boolean:
                    {
                        if (propertyScope == PropertyScope.Own)
                            return notExists;
                        return Context.CurrentGlobalContext.GetPrototype(typeof(Boolean)).GetProperty(key, false, PropertyScope.Common);
                    }
                case JSValueType.Integer:
                case JSValueType.Double:
                {
                    return propertyScope == PropertyScope.Own ? notExists 
                        : Context.CurrentGlobalContext.GetPrototype(typeof(Number)).GetProperty(key, false, PropertyScope.Common);
                }
                case JSValueType.String:
                    {
                        return stringGetProperty(key, propertyScope);
                    }
                case JSValueType.Undefined:
                case JSValueType.NotExists:
                case JSValueType.NotExistsInObject:
                    {
                        ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToGetProperty, key, "undefined"));
                        return null;
                    }
                default:
                    {
                        if (_oValue == this)
                            break;
                        if (_oValue == null)
                            ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToGetProperty, key, "null"));

                        if (_oValue is JSObject inObj)
                            return inObj.GetProperty(key, forWrite, propertyScope);
                        break;
                    }
            }
            ExceptionHelper.Throw(new InvalidOperationException("Method GetProperty(...) of custom types must be overridden"));
            return null;
        }

        private JSValue stringGetProperty(JSValue name, PropertyScope propertyScope)
        {
            if ((name._valueType == JSValueType.String || name._valueType >= JSValueType.Object)
                && string.CompareOrdinal(name._oValue.ToString(), "length") == 0)
            {
                if (_oValue is RopeString)
                    return (_oValue as RopeString).Length;
                return _oValue.ToString().Length;
            }

            int index;
            var dindex = Tools.JSObjectToDouble(name);

            if (dindex >= 0.0
                && ((index = (int)dindex) == dindex)
                && _oValue.ToString().Length > index)
                return _oValue.ToString()[index];

            if (propertyScope == PropertyScope.Own)
                return notExists;

            return Context.CurrentGlobalContext
                .GetPrototype(typeof(String))
                .GetProperty(name, false, PropertyScope.Common);
        }

        protected internal void SetProperty(JSValue name, JSValue value, bool throwOnError)
        {
            SetProperty(name, value, PropertyScope.Common, throwOnError);
        }

        protected internal virtual void SetProperty(JSValue name, JSValue value, PropertyScope propertyScope, bool throwOnError)
        {
            JSValue field;
            if (_valueType >= JSValueType.Object)
            {
                if (_oValue == null)
                    ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToSetProperty, name, "null"));

                if (_oValue == this)
                {
                    GetProperty(name, true, propertyScope).Assign(value);
                }

                field = _oValue as JSObject;
                field?.SetProperty(name, value, propertyScope, throwOnError);
            }
            else if (_valueType <= JSValueType.Undefined)
            {
                ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToSetProperty, name, "undefined"));
            }
        }

        protected internal virtual bool DeleteProperty(JSValue name)
        {
            if (_valueType >= JSValueType.Object)
            {
                if (_oValue == null)
                    ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToGetProperty, name, "null"));
                if (_oValue == this)
                    throw new InvalidOperationException();
                if (_oValue is JSObject obj)
                    return obj.DeleteProperty(name);
            }
            else if (_valueType <= JSValueType.Undefined)
                ExceptionHelper.ThrowTypeError(string.Format(Messages.TryingToGetProperty, name, "undefined"));
            return true;
        }

        [Hidden]
        public override bool Equals(object obj)
        {
            if (!(obj is JSValue))
                return false;
            return ReferenceEquals(obj, this) || StrictEqual.Check(this, obj as JSValue);
        }

        [Hidden]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [Hidden]
        public static implicit operator JSValue(char value)
        {
            return new String(value.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Hidden]
        public static implicit operator JSValue(bool value)
        {
            return (Boolean)value;
        }

        [Hidden]
        public static implicit operator JSValue(int value)
        {
            return new Number(value);
        }

        [Hidden]
        public static implicit operator JSValue(long value)
        {
            return new Number(value);
        }

        [Hidden]
        public static implicit operator JSValue(double value)
        {
            return new Number(value);
        }

        [Hidden]
        public static implicit operator JSValue(string value)
        {
            return new String(value);
        }

        [Hidden]
        public static explicit operator int(JSValue obj)
        {
            return Tools.JSObjectToInt32(obj);
        }

        [Hidden]
        public static explicit operator long(JSValue obj)
        {
            return Tools.JSObjectToInt64(obj);
        }

        [Hidden]
        public static explicit operator double(JSValue obj)
        {
            return Tools.JSObjectToDouble(obj);
        }

        [Hidden]
        public static explicit operator bool(JSValue obj)
        {
            switch (obj._valueType)
            {
                case JSValueType.Integer:
                case JSValueType.Boolean:
                    return obj._iValue != 0;
                case JSValueType.Double:
                    return !(obj._dValue == 0.0 || double.IsNaN(obj._dValue));
                case JSValueType.String:
                    return !string.IsNullOrEmpty(obj._oValue.ToString());
                case JSValueType.Object:
                case JSValueType.Date:
                case JSValueType.Function:
                    return obj._oValue != null;
            }
            return false;
        }

        [Hidden]
        public static implicit operator JSValue(Delegate action) => Marshal(action);

        [Hidden]
        public object Clone()
        {
            return CloneImpl();
        }

        internal JSValue CloneImpl()
        {
            return CloneImpl(true);
        }

        internal JSValue CloneImpl(bool force)
        {
            return CloneImpl(force, JsValueAttributesInternal.ReadOnly
                | JsValueAttributesInternal.SystemObject
                | JsValueAttributesInternal.Temporary
                | JsValueAttributesInternal.Reassign
                | JsValueAttributesInternal.ProxyPrototype);
        }

        internal virtual JSValue CloneImpl(JsValueAttributesInternal resetMask)
        {
            return CloneImpl(true, resetMask);
        }

        internal virtual JSValue CloneImpl(bool force, JsValueAttributesInternal resetMask)
        {
            if (!force && (_attributes & JsValueAttributesInternal.Cloned) != 0)
            {
                _attributes &= ~(JsValueAttributesInternal.Cloned | resetMask);
                return this;
            }

            var res = new JSValue();
            res.Assign(this);
            res._attributes = _attributes & ~resetMask;
            return res;
        }

        [Hidden]
        public override string ToString()
        {
            return BaseToString();
        }

        protected internal string BaseToString()
        {
            if (_valueType == JSValueType.String)
                return _oValue.ToString();
            if (_valueType <= JSValueType.Undefined)
                return "undefined";

            if (_valueType == JSValueType.Property)
            {
                var tempStr = "[";
                if ((_oValue as PropertyPair).getter != null)
                    tempStr += "Getter";
                if ((_oValue as PropertyPair).setter != null)
                    tempStr += (tempStr.Length != 1 ? "/Setter" : "Setter");
                if (tempStr.Length == 1)
                    return "[Invalid Property]";
                tempStr += "]";
                return tempStr;
            }

            var res = _valueType >= JSValueType.Object ? ToPrimitiveValue_String_Value() : this;
            switch (res._valueType)
            {
                case JSValueType.Boolean:
                    return res._iValue != 0 ? "true" : "false";
                case JSValueType.Integer:
                    return Tools.Int32ToString(res._iValue);
                case JSValueType.Double:
                    return Tools.DoubleToString(res._dValue);
                case JSValueType.String:
                    return res._oValue.ToString();
                default:
                    return (res._oValue ?? "null").ToString();
            }
        }

        [Hidden]
        public virtual void Assign(JSValue value)
        {
            if ((_attributes & (JsValueAttributesInternal.ReadOnly | JsValueAttributesInternal.SystemObject)) != 0)
                return;
            _valueType = value._valueType | JSValueType.Undefined;
            _iValue = value._iValue;
            _dValue = value._dValue;
            _oValue = value._oValue;
        }

        internal JSValue ToPrimitiveValue_Value_String()
        {
            return ToPrimitiveValue("valueOf", "toString");
        }

        internal JSValue ToPrimitiveValue_LocaleString_Value()
        {
            return ToPrimitiveValue("toLocaleString", "valueOf");
        }

        internal JSValue ToPrimitiveValue_String_Value()
        {
            return ToPrimitiveValue("toString", "valueOf");
        }

        internal JSValue ToPrimitiveValue(string func0, string func1 = null)
        {
            if (_valueType < JSValueType.Object || _oValue == null) return this;

            if (_oValue == null)
                return nullString;

            JSValue res;

            var tpvs = Tools.InvokeGetter(GetProperty(func0), this);
            if (tpvs._valueType == JSValueType.Function)
            {
                res = (tpvs._oValue as Function).Call(this, null);
                if (res._valueType == JSValueType.Object)
                {
                    if (res._oValue is String)
                        res = res._oValue as String;
                }

                if (res._valueType < JSValueType.Object)
                    return res;
            }

            if (func1 != null)
            {
                tpvs = Tools.InvokeGetter(GetProperty(func1), this);
                if (tpvs._valueType == JSValueType.Function)
                {
                    res = (tpvs._oValue as Function).Call(this, null);
                    if (res._valueType == JSValueType.Object)
                    {
                        if (res._oValue is String)
                            res = res._oValue as String;
                    }

                    if (res._valueType < JSValueType.Object)
                        return res;
                }
            }

            ExceptionHelper.Throw(new TypeError("Can't convert object to primitive value."));
            return this;
        }

        [Hidden]
        public JSObject ToObject()
        {
            if (_valueType >= JSValueType.Object)
                return _oValue as JSObject ?? @null;

            if (_valueType >= JSValueType.Undefined)
                return new ObjectWrapper(ToPrimitiveTypeContainer());

            return new JSObject { _valueType = JSValueType.Object };
        }

        [Hidden]
        public JSValue ToPrimitiveTypeContainer()
        {
            if (_valueType >= JSValueType.Object)
                return null;

            switch (_valueType)
            {
                case JSValueType.Boolean:
                    return this is Boolean ? this : new Boolean(_iValue != 0);
                case JSValueType.Integer:
                    return this is Number ? this : new Number(_iValue);
                case JSValueType.Double:
                    return this is Number ? this : new Number(_dValue);
                case JSValueType.String:
                    return this is String ? this : new String(_oValue.ToString());
                case JSValueType.Symbol:
                    return _oValue as Symbol;
            }

            return new JSValue { _valueType = JSValueType.Undefined };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Hidden]
        public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator()
        {
            return GetEnumerator(true, EnumerationMode.RequireValuesForWrite);
        }

        protected internal virtual IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator(bool hideNonEnumerable, EnumerationMode enumeratorMode)
        {
            if (_valueType >= JSValueType.Object && _oValue != this)
            {
                var innerObject = _oValue as JSValue;
                if (innerObject != null)
                    return innerObject.GetEnumerator(hideNonEnumerable, enumeratorMode);
            }

            return GetEnumeratorImpl(hideNonEnumerable);
        }

        private IEnumerator<KeyValuePair<string, JSValue>> GetEnumeratorImpl(bool hideNonEnum)
        {
            if (_valueType == JSValueType.String)
            {
                var strValue = _oValue.ToString();
                var len = strValue.Length;
                for (var i = 0; i < len; i++)
                    yield return new KeyValuePair<string, JSValue>(Tools.Int32ToString(i), strValue[i].ToString());
                if (!hideNonEnum)
                    yield return new KeyValuePair<string, JSValue>("length", len);
            }
            else if (_valueType == JSValueType.Object)
            {
                if (_oValue == this)
                    throw new InvalidOperationException("Internal error. #VaO");
            }
        }

       
        [DoNotEnumerate]
        [ArgumentsCount(0)]
        [AllowNullArguments]
        public virtual JSValue toString(Arguments args)
        {
            var self = _oValue as JSValue ?? this;
            switch (self._valueType)
            {
                case JSValueType.Integer:
                case JSValueType.Double:
                    {
                        return "[object Number]";
                    }
                case JSValueType.Undefined:
                    {
                        return "[object Undefined]";
                    }
                case JSValueType.String:
                    {
                        return "[object String]";
                    }
                case JSValueType.Boolean:
                    {
                        return "[object Boolean]";
                    }
                case JSValueType.Function:
                    {
                        return "[object Function]";
                    }
                case JSValueType.Date:
                case JSValueType.Object:
                    {
                        if (self._oValue == null)
                            return "[object Null]";

                        if (self._oValue is GlobalObject)
                            return self._oValue.ToString();

                        var tag = self.GetProperty(Symbol.toStringTag, false, PropertyScope.Common);
                        if (tag.Defined)
                        {
                            return $"[object {Tools.InvokeGetter(tag, self)}]";
                        }

                        if (self._oValue is Proxy proxy)
                        {
                            var hostedType = proxy._hostedType;

                            return hostedType == typeof(JSObject) ? "[object Object]" : $"[object {hostedType.Name}]";
                        }

                        return self.Value.GetType() == typeof(JSObject) 
                                ?  "[object Object]" 
                                : $"[object {self.Value.GetType().Name}]";
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        [DoNotEnumerate]
        public virtual JSValue toLocaleString()
        {
            var self = _oValue as JSValue ?? this;

            if (self._valueType >= JSValueType.Object && self._oValue == null)
                ExceptionHelper.Throw(new TypeError("toLocaleString calling on null."));

            if (self._valueType <= JSValueType.Undefined)
                ExceptionHelper.Throw(new TypeError("toLocaleString calling on undefined value."));

            if (self == this)
                return ToPrimitiveValue("toString");

            return self.toLocaleString();
        }

        [DoNotEnumerate]
        public virtual JSValue valueOf()
        {
            if (_valueType >= JSValueType.Object && _oValue == null)
                ExceptionHelper.Throw(new TypeError("valueOf calling on null."));

            if (_valueType <= JSValueType.Undefined)
                ExceptionHelper.Throw(new TypeError("valueOf calling on undefined value."));

            return _valueType < JSValueType.Object ? new JSObject { _valueType = JSValueType.Object, _oValue = this } : this;
        }

        [DoNotEnumerate]
        public virtual JSValue propertyIsEnumerable(Arguments args)
        {
            if (_valueType >= JSValueType.Object && _oValue == null)
                ExceptionHelper.Throw(new TypeError("propertyIsEnumerable calling on null."));

            if (_valueType <= JSValueType.Undefined)
                ExceptionHelper.Throw(new TypeError("propertyIsEnumerable calling on undefined value."));

            var name = args[0];
            string n = name.ToString();
            var res = GetProperty(n, PropertyScope.Own);
            res = (res.Exists) && ((res._attributes & JsValueAttributesInternal.DoNotEnumerate) == 0);
            return res;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DoNotEnumerate]
        public virtual JSValue isPrototypeOf(Arguments args)
        {
            if (_valueType >= JSValueType.Object && _oValue == null)
                ExceptionHelper.Throw(new TypeError("isPrototypeOf calling on null."));

            if (_valueType <= JSValueType.Undefined)
                ExceptionHelper.Throw(new TypeError("isPrototypeOf calling on undefined value."));

            if (args.GetProperty("length")._iValue == 0)
                return false;

            var a = args[0];
            a = a.__proto__;
            if (_valueType >= JSValueType.Object)
            {
                if (_oValue != null)
                {
                    while (a != null && a._valueType >= JSValueType.Object && a._oValue != null)
                    {
                        if (a._oValue == _oValue)
                            return true;
                        var pi = (a._oValue as StaticProxy)?.PrototypeInstance;
                        if (pi != null && (this == pi || this == pi._oValue))
                            return true;
                        a = a.__proto__;
                    }
                }
            }
            else
            {
                if (a._oValue == _oValue)
                    return true;
                var pi = (a._oValue as StaticProxy)?.PrototypeInstance;
                if (pi != null && (this == pi || this == pi._oValue))
                    return true;
            }

            return false;
        }

        [DoNotEnumerate]
        public virtual JSValue hasOwnProperty(Arguments args)
        {
            JSValue name = args[0];
            var res = GetProperty(name, false, PropertyScope.Own);
            return res.Exists;
        }

        #region Члены IConvertible
#if !(PORTABLE)
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool)this;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)Tools.JSObjectToInt32(this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            var s = ToString();
            return s.Length > 0 ? s[0] : '\0';
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            if (_valueType == JSValueType.Date)
                return (_oValue as Date).ToDateTime();
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)Tools.JSObjectToDouble(this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Tools.JSObjectToDouble(this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)Tools.JSObjectToInt32(this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (_valueType == JSValueType.Integer)
                return _iValue;
            return Tools.JSObjectToInt32(this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (byte)Tools.JSObjectToInt64(this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)Tools.JSObjectToInt32(this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)Tools.JSObjectToDouble(this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Tools.convertJStoObj(this, conversionType, true);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)Tools.JSObjectToInt32(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)Tools.JSObjectToInt32(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)Tools.JSObjectToInt64(this);
        }
#endif
        #endregion

        #region Члены IComparable<JSValue>

        [Hidden]
        public virtual int CompareTo(JSValue other)
        {
            if (_valueType == other._valueType)
            {
                switch (_valueType)
                {
                    case JSValueType.Undefined:
                    case JSValueType.NotExists:
                    case JSValueType.NotExistsInObject:
                        return 0;
                    case JSValueType.Boolean:
                    case JSValueType.Integer:
                        return _iValue - other._iValue;
                    case JSValueType.Double:
                        return Math.Sign(_dValue - other._dValue);
                    case JSValueType.String:
                        return string.CompareOrdinal(_oValue.ToString(), other._oValue.ToString());
                    default:
                        throw new NotImplementedException("Try to compare two values of " + _valueType);
                }
            }

            throw new InvalidOperationException("Type mismatch");
        }

        #endregion

        public static JSValue Marshal(object value)
        {
            return Context.CurrentGlobalContext.ProxyValue(value);
        }

        public static JSValue Wrap(object value)
        {
            if (value == null)
                return Null;

            return new ObjectWrapper(value);
        }

        public static JSValue GetConstructor(Type type)
        {
            return Context.CurrentGlobalContext.GetConstructor(type);
        }

        public static Function GetGenericTypeSelector(IList<Type> types)
        {
            return Context.CurrentGlobalContext.GetGenericTypeSelector(types);
        }
    }
}
