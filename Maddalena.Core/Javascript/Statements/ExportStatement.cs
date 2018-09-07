﻿using System;
using System.Collections.Generic;
using System.Text;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Expressions;

namespace Maddalena.Core.Javascript.Statements
{
    public sealed class ExportStatement : CodeNode
    {
        private CodeNode _internalDefinition;
        private readonly List<KeyValuePair<string, Expression>> _map = new List<KeyValuePair<string, Expression>>();

        public string ReExportSourceModuleName { get; private set; }

        public CodeNode InternalDefinition => _internalDefinition;
        public IList<KeyValuePair<string, Expression>> ExportMap => _map.AsReadOnly();

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            if (!Parser.Validate(state.Code, "export", ref index))
                return null;

            Tools.SkipSpaces(state.Code, ref index);

            var result = new ExportStatement();
            var reexport = 0;

            if (Parser.Validate(state.Code, "*", ref index))
            {
                reexport = 1;
            }
            else if (Parser.Validate(state.Code, "default", ref index))
            {
                reexport = -1;
                Tools.SkipSpaces(state.Code, ref index);
                result._map.Add(new KeyValuePair<string, Expression>("", (Expression)ExpressionTree.Parse(state, ref index)));
            }
            else if (state.Code[index] == '{')
            {
                parseExportMap(result, state, ref index);
            }
            else
            {
                reexport = -1;
                var definition =
                    VariableDefinition.Parse(state, ref index)
                    ?? ClassDefinition.Parse(state, ref index)
                    ?? FunctionDefinition.Parse(state, ref index, FunctionKind.Function);

                if (definition == null)
                    ExceptionHelper.ThrowSyntaxError(Messages.UnexpectedToken, state.Code, index);

                result._internalDefinition = definition;
            }

            Tools.SkipSpaces(state.Code, ref index);

            if (Parser.Validate(state.Code, "from", ref index))
            {
                if (reexport == -1)
                    ExceptionHelper.ThrowSyntaxError("Reexport is not allowed with this syntax", state.Code, index - 4);

                Tools.SkipSpaces(state.Code, ref index);

                var start = index;
                if (!Parser.ValidateString(state.Code, ref index, false))
                    ExceptionHelper.ThrowSyntaxError("Expected module name", state.Code, index);

                result.ReExportSourceModuleName = Tools.Unescape(state.Code.Substring(start + 1, index - start - 2), false);
            }
            else if (reexport == 1)
                ExceptionHelper.ThrowSyntaxError("Expected 'from'", state.Code, index);

            return result;
        }

        private static void parseExportMap(ExportStatement export, ParseInfo state, ref int index)
        {
            index++;
            Tools.SkipSpaces(state.Code, ref index);

            if (state.Code[index] == '}')
                ExceptionHelper.ThrowSyntaxError("Empty export map", state.Code, index);

            while (state.Code[index] != '}')
            {
                var start = index;
                if (!Parser.ValidateName(state.Code, ref index))
                    ExceptionHelper.ThrowSyntaxError("Invalid export name", state.Code, index);
                var name = state.Code.Substring(start, index - start);
                var alias = name;

                Tools.SkipSpaces(state.Code, ref index);

                alias = parseAlias(state.Code, ref index) ?? name;

                for (var i = 0; i < export._map.Count; i++)
                {
                    if (export._map[i].Key == name)
                        ExceptionHelper.ThrowSyntaxError("Duplicate import", state.Code, index);
                }

                export._map.Add(
                    new KeyValuePair<string, Expression>(
                        alias,
                        new Variable(name, state.lexicalScopeLevel)
                        {
                            Position = start,
                            Length = name.Length
                        }));

                if (Parser.Validate(state.Code, ",", ref index))
                    Tools.SkipSpaces(state.Code, ref index);
            }

            index++;
        }

        private static string parseAlias(string code, ref int index)
        {
            string alias = null;
            if (Parser.Validate(code, "as", ref index))
            {
                Tools.SkipSpaces(code, ref index);

                var start = index;
                if (!Parser.ValidateName(code, ref index) && !Parser.Validate(code, "default", ref index))
                    ExceptionHelper.ThrowSyntaxError("Invalid export alias", code, index);

                alias = code.Substring(start, index - start);

                if (alias == "default")
                    alias = "";

                Tools.SkipSpaces(code, ref index);
            }

            return alias;
        }

