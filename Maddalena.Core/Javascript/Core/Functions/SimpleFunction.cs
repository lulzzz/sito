﻿using System;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core.Interop;
using Maddalena.Core.Javascript.Expressions;

namespace Maddalena.Core.Javascript.Core.Functions
{
    [Serializable]
    [Prototype(typeof(Function), true)]
    internal sealed class SimpleFunction : Function
    {
        internal SimpleFunction(Context context, FunctionDefinition creator)
            : base(context, creator)
        {
        }

        internal override JSValue InternalInvoke(JSValue targetObject, Expression[] arguments, Context initiator, bool withSpread, bool construct)
        {
            if (construct || withSpread)
                return base.InternalInvoke(targetObject, arguments, initiator, withSpread, construct);

            notExists._valueType = JSValueType.NotExists;

            if (_functionDefinition.Parameters.Length == arguments.Length // из-за необходимости иметь возможность построить аргументы, если они потребуются
                && arguments.Length < 9)
            {
                return fastInvoke(targetObject, arguments, initiator);
            }

            return base.InternalInvoke(targetObject, arguments, initiator, false, false);
        }

        private JSValue fastInvoke(JSValue targetObject, Expression[] arguments, Context initiator)
        {
            var body = _functionDefinition.Body;
            targetObject = correctTargetObject(targetObject, body._strict);
            if (_functionDefinition.recursionDepth > _functionDefinition.parametersStored) // рекурсивный вызов.
            {
                storeParameters();
                _functionDefinition.parametersStored++;
            }

            JSValue res = null;
            Arguments args = null;
            bool tailCall = false;
            for (;;)
            {
                var internalContext = new Context(_initialContext, false, this)
                {
                    _thisBind = _functionDefinition.Kind == FunctionKind.Arrow
                        ? _initialContext._thisBind
                        : targetObject
                };


                if (tailCall)
                    initParameters(args, internalContext);
                else
                    initParametersFast(arguments, initiator, internalContext);

                // Эта строка обязательно должна находиться после инициализации параметров
                _functionDefinition.recursionDepth++;

                if (_functionDefinition.Reference._descriptor != null && _functionDefinition.Reference._descriptor.cacheRes == null)
                {
                    _functionDefinition.Reference._descriptor.cacheContext = internalContext._parent;
                    _functionDefinition.Reference._descriptor.cacheRes = this;
                }

                internalContext._strict |= body._strict;
                internalContext.Activate();

                try
                {
                    res = evaluateBody(internalContext);
                    if (internalContext._executionMode == ExecutionMode.TailRecursion)
                    {
                        tailCall = true;
                        args = internalContext._executionInfo as Arguments;
                    }
                    else
                        tailCall = false;
                }
                finally
                {
                    _functionDefinition.recursionDepth--;
                    if (_functionDefinition.parametersStored > _functionDefinition.recursionDepth)
                        _functionDefinition.parametersStored--;
                    exit(internalContext);
                }

                if (!tailCall)
                    break;

                targetObject = correctTargetObject(internalContext._objectSource, body._strict);
            }
            return res;
        }

        private void initParametersFast(Expression[] arguments, Context initiator, Context internalContext)
        {
            JSValue a0 = null,
                    a1 = null,
                    a2 = null,
                    a3 = null,
                    a4 = null,
                    a5 = null,
                    a6 = null,
                    a7 = null; // Вместо кучи, выделяем память на стеке

            var argumentsCount = arguments.Length;
            if (_functionDefinition.Parameters.Length != argumentsCount)
                throw new ArgumentException("Invalid arguments count");
            if (argumentsCount > 8)
                throw new ArgumentException("To many arguments");
            if (argumentsCount == 0)
                return;

            /*
             * Да, от этого кода можно вздрогнуть, но по ряду причин лучше сделать не получится.
             * Такая она цена оптимизации
             */

            /*
             * Эти два блока нельзя смешивать. Текущие значения параметров могут быть использованы для расчёта новых. 
             * Поэтому заменять значения можно только после полного расчёта новых значений
             */

            a0 = Tools.EvalExpressionSafe(initiator, arguments[0]);
            if (argumentsCount > 1)
            {
                a1 = Tools.EvalExpressionSafe(initiator, arguments[1]);
                if (argumentsCount > 2)
                {
                    a2 = Tools.EvalExpressionSafe(initiator, arguments[2]);
                    if (argumentsCount > 3)
                    {
                        a3 = Tools.EvalExpressionSafe(initiator, arguments[3]);
                        if (argumentsCount > 4)
                        {
                            a4 = Tools.EvalExpressionSafe(initiator, arguments[4]);
                            if (argumentsCount > 5)
                            {
                                a5 = Tools.EvalExpressionSafe(initiator, arguments[5]);
                                if (argumentsCount > 6)
                                {
                                    a6 = Tools.EvalExpressionSafe(initiator, arguments[6]);
                                    if (argumentsCount > 7)
                                    {
                                        a7 = Tools.EvalExpressionSafe(initiator, arguments[7]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            setParamValue(0, a0, internalContext);
            if (argumentsCount > 1)
            {
                setParamValue(1, a1, internalContext);
                if (argumentsCount > 2)
                {
                    setParamValue(2, a2, internalContext);
                    if (argumentsCount > 3)
                    {
                        setParamValue(3, a3, internalContext);
                        if (argumentsCount > 4)
                        {
                            setParamValue(4, a4, internalContext);
                            if (argumentsCount > 5)
                            {
                                setParamValue(5, a5, internalContext);
                                if (argumentsCount > 6)
                                {
                                    setParamValue(6, a6, internalContext);
                                    if (argumentsCount > 7)
                                    {
                                        setParamValue(7, a7, internalContext);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void setParamValue(int index, JSValue value, Context context)
        {
            if (_functionDefinition.Parameters[index].assignments != null)
            {
                value = value.CloneImpl(false);
                value._attributes |= JSValueAttributesInternal.Argument;
            }
            else
                value._attributes &= ~JSValueAttributesInternal.Cloned;
            if (!value.Defined && _functionDefinition.Parameters.Length > index && _functionDefinition.Parameters[index].initializer != null)
                value.Assign(_functionDefinition.Parameters[index].initializer.Evaluate(context));
            _functionDefinition.Parameters[index].cacheRes = value;
            _functionDefinition.Parameters[index].cacheContext = context;
            if (_functionDefinition.Parameters[index].captured)
            {
                if (context._variables == null)
                    context._variables = getFieldsContainer();
                context._variables[_functionDefinition.Parameters[index].name] = value;
            }
        }
    }
}
