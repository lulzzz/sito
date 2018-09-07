using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Math = System.Math;

namespace Maddalena.Core.Javascript
{
    internal static class ExceptionHelper
    {
        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void Throw(Error error)
        {
            throw new JSException(error);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void Throw(Error error, CodeNode exceptionMaker, string code)
        {
            throw new JSException(error, exceptionMaker, code);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void Throw(JSValue error)
        {
            throw new JSException(error ?? JSValue.undefined);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void Throw(Error error, Exception innerException)
        {
            throw new JSException(error, innerException);
        }

        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowArgumentNull(string message)
        {
            throw new ArgumentNullException(message);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowVariableIsNotDefined(string variableName, string code, int position, int length, CodeNode exceptionMaker)
        {
            Throw(new ReferenceError(string.Format(Messages.VariableNotDefined, variableName)), exceptionMaker, code);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowVariableIsNotDefined(string variableName, CodeNode exceptionMaker)
        {
            Throw(new ReferenceError(string.Format(Messages.VariableNotDefined, variableName)), exceptionMaker, null);
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowIncrementPropertyWOSetter(object proprtyName)
        {
            Throw(new TypeError(string.Format(Messages.IncrementPropertyWOSetter, proprtyName)));
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowIncrementReadonly(object entityName)
        {
            Throw(new TypeError(string.Format(Messages.IncrementReadonly, entityName)));
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowUnknownToken(string code, int index)
        {
            var cord = CodeCoordinates.FromTextPosition(code, index, 0);
            Throw(new SyntaxError(string.Format(
                Messages.UnknowIdentifier,
                code.Substring(index, Math.Min(50, code.Length - index)).Split(Tools.TrimChars).FirstOrDefault(),
                cord)));
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowSyntaxError(string message)
        {
            Throw(new SyntaxError(message));
        }

        /// <exception cref="JSException">
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void ThrowSyntaxError(string message, string code, int position)
        {
            ThrowSyntaxError(message, code, position, 0);
        }

        /// <exception cref="JSException">
        /// </exception>
        [DebuggerStepThrough]
        internal static void ThrowSyntaxError(string message, string code, int position, int length)
        {
            var cord = CodeCoordinates.FromTextPosition(code, position, 0);
            Throw(new SyntaxError(message + " " + cord));
        }

        /// <exception cref="JSException">
        /// </exception>
#if INLINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        internal static T ThrowIfNotExists<T>(T obj, object name) where T : JSValue
        {
            if (obj._valueType == JSValueType.NotExists)
                Throw((new ReferenceError("Variable \"" + name + "\" is not defined.")));
            return obj;
        }

        /// <exception cref="JSException">
        /// </exception>
        [DebuggerStepThrough]
        internal static void ThrowReferenceError(string message, string code, int position, int length)
        {
            var cord = CodeCoordinates.FromTextPosition(code, position, 0);
            Throw(new ReferenceError(message + " " + cord));
        }

        /// <exception cref="JSException">
        /// </exception>
        [DebuggerStepThrough]
        internal static void ThrowReferenceError(string message)
        {
            Throw(new ReferenceError(message));
        }

        /// <exception cref="JSException">
        /// </exception>
        [DebuggerStepThrough]
        internal static void ThrowTypeError(string message)
        {
            Throw(new TypeError(message));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        internal static void Throw(Exception exception)
        {
            throw exception;
        }
    }
}