        public override void Decompose(ref CodeNode self)
        {
            throw new InvalidOperationException();
        }

        public override JSValue Evaluate(Context context)
        {
            if (context._module == null)
                ExceptionHelper.Throw(new Error("Module undefined"));

            if (ReExportSourceModuleName != null)
            {
                if (string.IsNullOrEmpty(context._module.FilePath))
                    ExceptionHelper.Throw(new Error("Module must has name"));

                var module = context._module.Import(ReExportSourceModuleName);

                if (_map.Count == 0)
                {
                    foreach (var item in module.Exports)
                    {
                        context._module.Exports[item.Key] = item.Value;
                    }
                }
                else
                {
                    for (var i = 0; i < _map.Count; i++)
                    {
                        context._module.Exports[_map[i].Key] = module.Exports[_map[i].Value.ToString()];
                    }
                }
            }
            else if (_internalDefinition != null)
            {
                _internalDefinition.Evaluate(context);

                var variableDef = _internalDefinition as VariableDefinition;
                if (variableDef != null)
                {
                    for (var i = 0; i < variableDef._variables.Length; i++)
                    {
                        context._module.Exports[variableDef._variables[i].name] = variableDef._variables[i].references[0].Evaluate(context);
                    }
                }
                else
                {
                    var entityDef = _internalDefinition as EntityDefinition;

                    context._module.Exports[entityDef.Name] = entityDef.Reference.Descriptor.Get(context, false, 1);
                }
            }
            else
            {
                for (var i = 0; i < _map.Count; i++)
                {
                    context._module.Exports[_map[i].Key] = _map[i].Value.Evaluate(context);
                }
            }

            return null;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (ReExportSourceModuleName != null)
                return false;

            if (_internalDefinition != null)
            {
                Parser.Build(ref _internalDefinition, expressionDepth, variables, codeContext, message, stats, opts | Options.SuppressUselessStatementsElimination);
            }
            else
            {
                for (var i = 0; i < _map.Count; i++)
                {
                    var v = _map[i].Value;
                    Parser.Build(ref v, expressionDepth + 1, variables, codeContext, message, stats, opts);
                    if (v != _map[i].Value)
                        _map[i] = new KeyValuePair<string, Expression>(_map[i].Key, v);
                }
            }

            return false;
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            if (ReExportSourceModuleName != null)
                return;

            if (_internalDefinition != null)
            {
                _internalDefinition.RebuildScope(functionInfo, transferedVariables, scopeBias);
            }
            else
            {
                for (var i = 0; i < _map.Count; i++)
                {
                    _map[i].Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
                }
            }
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            if (ReExportSourceModuleName != null)
                return;

            if (_internalDefinition != null)
            {
                CodeNode t = _internalDefinition;
                _internalDefinition.Optimize(ref t, owner, message, opts, stats);
                if (t != _internalDefinition)
                    _internalDefinition = t as VariableDefinition;
            }
            else
            {
                for (var i = 0; i < _map.Count; i++)
                {
                    var v = _map[i].Value;
                    _map[i].Value.Optimize(ref v, owner, message, opts, stats);
                    if (v != _map[i].Value)
                        _map[i] = new KeyValuePair<string, Expression>(_map[i].Key, v);
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder("export ");

            if (_map.Count == 1 && _map[0].Key == "")
            {
                result.Append(" default ");
                result.Append(_map[0].Value);
            }

            var i = 0;
            if (i < _map.Count)
            {
                result.Append("{ ");

                for (;;)
                {
                    var item = _map[i];

                    result.Append(item.Key);

                    if (item.Key != item.Value.ToString())
                    {
                        result
                            .Append(" as ")
                            .Append(string.IsNullOrEmpty(item.Value.ToString()) ? "default" : item.Value.ToString());
                    }

                    i++;

                    if (i < _map.Count)
                        result.Append(", ");
                    else
                        break;
                }

                result.Append(" }");
            }
            else
            {
                if (ReExportSourceModuleName != null)
                    result.Append(" * ");
                else
                    result.Append(_internalDefinition);
            }

            if (ReExportSourceModuleName != null)
            {
                result
                    .Append(" from \"")
                    .Append(ReExportSourceModuleName)
                    .Append("\"");
            }

            return result.ToString();
        }
    }
}
