//#define CALLSTACKTOSTRING

using System;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Interop;

namespace Maddalena.Core.Javascript.BaseLibrary
{
    [Serializable]
    public class Error
    {
        [DoNotEnumerate]
        public JSValue message
        {
            [Hidden]
            get;
        }
        [DoNotEnumerate]
        public JSValue name
        {
            [Hidden]
            get;
            set;
        }

        [DoNotEnumerate]
        public Error()
        {
            name = GetType().Name;
            message = "";
        }

        [DoNotEnumerate]
        public Error(Arguments args)
        {
            name = GetType().Name;
            message = args[0].ToString();
        }

        [DoNotEnumerate]
        public Error(string message)
        {
            name = GetType().Name;
            this.message = message;
        }

        [Hidden]
        public override string ToString()
        {
            string mstring;
            string nstring;
            if (message == null
                || message._valueType <= JSValueType.Undefined
                || string.IsNullOrEmpty((mstring = message.ToString())))
                return name.ToString();
            if (name == null
                || name._valueType <= JSValueType.Undefined
                || string.IsNullOrEmpty((nstring = name.ToString())))
                return mstring;
            return nstring + ": " + mstring;
        }

        [DoNotEnumerate]
       
        public JSValue toString()
        {
            return ToString();
        }
    }
}
