using System;
using Maddalena.Core.Javascript.Core;

namespace Maddalena.Core.Javascript.Expressions
{
    [Serializable]
    internal sealed class EntityReference : VariableReference
    {
        public EntityDefinition Entity => (EntityDefinition)Descriptor.initializer;

        public override string Name => Entity.Name;

        public override JSValue Evaluate(Context context)
        {
            throw new InvalidOperationException();
        }

        public EntityReference(EntityDefinition entityDefinition)
        {
            ScopeLevel = 1;
            _descriptor = new VariableDescriptor(entityDefinition.Name, 1)
            {
                lexicalScope = !entityDefinition.Hoist,
                initializer = entityDefinition
            };
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
}