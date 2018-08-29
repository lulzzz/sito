using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JS.Core.Core;
using JS.Core.Extensions;
using NiL.JS;
using NiL.JS.BaseLibrary;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class MemberDescriptor
    {
        internal Expression _name;
        internal Expression _value;
        internal bool _static;

        public Expression Name => _name;
        public Expression Value => _value;
        public bool Static => _static;

        public MemberDescriptor(Expression name, Expression value, bool @static)
        {
            _name = name;
            _value = value;
            _static = @static;
        }

        public override string ToString()
        {
            if (_static)
                return "static " + _value;
            return _value.ToString();
        }
    }

#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ClassDefinition : EntityDefinition
    {
        private sealed class ClassConstructor : Function
        {
            private readonly ClassDefinition classDefinition;
            public override string name => classDefinition.Name;

            public ClassConstructor(Context context, FunctionDefinition creator, ClassDefinition classDefinition)
                : base(context, creator)
            {
                this.classDefinition = classDefinition;
            }

            protected internal override JSValue ConstructObject()
            {
                var result = CreateObject();
                result.__proto__ = prototype._oValue as JSObject;
                return result;
            }

            public override string ToString(bool headerOnly)
            {
                return classDefinition.ToString();
            }
        }

        protected internal override PredictedType ResultType => PredictedType.Function;

        internal override bool ResultInTempContainer => false;

        protected internal override bool ContextIndependent => false;

        protected internal override bool NeedDecompose
        {
            get
            {
                if (_constructor.NeedDecompose)
                    return true;

                for (var i = 0; i < _members.Length; i++)
                {
                    if (_members[i]._value.NeedDecompose)
                        return true;
                }

                return false;
            }
        }

        private readonly MemberDescriptor[] _members;
        private Expression _baseClass;
        private FunctionDefinition _constructor;
        private readonly MemberDescriptor[] computedProperties;

        public IEnumerable<MemberDescriptor> Members => _members;
        public Expression BaseClass => _baseClass;
        public FunctionDefinition Constructor => _constructor;
        public IEnumerable<MemberDescriptor> ComputedProperties => computedProperties;

        public override bool Hoist => false;

        private ClassDefinition(string name, Expression baseType, MemberDescriptor[] fields, FunctionDefinition ctor, MemberDescriptor[] computedProperties)
            : base(name)
        {
            this._baseClass = baseType;
            this._constructor = ctor;
            this._members = fields;
            this.computedProperties = computedProperties;
        }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            string code = state.Code;
            int i = index;
            if (!Parser.Validate(code, "class", ref i))
                return null;
            while (Tools.IsWhiteSpace(code[i]))
                i++;
            string name = null;
            Expression baseType = null;
            if (!Parser.Validate(code, "extends ", i))
            {
                var n = i;
                if (Parser.ValidateName(code, ref i, true))
                    name = code.Substring(n, i - n);

                while (Tools.IsWhiteSpace(code[i]))
                    i++;
            }
            if (Parser.Validate(code, "extends ", ref i))
            {
                var n = i;
                if (!Parser.ValidateName(code, ref i, true) && !Parser.Validate(code, "null", ref i))
                    ExceptionHelper.ThrowSyntaxError("Invalid base class name", state.Code, i);

                var baseClassName = code.Substring(n, i - n);
                if (baseClassName == "null")
                    baseType = new Constant(JSValue.@null);
                else
                    baseType = new Variable(baseClassName, 1);

                baseType.Position = n;
                baseType.Length = i - n;

                while (Tools.IsWhiteSpace(code[i]))
                    i++;
            }
            if (code[i] != '{')
                ExceptionHelper.ThrowSyntaxError(Messages.UnexpectedToken, code, i);

            FunctionDefinition ctor = null;
            ClassDefinition result = null;
            var oldStrict = state.strict;
            state.strict = true;
            var flds = new Dictionary<string, MemberDescriptor>();
            var computedProperties = new List<MemberDescriptor>();
            var oldCodeContext = state.CodeContext;
            state.CodeContext |= CodeContext.InExpression;

            try
            {
                while (code[i] != '}')
                {
                    do
                        i++;
                    while (Tools.IsWhiteSpace(code[i]) || code[i] == ';');
                    int s = i;
                    if (state.Code[i] == '}')
                        break;

                    bool @static = Parser.Validate(state.Code, "static", ref i);
                    if (@static)
                    {
                        Tools.SkipSpaces(state.Code, ref i);
                        s = i;
                    }

                    bool getOrSet = Parser.Validate(state.Code, "get", ref i) || Parser.Validate(state.Code, "set", ref i);
                    if (getOrSet)
                    {
                        Tools.SkipSpaces(state.Code, ref i);
                    }

                    var asterisk = state.Code[i] == '*';
                    if (asterisk)
                    {
                        do
                            i++;
                        while (Tools.IsWhiteSpace(state.Code[i]));
                    }

                    if (Parser.Validate(state.Code, "[", ref i))
                    {
                        var propertyName = ExpressionTree.Parse(state, ref i, false, false, false, true, false);
                        while (Tools.IsWhiteSpace(state.Code[i]))
                            i++;
                        if (state.Code[i] != ']')
                            ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, i);
                        do
                            i++;
                        while (Tools.IsWhiteSpace(state.Code[i]));

                        CodeNode initializer;
                        if (state.Code[i] == '(')
                        {
                            initializer = FunctionDefinition.Parse(state, ref i, asterisk ? FunctionKind.AnonymousGenerator : FunctionKind.AnonymousFunction);
                        }
                        else
                        {
                            initializer = ExpressionTree.Parse(state, ref i);
                        }

                        switch (state.Code[s])
                        {
                            case 'g':
                                {
                                    computedProperties.Add(new MemberDescriptor((Expression)propertyName, new PropertyPair((Expression)initializer, null), @static));
                                    break;
                                }
                            case 's':
                                {
                                    computedProperties.Add(new MemberDescriptor((Expression)propertyName, new PropertyPair(null, (Expression)initializer), @static));
                                    break;
                                }
                            default:
                                {
                                    computedProperties.Add(new MemberDescriptor((Expression)propertyName, (Expression)initializer, @static));
                                    break;
                                }
                        }
                    }
                    else if (getOrSet)
                    {
                        i = s;
                        var mode = state.Code[i] == 's' ? FunctionKind.Setter : FunctionKind.Getter;
                        var propertyAccessor = FunctionDefinition.Parse(state, ref i, mode) as FunctionDefinition;
                        var accessorName = (@static ? "static " : "") + propertyAccessor.Name;
                        if (!flds.ContainsKey(accessorName))
                        {
                            var propertyPair = new PropertyPair
                            (
                                mode == FunctionKind.Getter ? propertyAccessor : null,
                                mode == FunctionKind.Setter ? propertyAccessor : null
                            );
                            flds.Add(accessorName, new MemberDescriptor(new Constant(propertyAccessor.Name), propertyPair, @static));
                        }
                        else
                        {
                            var vle = flds[accessorName].Value as PropertyPair;

                            if (vle == null)
                                ExceptionHelper.Throw((new SyntaxError("Try to define " + mode.ToString().ToLowerInvariant() + " for defined field at " + CodeCoordinates.FromTextPosition(state.Code, s, 0))));

                            do
                            {
                                if (mode == FunctionKind.Getter)
                                {
                                    if (vle.Getter == null)
                                    {
                                        vle.Getter = propertyAccessor;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (vle.Setter == null)
                                    {
                                        vle.Setter = propertyAccessor;
                                        break;
                                    }
                                }

                                ExceptionHelper.ThrowSyntaxError("Try to redefine " + mode.ToString().ToLowerInvariant() + " of " + propertyAccessor.Name, state.Code, s);
                            }
                            while (false);
                        }
                    }
                    else
                    {
                        i = s;
                        string fieldName = null;
                        if (state.Code[i] == '*')
                        {
                            do
                                i++;
                            while (Tools.IsWhiteSpace(code[i]));
                        }

                        if (Parser.ValidateName(state.Code, ref i, false, true, state.strict))
                            fieldName = Tools.Unescape(state.Code.Substring(s, i - s), state.strict);
                        else if (Parser.ValidateValue(state.Code, ref i))
                        {
                            double d = 0.0;
                            int n = s;
                            if (Tools.ParseNumber(state.Code, ref n, out d))
                                fieldName = Tools.DoubleToString(d);
                            else if (state.Code[s] == '\'' || state.Code[s] == '"')
                                fieldName = Tools.Unescape(state.Code.Substring(s + 1, i - s - 2), state.strict);
                        }

                        if (fieldName == null)
                            ExceptionHelper.Throw((new SyntaxError("Invalid member name at " + CodeCoordinates.FromTextPosition(state.Code, s, i - s))));

                        if (fieldName == "constructor")
                        {
                            if (@static)
                            {
                                ExceptionHelper.ThrowSyntaxError(Messages.ConstructorCannotBeStatic, state.Code, s);
                            }
                            if (ctor != null)
                            {
                                ExceptionHelper.ThrowSyntaxError("Trying to redefinition constructor", state.Code, s);
                            }

                            state.CodeContext |= CodeContext.InClassConstructor;
                        }
                        else if (@static)
                        {
                            fieldName = "static " + fieldName;
                            state.CodeContext |= CodeContext.InStaticMember;
                        }
                        if (flds.ContainsKey(fieldName))
                            ExceptionHelper.Throw(new SyntaxError("Trying to redefinition member \"" + fieldName + "\" at " + CodeCoordinates.FromTextPosition(state.Code, s, i - s)));

                        state.CodeContext |= CodeContext.InClassDefenition;
                        state.CodeContext &= ~CodeContext.InGenerator;

                        i = s;
                        var method = FunctionDefinition.Parse(state, ref i, FunctionKind.Method) as FunctionDefinition;
                        if (method == null)
                            ExceptionHelper.ThrowSyntaxError("Unable to parse constructor", state.Code, i);

                        if (fieldName == "constructor")
                        {
                            ctor = method;
                        }
                        else
                        {
                            flds[fieldName] = new MemberDescriptor(new Constant(method.Name), method, @static);
                        }
                    }

                    code = state.Code;
                }

                if (ctor == null)
                {
                    string ctorCode;
                    int ctorIndex = 0;
                    if (baseType != null && !(baseType is Constant))
                        ctorCode = "constructor(...args) { super(...args); }";
                    else
                        ctorCode = "constructor(...args) { }";
                    ctor = (FunctionDefinition)FunctionDefinition.Parse(
                        new ParseInfo(ctorCode, ctorCode, null)
                        {
                            strict = true,
                            CodeContext = CodeContext.InClassConstructor | CodeContext.InClassDefenition
                        },
                        ref ctorIndex,
                        FunctionKind.Method);
                }

                result = new ClassDefinition(name, baseType, new List<MemberDescriptor>(flds.Values).ToArray(), ctor as FunctionDefinition, computedProperties.ToArray());

                if ((oldCodeContext & CodeContext.InExpression) == 0)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        ExceptionHelper.ThrowSyntaxError("Class must have name", state.Code, index);
                    }
                    if (state.strict && state.functionScopeLevel != state.lexicalScopeLevel)
                    {
                        ExceptionHelper.ThrowSyntaxError("In strict mode code, class can only be declared at top level or immediately within other function.", state.Code, index);
                    }

                    state.Variables.Add(result.Reference._descriptor);
                }
            }
            finally
            {
                state.CodeContext = oldCodeContext;
                state.strict = oldStrict;
            }
            index = i + 1;
            return result;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            if (Built)
                return false;
            Built = true;

            _codeContext = codeContext;

            if ((codeContext & CodeContext.InExpression) == 0)
                stats.WithLexicalEnvironment = true;

            VariableDescriptor descriptorToRestore = null;
            if (!string.IsNullOrEmpty(Name))
            {
                variables.TryGetValue(Name, out descriptorToRestore);
                variables[Name] = Reference._descriptor;
            }

            Parser.Build(ref _constructor, expressionDepth, variables, codeContext | CodeContext.InClassDefenition | CodeContext.InClassConstructor, message, stats, opts);
            Parser.Build(ref _baseClass, expressionDepth, variables, codeContext, message, stats, opts);

            foreach (var member in _members)
            {
                Parser.Build
                (
                    ref member._value,
                    expressionDepth,
                    variables,
                    codeContext | CodeContext.InClassDefenition | (member._static ? CodeContext.InStaticMember : 0),
                    message,
                    stats,
                    opts
                );
            }

            foreach (var prop in computedProperties)
            {
                Parser.Build(ref prop._name, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
                Parser.Build(ref prop._value, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
            }

            if (descriptorToRestore != null)
            {
                variables[descriptorToRestore.name] = descriptorToRestore;
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                variables.Remove(Name);
            }

            return false;
        }

        public override JSValue Evaluate(Context context)
        {
            JSValue variable = null;
            if ((_codeContext & CodeContext.InExpression) == 0)
            {
                variable = context.DefineVariable(Name);
            }

            var ctor = new ClassConstructor(context, this._constructor, this)
            {
                RequireNewKeywordLevel = RequireNewKeywordLevel.WithNewOnly
            };

            if (_baseClass != null)
            {
                JSValue baseProto = _baseClass.Evaluate(context)._oValue as JSObject;
                if (baseProto == null)
                {
                    ctor.prototype.__proto__ = null;
                }
                else
                {
                    ctor.prototype.__proto__ = Tools.InvokeGetter(baseProto.GetProperty("prototype"), baseProto)._oValue as JSObject;
                }
                ctor.__proto__ = baseProto as JSObject;
            }

            foreach (var member in _members)
            {
                var value = member.Value.Evaluate(context);
                var target = member.Static ? ctor : ctor.prototype;

                target.SetProperty(member.Name.Evaluate(null), value, true);
            }

            foreach (var prop in computedProperties)
            {
                JSObject target = prop.Static ? ctor : ctor.prototype._oValue as JSObject;

                var key = prop._name.Evaluate(context).CloneImpl(false);
                var value = prop._value.Evaluate(context).CloneImpl(false);

                JSValue existedValue;
                Symbol symbolKey = null;
                string stringKey = null;
                if (key.Is<Symbol>())
                {
                    symbolKey = key.As<Symbol>();
                    if (target._symbols == null)
                        target._symbols = new Dictionary<Symbol, JSValue>();

                    if (!target._symbols.TryGetValue(symbolKey, out existedValue))
                        target._symbols[symbolKey] = existedValue = value;
                }
                else
                {
                    stringKey = key.As<string>();
                    if (!target._fields.TryGetValue(stringKey, out existedValue))
                        target._fields[stringKey] = existedValue = value;
                }

                if (existedValue == value) continue;

                if (existedValue.Is(JSValueType.Property) && value.Is(JSValueType.Property))
                {
                    var egs = existedValue.As<global::JS.Core.Core.PropertyPair>();
                    var ngs = value.As<global::JS.Core.Core.PropertyPair>();
                    egs.getter = ngs.getter ?? egs.getter;
                    egs.setter = ngs.setter ?? egs.setter;
                }
                else
                {
                    if (key.Is<Symbol>())
                    {
                        target._symbols[symbolKey] = value;
                    }
                    else
                    {
                        target._fields[stringKey] = value;
                    }
                }
            }

            if ((_codeContext & CodeContext.InExpression) == 0)
                variable.Assign(ctor);
            return ctor;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            var result = _members.Select(t => t._value).Cast<CodeNode>().ToList();

            foreach (var prop in computedProperties)
            {
                result.Add(prop.Name);
                result.Add(prop.Value);
            }

            if (_baseClass != null)
                result.Add(_baseClass);

            return result.ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("class ").Append(Name);
            if (_baseClass != null)
                result.Append(" extends ").Append(_baseClass);
            result.Append(" {").Append(Environment.NewLine);

            var temp = _constructor.ToString().Replace(Environment.NewLine, Environment.NewLine + "  ");
            result.Append("constructor");
            result.Append(temp.Substring("constructor".Length));

            foreach (var member in _members)
            {
                temp = member.ToString().Replace(Environment.NewLine, Environment.NewLine + "  ");
                result.Append(temp);
            }

            result.Append(Environment.NewLine).Append("}");
            return result.ToString();
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {
            foreach (var member in _members)
            {
                member._value.Decompose(ref member._value, result); // results will be empty at each iterations
            }

            foreach (var prop in computedProperties)
            {
                prop._name.Decompose(ref prop._name, result);

                prop._value.Decompose(ref prop._value, result);
            }
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            _baseClass?.Optimize(ref _baseClass, owner, message, opts, stats);

            for (var i = _members.Length; i-- > 0;)
            {
                _members[i]._value.Optimize(ref _members[i]._value, owner, message, opts, stats);
            }

            foreach (var prop in computedProperties)
            {
                prop._name.Optimize(ref prop._name, owner, message, opts, stats);
                prop._value.Optimize(ref prop._value, owner, message, opts, stats);
            }
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            base.RebuildScope(functionInfo, null, scopeBias);

            _baseClass?.RebuildScope(functionInfo, null, scopeBias);
            _constructor.RebuildScope(functionInfo, null, scopeBias);
            foreach (var prop in computedProperties)
            {
                prop.Name.RebuildScope(functionInfo, null, scopeBias);
                prop.Value.RebuildScope(functionInfo, null, scopeBias);
            }
            foreach (var member in _members)
            {
                member.Name.RebuildScope(functionInfo, null, scopeBias);
                member.Value.RebuildScope(functionInfo, null, scopeBias);
            }
        }
    }
}