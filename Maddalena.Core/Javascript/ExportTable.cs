using System;
using System.Collections;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript
{
    [Serializable]
    public sealed class ExportTable : IEnumerable<KeyValuePair<string, JSValue>>
    {
        private readonly Dictionary<string, JSValue> _items = new Dictionary<string, JSValue>();

        public JSValue this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key) || !Parser.ValidateName(key))
                    ExceptionHelper.Throw(new ArgumentException());

                return !_items.TryGetValue(key, out var result) ? JSValue.undefined : result;
            }
            internal set => _items[key] = value;
        }

        public int Count => _items.Count;

        public JSValue Default
        {
            get
            {
                var result = JSValue.undefined;

                if (!_items.TryGetValue("", out result))
                    return JSValue.undefined;

                return result;
            }
        }

        public JSObject CreateExportList()
        {
            var result = JSObject.CreateObject(true);

            foreach(var item in _items)
            {
                if (item.Key != "")
                    result._fields[item.Key] = item.Value;
                else
                    result._fields["default"] = item.Value;
            }

            return result;
        }

        public IEnumerator<KeyValuePair<string, JSValue>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
