using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.BaseLibrary;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
    [Serializable]
    public sealed class PropertyPair : Expression
    {
        public Expression Getter
        {
            get => _left;
            internal set => _left = value;
        }

        public Expression Setter
        {
            get => _right;
            internal set => _right = value;
        }

        protected internal override bool ContextIndependent => false;

        public PropertyPair(Expression getter, Expression setter)
            : base(getter, setter, true)
        {
            _tempContainer._valueType = JSValueType.Property;
        }
        
        public override JSValue Evaluate(Context context)
        {
            _tempContainer._oValue = new Core.PropertyPair
            (
                (Function) Getter?.Evaluate(context),
                (Function) Setter?.Evaluate(context)
            );
            return _tempContainer;
        }

        public override void Decompose(ref Expression self, IList<CodeNode> result)
        {
            throw new InvalidOperationException();
        }
    }
}
