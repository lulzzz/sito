using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Core.Functions;
using Maddalena.Core.Javascript.Statements;
using Array = System.Array;
using Math = System.Math;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ParameterDescriptor : VariableDescriptor
    {
        public ObjectDesctructor Destructor { get; internal set; }
        public bool IsRest { get; private set; }

        internal ParameterDescriptor(string name, bool rest, int depth)
            : base(name, depth)
        {
            IsRest = rest;
            lexicalScope = true;
        }

        public override string ToString()
        {
            if (IsRest)
                return "..." + name;
            return name;
        }
    }

#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ParameterReference : VariableReference
    {
        public override string Name => _descriptor.name;

        internal ParameterReference(string name, bool rest, int depth)
        {
            _descriptor = new ParameterDescriptor(name, rest, depth);
            _descriptor.references.Add(this);
        }

        public override JSValue Evaluate(Context context)
        {
            if (_descriptor.cacheContext != context)
                throw new InvalidOperationException();

            return _descriptor.cacheRes;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return Descriptor.ToString();
        }
    }

    [Serializable]
    public sealed class FunctionDefinition : EntityDefinition
    {
        #region Runtime
        internal int parametersStored;
        internal int recursionDepth;
        #endregion

        internal readonly FunctionInfo FunctionInfo;

        public ParameterDescriptor[] Parameters { get; internal set; }

        public CodeBlock Body { get; internal set; }

        protected internal override bool NeedDecompose => FunctionInfo.NeedDecompose;

        protected internal override bool ContextIndependent => false;

        internal override bool ResultInTempContainer => false;

        protected internal override PredictedType ResultType => PredictedType.Function;

        public override bool Hoist => Kind != FunctionKind.Arrow
                                      && Kind != FunctionKind.AsyncArrow;

        public FunctionKind Kind { get; internal set; }

        public bool Strict
        {
            get => Body?.Strict ?? false;
            internal set
            {
                if (Body != null)
                    Body._strict = value;
            }
        }

        private FunctionDefinition(string name)
            : base(name)
        {
            FunctionInfo = new FunctionInfo();
        }

        internal FunctionDefinition()
            : this("anonymous")
        {
            Parameters = new ParameterDescriptor[0];
            Body = new CodeBlock(new CodeNode[0])
            {
                _strict = true,
                _variables = new VariableDescriptor[0]
            };
        }

        internal static ParseDelegate ParseFunction(FunctionKind kind)
        {
            return (ParseInfo info, ref int index) => Parse(info, ref index, kind);
        }

        internal static CodeNode ParseFunction(ParseInfo state, ref int index)
        {
            return Parse(state, ref index, FunctionKind.Function);
        }

        internal static Expression Parse(ParseInfo state, ref int index, FunctionKind kind)
        {
            string code = state.Code;
            int position = index;
            switch (kind)
            {
                case FunctionKind.AnonymousFunction:
                case FunctionKind.AnonymousGenerator:
                    {
                        break;
                    }
                case FunctionKind.Function:
                    {
                        if (!Parser.Validate(code, "function", ref position))
                            return null;

                        if (code[position] == '*')
                        {
                            kind = FunctionKind.Generator;
                            position++;
                        }
                        else if ((code[position] != '(') && (!Tools.IsWhiteSpace(code[position])))
                            return null;

                        break;
                    }
                case FunctionKind.Getter:
                    {
                        if (!Parser.Validate(code, "get ", ref position))
                            return null;

                        break;
                    }
                case FunctionKind.Setter:
                    {
                        if (!Parser.Validate(code, "set ", ref position))
                            return null;

                        break;
                    }
                case FunctionKind.MethodGenerator:
                case FunctionKind.Method:
                    {
                        if (code[position] == '*')
                        {
                            kind = FunctionKind.MethodGenerator;
                            position++;
                        }
                        else if (kind == FunctionKind.MethodGenerator)
                            throw new ArgumentException("mode");

                        break;
                    }
                case FunctionKind.Arrow:
                    {
                        break;
                    }
                case FunctionKind.AsyncFunction:
                    {
                        if (!Parser.Validate(code, "async", ref position))
                            return null;

                        Tools.SkipSpaces(code, ref position);

                        if (!Parser.Validate(code, "function", ref position))
                            return null;

                        break;
                    }
                default:
                    throw new NotImplementedException(kind.ToString());
            }

            Tools.SkipSpaces(state.Code, ref position);

            var parameters = new List<ParameterDescriptor>();
            CodeBlock body = null;
            string name = null;
            bool arrowWithSunglePrm = false;
            int nameStartPos = 0;
            bool containsDestructuringPrms = false;

            if (kind != FunctionKind.Arrow)
            {
                if (code[position] != '(')
                {
                    nameStartPos = position;
                    if (Parser.ValidateName(code, ref position, false, true, state.Strict))
                        name = Tools.Unescape(code.Substring(nameStartPos, position - nameStartPos), state.Strict);
                    else if ((kind == FunctionKind.Getter || kind == FunctionKind.Setter) && Parser.ValidateString(code, ref position, false))
                        name = Tools.Unescape(code.Substring(nameStartPos + 1, position - nameStartPos - 2), state.Strict);
                    else if ((kind == FunctionKind.Getter || kind == FunctionKind.Setter) && Parser.ValidateNumber(code, ref position))
                        name = Tools.Unescape(code.Substring(nameStartPos, position - nameStartPos), state.Strict);
                    else
                        ExceptionHelper.ThrowSyntaxError("Invalid function name", code, nameStartPos, position - nameStartPos);

                    Tools.SkipSpaces(code, ref position);

                    if (code[position] != '(')
                        ExceptionHelper.ThrowUnknownToken(code, position);
                }
                else if (kind == FunctionKind.Getter || kind == FunctionKind.Setter)
                    ExceptionHelper.ThrowSyntaxError("Getter and Setter must have name", code, index);
                else if (kind == FunctionKind.Method || kind == FunctionKind.MethodGenerator)
                    ExceptionHelper.ThrowSyntaxError("Method must have name", code, index);

                position++;
            }
            else if (code[position] != '(')
            {
                arrowWithSunglePrm = true;
            }
            else
            {
                position++;
            }

            Tools.SkipSpaces(code, ref position);

            if (code[position] == ',')
                ExceptionHelper.ThrowSyntaxError(Messages.UnexpectedToken, code, position);

            while (code[position] != ')')
            {
                if (parameters.Count == 255 || (kind == FunctionKind.Setter && parameters.Count == 1) || kind == FunctionKind.Getter)
                    ExceptionHelper.ThrowSyntaxError(string.Format(Messages.TooManyArgumentsForFunction, name), code, index);

                bool rest = Parser.Validate(code, "...", ref position);

                Expression destructor = null;
                int n = position;
                if (!Parser.ValidateName(code, ref position, state.Strict))
                {
                    if (code[position] == '{')
                        destructor = (Expression)ObjectDefinition.Parse(state, ref position);
                    else if (code[position] == '[')
                        destructor = (Expression)ArrayDefinition.Parse(state, ref position);

                    if (destructor == null)
                        ExceptionHelper.ThrowUnknownToken(code, nameStartPos);

                    containsDestructuringPrms = true;
                }

                var pname = Tools.Unescape(code.Substring(n, position - n), state.Strict);
                var reference = new ParameterReference(pname, rest, state.LexicalScopeLevel + 1)
                                    {
                                        Position = n,
                                        Length = position - n
                                    };
                var desc = reference.Descriptor as ParameterDescriptor;

                if (destructor != null)
                    desc.Destructor = new ObjectDesctructor(destructor);

                parameters.Add(desc);

                Tools.SkipSpaces(state.Code, ref position);
                if (arrowWithSunglePrm)
                {
                    position--;
                    break;
                }

                if (code[position] == '=')
                {
                    if (kind == FunctionKind.Arrow)
                        ExceptionHelper.ThrowSyntaxError("Parameters of arrow-function cannot have an initializer", code, position);

                    if (rest)
                        ExceptionHelper.ThrowSyntaxError("Rest parameters can not have an initializer", code, position);
                    do
                        position++;
                    while (Tools.IsWhiteSpace(code[position]));
                    desc.initializer = ExpressionTree.Parse(state, ref position, false, false);
                }

                if (code[position] == ',')
                {
                    if (rest)
                        ExceptionHelper.ThrowSyntaxError("Rest parameters must be the last in parameters list", code, position);
                    do
                        position++;
                    while (Tools.IsWhiteSpace(code[position]));
                }
            }

            if (kind == FunctionKind.Setter)
            {
                if (parameters.Count != 1)
                    ExceptionHelper.ThrowSyntaxError("Setter must have only one argument", code, index);
            }

            position++;
            Tools.SkipSpaces(code, ref position);

            if (kind == FunctionKind.Arrow)
            {
                if (!Parser.Validate(code, "=>", ref position))
                    ExceptionHelper.ThrowSyntaxError("Expected \"=>\"", code, position);
                Tools.SkipSpaces(code, ref position);
            }

            if (code[position] != '{')
            {
                var oldFunctionScopeLevel = state.FunctionScopeLevel;
                state.FunctionScopeLevel = ++state.LexicalScopeLevel;

                try
                {
                    if (kind == FunctionKind.Arrow)
                    {
                        body = new CodeBlock(new CodeNode[]
                        {
                            new Return(ExpressionTree.Parse(state, ref position, processComma: false))
                        })
                        {
                            _variables = new VariableDescriptor[0]
                        };

                        body.Position = body._lines[0].Position;
                        body.Length = body._lines[0].Length;
                    }
                    else
                        ExceptionHelper.ThrowUnknownToken(code, position);
                }
                finally
                {
                    state.FunctionScopeLevel = oldFunctionScopeLevel;
                    state.LexicalScopeLevel--;
                }
            }
            else
            {
                var oldCodeContext = state.CodeContext;
                state.CodeContext &= ~(CodeContext.InExpression | CodeContext.Conditional | CodeContext.InEval);
                if (kind == FunctionKind.Generator || kind == FunctionKind.MethodGenerator || kind == FunctionKind.AnonymousGenerator)
                    state.CodeContext |= CodeContext.InGenerator;
                else if (kind == FunctionKind.AsyncFunction)
                    state.CodeContext |= CodeContext.InAsync;
                state.CodeContext |= CodeContext.InFunction;

                var labels = state.Labels;
                state.Labels = new List<string>();
                state.AllowReturn++;
                try
                {
                    state.AllowBreak.Push(false);
                    state.AllowContinue.Push(false);
                    state.AllowDirectives = true;
                    body = CodeBlock.Parse(state, ref position) as CodeBlock;
                    if (containsDestructuringPrms)
                    {
                        var destructuringTargets = new List<VariableDescriptor>();
                        var assignments = new List<Expression>();
                        foreach (var parameter in parameters)
                        {
                            if (parameter.Destructor == null) continue;

                            var targets = parameter.Destructor.GetTargetVariables();
                            destructuringTargets.AddRange(targets.Select(t => new VariableDescriptor(t.Name, state.FunctionScopeLevel)));

                            assignments.Add(new Assignment(parameter.Destructor, parameter.references[0]));
                        }

                        var newLines = new CodeNode[body._lines.Length + 1];
                        Array.Copy(body._lines, 0, newLines, 1, body._lines.Length);
                        newLines[0] = new VariableDefinition(destructuringTargets.ToArray(), assignments.ToArray(), VariableKind.AutoGeneratedParameters);
                        body._lines = newLines;
                    }
                }
                finally
                {
                    state.CodeContext = oldCodeContext;
                    state.AllowBreak.Pop();
                    state.AllowContinue.Pop();
                    state.AllowDirectives = false;
                    state.Labels = labels;
                    state.AllowReturn--;
                }

                if (kind == FunctionKind.Function && string.IsNullOrEmpty(name))
                    kind = FunctionKind.AnonymousFunction;
            }

            if (body._strict || (parameters.Count > 0 && parameters[parameters.Count - 1].IsRest) || kind == FunctionKind.Arrow)
            {
                for (var j = parameters.Count; j-- > 1;)
                    for (var k = j; k-- > 0;)
                        if (parameters[j].Name == parameters[k].Name)
                            ExceptionHelper.ThrowSyntaxError("Duplicate names of function parameters not allowed in strict mode", code, index);

                if (name == "arguments" || name == "eval")
                    ExceptionHelper.ThrowSyntaxError("Functions name can not be \"arguments\" or \"eval\" in strict mode at", code, index);

                for (int j = parameters.Count; j-- > 0;)
                {
                    if (parameters[j].Name == "arguments" || parameters[j].Name == "eval")
                        ExceptionHelper.ThrowSyntaxError("Parameters name cannot be \"arguments\" or \"eval\" in strict mode at", code, parameters[j].references[0].Position, parameters[j].references[0].Length);
                }
            }

            var func = new FunctionDefinition(name)
            {
                Parameters = parameters.ToArray(),
                Body = body,
                Kind = kind,
                Position = index,
                Length = position - index,
            };

            if (!string.IsNullOrEmpty(name))
            {
                func.Reference.ScopeLevel = state.LexicalScopeLevel;
                func.Reference.Position = nameStartPos;
                func.Reference.Length = name.Length;

                func.Reference._descriptor.DefinitionScopeLevel = func.Reference.ScopeLevel;
            }

            if (parameters.Count != 0)
            {
                var newVariablesCount = body._variables.Length + parameters.Count;

                foreach (var variable in body._variables)
                {
                    if (parameters.Any(t => variable.name == t.name))
                    {
                        newVariablesCount--;
                    }
                }

                var newVariables = new VariableDescriptor[newVariablesCount];
                for (var j = 0; j < parameters.Count; j++)
                {
                    newVariables[j] = parameters[parameters.Count - j - 1]; // порядок определяет приоритет
                    for (var k = 0; k < body._variables.Length; k++)
                    {
                        if (body._variables[k] != null && body._variables[k].name == parameters[j].name)
                        {
                            if (body._variables[k].initializer != null)
                                newVariables[j] = body._variables[k];
                            else
                                body._variables[k].lexicalScope = false;
                            body._variables[k] = null;
                            break;
                        }
                    }
                }

                for (int j = 0, k = parameters.Count; j < body._variables.Length; j++)
                {
                    if (body._variables[j] != null)
                        newVariables[k++] = body._variables[j];
                }

                body._variables = newVariables;
            }

            if ((state.CodeContext & CodeContext.InExpression) == 0 && kind == FunctionKind.Function)
            {
                var tindex = position;
                while (position < code.Length && Tools.IsWhiteSpace(code[position]) && !Tools.IsLineTerminator(code[position]))
                    position++;

                if (position < code.Length && code[position] == '(')
                {
                    var args = new List<Expression>();
                    position++;
                    for (;;)
                    {
                        while (Tools.IsWhiteSpace(code[position]))
                            position++;
                        if (code[position] == ')')
                            break;
                        if (code[position] == ',')
                            do
                                position++;
                            while (Tools.IsWhiteSpace(code[position]));
                        args.Add(ExpressionTree.Parse(state, ref position, false, false));
                    }

                    position++;
                    index = position;
                    while (position < code.Length && Tools.IsWhiteSpace(code[position]))
                        position++;

                    if (position < code.Length && code[position] == ';')
                        ExceptionHelper.Throw((new SyntaxError("Expression can not start with word \"function\"")));

                    return new Call(func, args.ToArray());
                }

                position = tindex;
            }

            if ((state.CodeContext & CodeContext.InExpression) == 0
                && (kind != FunctionKind.Arrow || (state.CodeContext & CodeContext.InEval) == 0))
            {
                if (string.IsNullOrEmpty(name))
                {
                    ExceptionHelper.ThrowSyntaxError("Function must have name", state.Code, index);
                }

                if (state.Strict && state.FunctionScopeLevel != state.LexicalScopeLevel)
                {
                    ExceptionHelper.ThrowSyntaxError("In strict mode code, functions can only be declared at top level or immediately within other function.", state.Code, index);
                }

                state.Variables.Add(func.Reference._descriptor);
            }

            index = position;
            return func;
        }

        public override JSValue Evaluate(Context context)
        {
            return MakeFunction(context);
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            var res = new CodeNode[1 + Parameters.Length + (Reference != null ? 1 : 0)];
            for (var i = 0; i < Parameters.Length; i++)
                res[i] = Parameters[i].references[0];
            res[Parameters.Length] = Body;

            if (Reference != null)
                res[res.Length - 1] = Reference;

            return res;
        }

        public Function MakeFunction(Module script)
        {
            return MakeFunction(script.Context);
        }

        public Function MakeFunction(Context context)
        {
            if (Kind == FunctionKind.Generator || Kind == FunctionKind.MethodGenerator || Kind == FunctionKind.AnonymousGenerator)
                return new GeneratorFunction(context, this);

            if (Kind == FunctionKind.AsyncFunction || Kind == FunctionKind.AsyncAnonymousFunction || Kind == FunctionKind.AsyncArrow)
                return new AsyncFunction(context, this);

            if (Body != null)
            {
                if (Body._lines.Length == 0)
                {
                    return new ConstantFunction(JSValue.notExists, this);
                }

                if (Body._lines.Length == 1)
                {
                    if (Body._lines[0] is Return ret && (ret.Value == null || ret.Value.ContextIndependent))
                    {
                        return new ConstantFunction(ret.Value?.Evaluate(null) ?? JSValue.undefined, this);
                    }
                }
            }

            if (!FunctionInfo.ContainsArguments
                && !FunctionInfo.ContainsRestParameters
                && !FunctionInfo.ContainsEval
                && !FunctionInfo.ContainsWith)
            {
                return new SimpleFunction(context, this);
            }

            return new Function(context, this);
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (Body.built)
                return false;

            if (stats != null)
                stats.ContainsInnerEntities = true;

            _codeContext = codeContext;

            if ((codeContext & CodeContext.InLoop) != 0)
                message?.Invoke(MessageLevel.Warning, Position, EndPosition - Position, Messages.FunctionInLoop);

            var numbersOfReferences = new Dictionary<string, int>();
            foreach (var variable in variables)
            {
                numbersOfReferences[variable.Key] = variable.Value.references.Count;
            }

            VariableDescriptor descriptorToRestore = null;
            if (!string.IsNullOrEmpty(Name))
            {
                variables.TryGetValue(Name, out descriptorToRestore);
                variables[Name] = Reference._descriptor;
            }

            FunctionInfo.ContainsRestParameters = Parameters.Length > 0 && Parameters[Parameters.Length - 1].IsRest;

            var bodyCode = Body as CodeNode;
            bodyCode.Build(
                ref bodyCode,
                0,
                variables,
                codeContext & ~(CodeContext.Conditional
                              | CodeContext.InExpression
                              | CodeContext.InEval)
                            | CodeContext.InFunction,
                message,
                FunctionInfo,
                opts);
            Body = bodyCode as CodeBlock;

            if (message != null)
            {
                for (var i = Parameters.Length; i-- > 0;)
                {
                    if (Parameters[i].ReferenceCount == 1)
                        message(MessageLevel.Recomendation, Parameters[i].references[0].Position, 0, "Unused parameter \"" + Parameters[i].name + "\"");
                    else
                        break;
                }
            }

            Body._suppressScopeIsolation = SuppressScopeIsolationMode.Suppress;
            checkUsings();
            if (stats != null)
            {
                stats.ContainsDebugger |= FunctionInfo.ContainsDebugger;
                stats.ContainsEval |= FunctionInfo.ContainsEval;
                stats.ContainsInnerEntities = true;
                stats.ContainsTry |= FunctionInfo.ContainsTry;
                stats.ContainsWith |= FunctionInfo.ContainsWith;
                stats.NeedDecompose |= FunctionInfo.NeedDecompose;
                stats.UseCall |= FunctionInfo.UseCall;
                stats.UseGetMember |= FunctionInfo.UseGetMember;
                stats.ContainsThis |= FunctionInfo.ContainsThis;
            }

            if (descriptorToRestore != null)
            {
                variables[descriptorToRestore.name] = descriptorToRestore;
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                variables.Remove(Name);
            }

            foreach (var variable in variables)
            {
                if (!numbersOfReferences.TryGetValue(variable.Key, out var count) || count != variable.Value.references.Count)
                {
                    variable.Value.captured = true;
                    if ((codeContext & CodeContext.InWith) == 0) continue;
                    for (var i = count; i < variable.Value.references.Count; i++)
                        variable.Value.references[i].ScopeLevel = -Math.Abs(variable.Value.references[i].ScopeLevel);
                }
            }

            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            var bd = Body as CodeNode;
            var oldScopeIsolation = Body._suppressScopeIsolation;
            Body._suppressScopeIsolation = SuppressScopeIsolationMode.DoNotSuppress;
            Body.Optimize(ref bd, this, message, opts, FunctionInfo);
            Body._suppressScopeIsolation = oldScopeIsolation;

            if (FunctionInfo.Returns.Count > 0)
            {
                FunctionInfo.ResultType = FunctionInfo.Returns[0].ResultType;
                for (var i = 1; i < FunctionInfo.Returns.Count; i++)
                {
                    if (FunctionInfo.ResultType != FunctionInfo.Returns[i].ResultType)
                    {
                        FunctionInfo.ResultType = PredictedType.Ambiguous;
                        if (message != null
                            && FunctionInfo.ResultType >= PredictedType.Undefined
                            && FunctionInfo.Returns[i].ResultType >= PredictedType.Undefined)
                            message(MessageLevel.Warning, Parameters[i].references[0].Position, 0, "Type of return value is ambiguous");
                        break;
                    }
                }
            }
            else
                FunctionInfo.ResultType = PredictedType.Undefined;
        }

        private void checkUsings()
        {
            if (Body?._lines == null || Body._lines.Length == 0)
                return;

            if (Body._variables == null) return;

            var containsEntities = FunctionInfo.ContainsInnerEntities;
            if (!containsEntities)
            {
                for (var i = 0; !containsEntities && i < Body._variables.Length; i++)
                    containsEntities |= Body._variables[i].initializer != null;
                FunctionInfo.ContainsInnerEntities = containsEntities;
            }
            foreach (var variable in Body._variables)
            {
                FunctionInfo.ContainsArguments |= variable.name == "arguments";
            }
        }

        internal override System.Linq.Expressions.Expression TryCompile(bool selfCompile, bool forAssign, Type expectedType, List<CodeNode> dynamicValues)
        {
            Body.TryCompile(true, false, null, new List<CodeNode>());
            return null;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {
            CodeNode cn = Body;
            cn.Decompose(ref cn);
            Body = (CodeBlock)cn;
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            base.RebuildScope(functionInfo, null, scopeBias);

            var tempVariables = FunctionInfo.WithLexicalEnvironment ? null : new Dictionary<string, VariableDescriptor>();
            Body.RebuildScope(FunctionInfo, tempVariables, scopeBias + (Body._variables == null || Body._variables.Length == 0 || !FunctionInfo.WithLexicalEnvironment ? 1 : 0));
            if (tempVariables != null && Body is CodeBlock block)
                block._variables = tempVariables.Values.Where(x => !(x is ParameterDescriptor)).ToArray();
        }

        public override string ToString()
        {
            return ToString(false);
        }

        internal string ToString(bool headerOnly)
        {
            StringBuilder code = new StringBuilder();
            switch (Kind)
            {
                case FunctionKind.Generator:
                    {
                        code.Append("functions* ");
                        break;
                    }
                case FunctionKind.Method:
                    {
                        break;
                    }
                case FunctionKind.Getter:
                    {
                        code.Append("get ");
                        break;
                    }
                case FunctionKind.Setter:
                    {
                        code.Append("set ");
                        break;
                    }
                case FunctionKind.Arrow:
                    {
                        break;
                    }
                case FunctionKind.AsyncFunction:
                    {
                        code.Append("async ");
                        goto default;
                    }
                default:
                    {
                        code.Append("function ");
                        break;
                    }
            }

            code.Append(Name)
                .Append("(");

            if (Parameters != null)
                for (int i = 0; i < Parameters.Length;)
                    code.Append(Parameters[i])
                        .Append(++i < Parameters.Length ? "," : "");

            code.Append(")");

            if (!headerOnly)
            {
                code.Append(" ");
                if (Kind == FunctionKind.Arrow)
                    code.Append("=> ");

                if (Kind == FunctionKind.Arrow
                    && Body._lines.Length == 1
                    && Body.Position == Body._lines[0].Position)
                    code.Append(Body._lines[0].Childs[0]);
                else
                    code.Append((object)Body ?? "{ [native code] }");
            }

            return code.ToString();
        }
    }
}
