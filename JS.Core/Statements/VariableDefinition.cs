﻿using System;
using System.Collections.Generic;
using JS.Core;
using JS.Core.Core;
using JS.Core.Expressions;
using NiL.JS.BaseLibrary;

namespace NiL.JS.Statements
{
#if !(PORTABLE)
    [Serializable]
#endif
    public enum VariableKind
    {
        AutoGeneratedParameters,
        FunctionScope,
        LexicalScope,
        ConstantInLexicalScope
    }

#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class VariableDefinition : CodeNode
    {
        internal readonly VariableDescriptor[] _variables;
        internal Expression[] _initializers;

        public CodeNode[] Initializers => _initializers;
        public VariableDescriptor[] Variables => _variables;
        public VariableKind Kind { get; }

        internal VariableDefinition(VariableDescriptor[] variables, Expression[] initializers, VariableKind kind)
        {
            _initializers = initializers;
            _variables = variables;
            Kind = kind;
        }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            return Parse(state, ref index, false);
        }

        internal static CodeNode Parse(ParseInfo state, ref int index, bool forForLoop)
        {
            int position = index;
            Tools.SkipSpaces(state.Code, ref position);

            var mode = VariableKind.FunctionScope;
            if (Parser.Validate(state.Code, "var ", ref position))
                mode = VariableKind.FunctionScope;
            else if (Parser.Validate(state.Code, "let ", ref position))
                mode = VariableKind.LexicalScope;
            else if (Parser.Validate(state.Code, "const ", ref position))
                mode = VariableKind.ConstantInLexicalScope;
            else
                return null;

            var level = mode <= VariableKind.FunctionScope ? state.functionScopeLevel : state.lexicalScopeLevel;
            var initializers = new List<Expression>();
            var names = new List<string>();
            int s = position;
            while ((state.Code[position] != ';') && (state.Code[position] != '}') && !Tools.IsLineTerminator(state.Code[position]))
            {
                Tools.SkipSpaces(state.Code, ref position);

                if (state.Code[position] != '[' && state.Code[position] != '{' && !Parser.ValidateName(state.Code, position, state.strict))
                {
                    if (Parser.ValidateName(state.Code, ref position, false, true, state.strict))
                        ExceptionHelper.ThrowSyntaxError('\"' + Tools.Unescape(state.Code.Substring(s, position - s), state.strict) + "\" is a reserved word, but used as a variable. " + CodeCoordinates.FromTextPosition(state.Code, s, position - s));
                    ExceptionHelper.ThrowSyntaxError("Invalid variable definition at " + CodeCoordinates.FromTextPosition(state.Code, s, position - s));
                }

                var expression = ExpressionTree.Parse(state, ref position, processComma: false, forForLoop: forForLoop);
                if (expression is VariableReference)
                {
                    var name = expression.ToString();
                    if (state.strict)
                    {
                        if (name == "arguments" || name == "eval")
                            ExceptionHelper.ThrowSyntaxError("Varible name cannot be \"arguments\" or \"eval\" in strict mode", state.Code, s, position - s);
                    }

                    names.Add(name);
                    initializers.Add(expression);
                }
                else
                {
                    bool valid = false;
                    var expr = expression as ExpressionTree;
                    if (expr != null)
                    {
                        if (expr.Type == OperationType.None && expr._right == null)
                            expr = expr._left as ExpressionTree;
                        valid |= expr != null && expr.Type == OperationType.Assignment;

                        if (valid)
                        {
                            if (expr._left is ObjectDesctructor)
                            {
                                var expressions = (expr._left as ObjectDesctructor).GetTargetVariables();
                                for (var i = 0; i < expressions.Count; i++)
                                {
                                    names.Add(expressions[i].ToString());
                                    initializers.Add(expressions[i]);
                                }

                                initializers.Add(expr);
                            }
                            else
                            {
                                names.Add(expr._left.ToString());
                                initializers.Add(expression);
                            }
                        }
                    }
                    else
                    {
                        var cnst = expression as Constant;
                        valid = cnst != null && cnst.value == JSValue.undefined;
                        if (valid)
                        {
                            initializers.Add(expression);
                            names.Add(cnst.value.ToString());
                        }
                    }

                    if (!valid)
                        ExceptionHelper.ThrowSyntaxError("Invalid variable initializer", state.Code, position);
                }

                s = position;

                if (position >= state.Code.Length)
                    break;

                Tools.SkipSpaces(state.Code, ref s);
                if (s >= state.Code.Length)
                    break;
                if (state.Code[s] == ',')
                {
                    position = s + 1;
                    Tools.SkipSpaces(state.Code, ref position);
                }
                else
                    break;
            }

            if (names.Count == 0)
                throw new InvalidOperationException("code (" + position + ")");

            if (!forForLoop && position < state.Code.Length && state.Code[position] == ';')
                position++;
            else
                position = s;

            var variables = new VariableDescriptor[names.Count];
            for (int i = 0, skiped = 0; i < names.Count; i++)
            {
                bool skip = false;
                for (var j = 0; j < state.Variables.Count - i + skiped; j++)
                {
                    if (state.Variables[j].name == names[i] && state.Variables[j].definitionScopeLevel >= level)
                    {
                        if (state.Variables[j].lexicalScope && mode > VariableKind.FunctionScope)
                            ExceptionHelper.ThrowSyntaxError(string.Format(Messages.IdentifierAlreadyDeclared, names[i]), state.Code, index);

                        skip = true;
                        variables[i] = state.Variables[j];
                        skiped++;
                        break;
                    }
                }

                if (skip)
                    continue;

                variables[i] = new VariableDescriptor(names[i], level)
                {
                    lexicalScope = mode > VariableKind.FunctionScope,
                    isReadOnly = mode == VariableKind.ConstantInLexicalScope
                };

                state.Variables.Add(variables[i]);
            }

            if (initializers.Count != variables.Length)
                throw new InvalidOperationException("initializers.Count != variables.Length");

            var pos = index;
            index = position;
            return new VariableDefinition(variables, initializers.ToArray(), mode)
            {
                Position = pos,
                Length = index - pos
            };
        }

