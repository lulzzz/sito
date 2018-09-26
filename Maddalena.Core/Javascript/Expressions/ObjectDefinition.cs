using System;
using System.Collections.Generic;
using System.Linq;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;
using Maddalena.Core.Javascript.Extensions;
using Maddalena.Core.Javascript.Statements;

namespace Maddalena.Core.Javascript.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
    public sealed class ObjectDefinition : Expression
    {
        public string[] FieldNames { get; }

        public Expression[] Values { get; }

        public KeyValuePair<Expression, Expression>[] ComputedProperties { get; }

        protected internal override bool ContextIndependent => false;

        protected internal override PredictedType ResultType => PredictedType.Object;

        internal override bool ResultInTempContainer => false;

        protected internal override bool NeedDecompose
        {
            get
            {
                return Values.Any(x => x.NeedDecompose);
            }
        }

        private ObjectDefinition(Dictionary<string, Expression> fields, KeyValuePair<Expression, Expression>[] computedProperties)
        {
            ComputedProperties = computedProperties;
            FieldNames = new string[fields.Count];
            Values = new Expression[fields.Count];

            int i = 0;
            foreach (var f in fields)
            {
                FieldNames[i] = f.Key;
                Values[i++] = f.Value;
            }
        }

        internal static CodeNode Parse(ParseInfo state, ref int index)
        {
            if (state.Code[index] != '{')
                throw new ArgumentException("Invalid JSON definition");
            var flds = new Dictionary<string, Expression>();
            var computedProperties = new List<KeyValuePair<Expression, Expression>>();
            int i = index;
            while (state.Code[i] != '}')
            {
                i++;
                Tools.SkipSpaces(state.Code, ref i);
                int s = i;
                if (state.Code[i] == '}')
                    break;

                bool getOrSet = Parser.Validate(state.Code, "get", ref i) || Parser.Validate(state.Code, "set", ref i);
                Tools.SkipSpaces(state.Code, ref i);
                if (getOrSet && state.Code[i] == '(')  // function with name 'get' or 'set'
                {
                    getOrSet = false;
                    i = s;
                }

                var asterisk = state.Code[i] == '*';
                Tools.SkipSpaces(state.Code, ref i);

                if (Parser.Validate(state.Code, "[", ref i))
                {
                    var name = ExpressionTree.Parse(state, ref i, false, false, false, true, false);
                    while (Tools.IsWhiteSpace(state.Code[i]))
                        i++;
                    if (state.Code[i] != ']')
                        ExceptionHelper.ThrowSyntaxError("Expected ']'", state.Code, i);
                    do
                        i++;
                    while (Tools.IsWhiteSpace(state.Code[i]));

                    Tools.SkipSpaces(state.Code, ref i);
                    if (state.Code[s] != 'g' && state.Code[s] != 's')
                    {
                        if (!Parser.Validate(state.Code, ":", ref i))
                            ExceptionHelper.ThrowSyntaxError(Messages.UnexpectedToken, state.Code, i);
                        Tools.SkipSpaces(state.Code, ref i);
                    }

                    CodeNode initializer;
                    initializer = state.Code[i] == '(' ? FunctionDefinition.Parse(state, ref i, asterisk ? FunctionKind.AnonymousGenerator : FunctionKind.AnonymousFunction) : ExpressionTree.Parse(state, ref i);

                    switch (state.Code[s])
                    {
                        case 'g':
                            {
                                computedProperties.Add(new KeyValuePair<Expression, Expression>(name, new PropertyPair((Expression)initializer, null)));
                                break;
                            }
                        case 's':
                            {
                                computedProperties.Add(new KeyValuePair<Expression, Expression>(name, new PropertyPair(null, (Expression)initializer)));
                                break;
                            }
                        default:
                            {
                                computedProperties.Add(new KeyValuePair<Expression, Expression>(name, (Expression)initializer));
                                break;
                            }
                    }
                }
                else if (getOrSet && state.Code[i] != ':')
                {
                    i = s;
                    var mode = state.Code[i] == 's' ? FunctionKind.Setter : FunctionKind.Getter;
                    var propertyAccessor = FunctionDefinition.Parse(state, ref i, mode) as FunctionDefinition;
                    var accessorName = propertyAccessor.Name;
                    if (!flds.ContainsKey(accessorName))
                    {
                        var propertyPair = new PropertyPair
                        (
                            mode == FunctionKind.Getter ? propertyAccessor : null,
                            mode == FunctionKind.Setter ? propertyAccessor : null
                        );
                        flds.Add(accessorName, propertyPair);
                    }
                    else
                    {
                        var vle = flds[accessorName] as PropertyPair;

                        if (vle == null)
                            ExceptionHelper.ThrowSyntaxError("Try to define " + mode.ToString().ToLowerInvariant() + " for defined field", state.Code, s);

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
                    if (asterisk)
                    {
                        do
                            i++;
                        while (Tools.IsWhiteSpace(state.Code[i]));
                    }

                    i = s;
                    var fieldName = "";
                    if (Parser.ValidateName(state.Code, ref i, false, true, state.Strict))
                        fieldName = Tools.Unescape(state.Code.Substring(s, i - s), state.Strict);
                    else if (Parser.ValidateValue(state.Code, ref i))
                    {
                        if (state.Code[s] == '-')
                            ExceptionHelper.Throw(new SyntaxError("Invalid char \"-\" at " + CodeCoordinates.FromTextPosition(state.Code, s, 1)));
                        double d = 0.0;
                        int n = s;
                        if (Tools.ParseNumber(state.Code, ref n, out d))
                            fieldName = Tools.DoubleToString(d);
                        else if (state.Code[s] == '\'' || state.Code[s] == '"')
                            fieldName = Tools.Unescape(state.Code.Substring(s + 1, i - s - 2), state.Strict);
                        else if (flds.Count != 0)
                            ExceptionHelper.Throw((new SyntaxError("Invalid field name at " + CodeCoordinates.FromTextPosition(state.Code, s, i - s))));
                        else
                            return null;
                    }
                    else
                        return null;

                    while (Tools.IsWhiteSpace(state.Code[i]))
                        i++;

                    Expression initializer = null;

                    if (state.Code[i] == '(')
                    {
                        i = s;
                        initializer = FunctionDefinition.Parse(state, ref i, asterisk ? FunctionKind.MethodGenerator : FunctionKind.Method);
                    }
                    else
                    {
                        if (asterisk)
                            ExceptionHelper.ThrowSyntaxError("Unexpected token", state.Code, i);

                        if (state.Code[i] != ':' && state.Code[i] != ',' && state.Code[i] != '}')
                            ExceptionHelper.ThrowSyntaxError("Expected ',', ';' or '}'", state.Code, i);

                        Expression aei = null;
                        if (flds.TryGetValue(fieldName, out aei))
                        {
                            if (state.Strict ? (!(aei is Constant) || (aei as Constant).value != JSValue.undefined) : aei is PropertyPair)
                                ExceptionHelper.ThrowSyntaxError("Try to redefine field \"" + fieldName + "\"", state.Code, s, i - s);

                            state.Message?.Invoke(MessageLevel.Warning, i, 0, "Duplicate key \"" + fieldName + "\"");
                        }

                        if (state.Code[i] == ',' || state.Code[i] == '}')
                        {
                            if (!Parser.ValidateName(fieldName, 0))
                                ExceptionHelper.ThrowSyntaxError("Invalid variable name", state.Code, i);

                            initializer = new Variable(fieldName, state.LexicalScopeLevel);
                        }
                        else
                        {
                            do
                                i++;
                            while (Tools.IsWhiteSpace(state.Code[i]));
                            initializer = ExpressionTree.Parse(state, ref i, false, false);
                        }
                    }
                    flds[fieldName] = initializer;
                }

                while (Tools.IsWhiteSpace(state.Code[i]))
                    i++;

                if ((state.Code[i] != ',') && (state.Code[i] != '}'))
                    return null;
            }

            i++;
            var pos = index;
            index = i;
            return new ObjectDefinition(flds, computedProperties.ToArray())
            {
                Position = pos,
                Length = index - pos
            };
        }

        public override JSValue Evaluate(Context context)
        {
            var res = JSObject.CreateObject();
            if (FieldNames.Length == 0 && ComputedProperties.Length == 0)
                return res;

            res._fields = JSObject.getFieldsContainer();
            for (int i = 0; i < FieldNames.Length; i++)
            {
                var val = Values[i].Evaluate(context);
                val = val.CloneImpl(false);
                val._attributes = JsValueAttributesInternal.None;
                if (FieldNames[i] == "__proto__")
                    res.__proto__ = val._oValue as JSObject;
                else
                    res._fields[FieldNames[i]] = val;
            }

            for (var i = 0; i < ComputedProperties.Length; i++)
            {
                var key = ComputedProperties[i].Key.Evaluate(context).CloneImpl(false);
                var value = ComputedProperties[i].Value.Evaluate(context).CloneImpl(false);

                JSValue existedValue;
                Symbol symbolKey = null;
                string stringKey = null;
                if (key.Is<Symbol>())
                {
                    symbolKey = key.As<Symbol>();
                    if (res._symbols == null)
                        res._symbols = new Dictionary<Symbol, JSValue>();

                    if (!res._symbols.TryGetValue(symbolKey, out existedValue))
                        res._symbols[symbolKey] = existedValue = value;
                }
                else
                {
                    stringKey = key.As<string>();
                    if (!res._fields.TryGetValue(stringKey, out existedValue))
                        res._fields[stringKey] = existedValue = value;
                }

                if (existedValue != value)
                {
                    if (existedValue.Is(JSValueType.Property) && value.Is(JSValueType.Property))
                    {
                        var egs = existedValue.As<Core.PropertyPair>();
                        var ngs = value.As<Core.PropertyPair>();
                        egs.getter = ngs.getter ?? egs.getter;
                        egs.setter = ngs.setter ?? egs.setter;
                    }
                    else
                    {
                        if (key.Is<Symbol>())
                        {
                            res._symbols[symbolKey] = value;
                        }
                        else
                        {
                            res._fields[stringKey] = value;
                        }
                    }
                }
            }
            return res;
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            _codeContext = codeContext;

            for (var i = 0; i < Values.Length; i++)
            {
                Parser.Build(ref Values[i], 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);
            }

            for (var i = 0; i < ComputedProperties.Length; i++)
            {
                var key = ComputedProperties[i].Key;
                Parser.Build(ref key, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);

                var value = ComputedProperties[i].Value;
                Parser.Build(ref value, 2, variables, codeContext | CodeContext.InExpression, message, stats, opts);

                ComputedProperties[i] = new KeyValuePair<Expression, Expression>(key, value);
            }

            return false;
        }

        public override void Optimize(ref CodeNode _this, FunctionDefinition owner, InternalCompilerMessageCallback message, Options opts, FunctionInfo stats)
        {
            for (var i = Values.Length; i-- > 0;)
            {
                var cn = Values[i] as CodeNode;
                cn.Optimize(ref cn, owner, message, opts, stats);
                Values[i] = cn as Expression;
            }
            for (var i = 0; i < ComputedProperties.Length; i++)
            {
                var key = ComputedProperties[i].Key;
                key.Optimize(ref key, owner, message, opts, stats);

                var value = ComputedProperties[i].Value;
                value.Optimize(ref value, owner, message, opts, stats);

                ComputedProperties[i] = new KeyValuePair<Expression, Expression>(key, value);
            }
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            base.RebuildScope(functionInfo, transferedVariables, scopeBias);

            for (var i = 0; i < Values.Length; i++)
            {
                Values[i].RebuildScope(functionInfo, transferedVariables, scopeBias);
            }

            for (var i = 0; i < ComputedProperties.Length; i++)
            {
                ComputedProperties[i].Key.RebuildScope(functionInfo, transferedVariables, scopeBias);
                ComputedProperties[i].Value.RebuildScope(functionInfo, transferedVariables, scopeBias);
            }
        }

        protected internal override CodeNode[] GetChildsImpl()
        {
            return Values;
        }

        public override T Visit<T>(Visitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {
            var lastDecomposeIndex = -1;
            var lastComputeDecomposeIndex = -1;
            for (var i = 0; i < Values.Length; i++)
            {
                Values[i].Decompose(ref Values[i], result);
                if (Values[i].NeedDecompose)
                {
                    lastDecomposeIndex = i;
                }
            }
            for (var i = 0; i < ComputedProperties.Length; i++)
            {
                var key = ComputedProperties[i].Key;
                key.Decompose(ref key, result);

                var value = ComputedProperties[i].Value;
                value.Decompose(ref value, result);

                if ((key != ComputedProperties[i].Key)
                    || (value != ComputedProperties[i].Value))
                    ComputedProperties[i] = new KeyValuePair<Expression, Expression>(key, value);

                if (ComputedProperties[i].Value.NeedDecompose
                    || ComputedProperties[i].Key.NeedDecompose)
                {
                    lastComputeDecomposeIndex = i;
                }
            }

            if (lastComputeDecomposeIndex >= 0)
                lastDecomposeIndex = Values.Length;

            for (var i = 0; i < lastDecomposeIndex; i++)
            {
                if (!(Values[i] is ExtractStoredValue))
                {
                    result.Add(new StoreValue(Values[i], false));
                    Values[i] = new ExtractStoredValue(Values[i]);
                }
            }

            for (var i = 0; i < lastDecomposeIndex; i++)
            {
                Expression key = null;
                Expression value = null;

                if (!(ComputedProperties[i].Key is ExtractStoredValue))
                {
                    result.Add(new StoreValue(ComputedProperties[i].Key, false));
                    key = new ExtractStoredValue(ComputedProperties[i].Key);
                }
                if (!(ComputedProperties[i].Value is ExtractStoredValue))
                {
                    result.Add(new StoreValue(ComputedProperties[i].Value, false));
                    value = new ExtractStoredValue(ComputedProperties[i].Value);
                }
                if ((key != null)
                    || (value != null))
                    ComputedProperties[i] = new KeyValuePair<Expression, Expression>(
                        key ?? ComputedProperties[i].Key,
                        value ?? ComputedProperties[i].Value);
            }
        }

        public override string ToString()
        {
            string res = "{ ";

            for (int i = 0; i < FieldNames.Length; i++)
            {
                res += "\"" + FieldNames[i] + "\"" + " : " + Values[i];
                if (i + 1 < FieldNames.Length)
                    res += ", ";
            }

            for (int i = 0; i < ComputedProperties.Length; i++)
            {
                res += "[" + ComputedProperties[i].Key + "]" + " : " + ComputedProperties[i].Value;
                if (i + 1 < FieldNames.Length)
                    res += ", ";
            }

            return res + " }";
        }
    }
}