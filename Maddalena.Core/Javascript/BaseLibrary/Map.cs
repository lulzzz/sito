﻿using System;
using System.Collections.Generic;
using System.Linq;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Extensions;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [RequireNewKeyword,Serializable]
    public sealed class Map : IIterable
    {
        private readonly Dictionary<object, object> _storage;

        public Map()
        {
            _storage = new Dictionary<object, object>();
        }

        public Map(IIterable iterable)
            : this()
        {
            if (iterable == null)
                return;

            foreach (var item in iterable.AsEnumerable())
            {
                if (item._valueType < JSValueType.Object)
                    ExceptionHelper.ThrowTypeError($"Iterator value {item} is not an entry object");

                var value = item["1"];
                _storage[item["0"].Value] = value.Value as JSValue ?? value;
            }
        }

        public object get(object key)
        {
            if (key == null)
                key = JSValue.@null;
            else
                key = (key as JSValue)?.Value ?? key;

            object result = null;
            _storage.TryGetValue(key, out result);
            return result;
        }

        public Map set(object key, object value)
        {
            if (key == null)
                key = JSValue.@null;
            else
                key = (key as JSValue)?.Value ?? key;

            _storage[key] = value;

            return this;
        }

        public void clear()
        {
            _storage.Clear();
        }

        public bool delete(object key)
        {
            if (key == null)
                key = JSValue.@null;
            else
                key = (key as JSValue)?.Value ?? key;

            return _storage.Remove(key);
        }

        public bool has(object key)
        {
            if (key == null)
                key = JSValue.@null;
            else
                key = (key as JSValue)?.Value ?? key;

            return _storage.ContainsKey(key);
        }

        public void forEach(Function callback, JSValue thisArg)
        {
            foreach (var item in _storage)
            {
                callback.Call(thisArg, new Arguments { item.Key, item.Value, this });
            }
        }

        public IIterator keys()
        {
            return _storage.Keys.AsIterable().iterator();
        }

        public IIterator values()
        {
            return _storage.Keys.AsIterable().iterator();
        }

        public IIterator iterator()
        {
            return _storage
                .Select(x => new Array { JSValue.Marshal(x.Key), JSValue.Marshal(x.Value) })
                .GetEnumerator()
                .AsIterator();
        }
    }
}