        public override JSValue Evaluate(Context context)
        {
            int i = 0;
            if (context._executionMode >= ExecutionMode.Resume)
            {
                i = (int)context.SuspendData[this];
            }

            for (; i < _initializers.Length; i++)
            {
                if (context._executionMode == ExecutionMode.None && Kind > VariableKind.FunctionScope && _variables[i].lexicalScope)
                {
                    JSValue f = context.DefineVariable(_variables[i].name, false);

                    _variables[i].cacheRes = f;
                    _variables[i].cacheContext = context;

                    if (Kind == VariableKind.ConstantInLexicalScope)
                        f._attributes |= JSValueAttributesInternal.ReadOnly;
                }

                _initializers[i].Evaluate(context);

                if (context._executionMode == ExecutionMode.Suspend)
                {
                    context.SuspendData[this] = i;
                    return null;
                }
            }
            return JSValue.notExists;
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            var res = new List<CodeNode>();
            res.AddRange(_initializers);
            res.RemoveAll(x => x == null);
            return res.ToArray();
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (Kind > VariableKind.FunctionScope)
                stats.WithLexicalEnvironment = true;

            int actualChilds = 0;
            for (int i = 0; i < _initializers.Length; i++)
            {
                Parser.Build(ref _initializers[i], message != null ? 2 : expressionDepth, variables, codeContext, message, stats, opts);
                if (_initializers[i] != null)
                {
                    actualChilds++;

                    if (Kind == VariableKind.ConstantInLexicalScope && _initializers[i] is Assignment)
                    {
                        _initializers[i] = new ForceAssignmentOperator(_initializers[i]._left, _initializers[i]._right)
                        {
                            Position = _initializers[i].Position,
                            Length = _initializers[i].Length
                        };
                    }
                }
            }

            if (actualChilds < _initializers.Length)
            {
                if ((opts & Options.SuppressUselessStatementsElimination) == 0 && actualChilds == 0)
                {
                    _this = null;
                    Eliminated = true;
                    return false;
                }

                var newinits = new Expression[actualChilds];
                for (int i = 0, j = 0; i < _initializers.Length; i++)
                    if (_initializers[i] != null)
                        newinits[j++] = _initializers[i];
                _initializers = newinits;
            }

            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            for (int i = 0; i < _initializers.Length; i++)
            {
                _initializers[i].Optimize(ref _initializers[i], owner, message, opts, stats);
            }
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            if (Kind == VariableKind.AutoGeneratedParameters)
                return "";

            var res = "";
            if (Kind == VariableKind.ConstantInLexicalScope)
                res = "const ";
            else if (Kind == VariableKind.LexicalScope)
                res = "let ";
            else
                res = "var ";

            for (var i = 0; i < _initializers.Length; i++)
            {
                var t = _initializers[i].ToString();
                if (string.IsNullOrEmpty(t))
                    continue;
                if (t[0] == '(')
                    t = t.Substring(1, t.Length - 2);
                if (i > 0)
                    res += ", ";
                res += t;
            }

            return res;
        }

        public override void Decompose(ref CodeNode self)
        {
            for (var i = 0; i < _initializers.Length; i++)
            {
                _initializers[i].Decompose(ref _initializers[i]);
            }
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            for (var i = 0; i < _initializers.Length; i++)
            {
                _initializers[i].RebuildScope(functionInfo, transferedVariables, scopeBias);
            }
        }
    }
}