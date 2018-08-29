using System;
using System.Collections.Generic;
using JS.Core.Core;
using NiL.JS;

namespace JS.Core.Expressions
{
#if !(PORTABLE)
    [Serializable]
#endif
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
            this._descriptor = new VariableDescriptor(entityDefinition.Name, 1)
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

    public abstract class EntityDefinition : Expression
    {
        protected bool Built { get; set; }
        public string Name { get; }
        public VariableReference Reference { get; }

        public abstract bool Hoist { get; }

        protected EntityDefinition(string name)
        {
            Name = name;
            Reference = new EntityReference(this);
        }

        public override bool Build(ref CodeNode _this, int expressionDepth, Dictionary<string, VariableDescriptor> variables, CodeContext codeContext, InternalCompilerMessageCallback message, FunctionInfo stats, Options opts)
        {
            _codeContext = codeContext;
            return false;
        }

        public abstract override void Decompose(ref Expression self, IList<CodeNode> result);

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> transferedVariables, int scopeBias)
        {
            Reference.ScopeBias = scopeBias;
            if (Reference._descriptor?.definitionScopeLevel >= 0)
            {
                Reference._descriptor.definitionScopeLevel = Reference.ScopeLevel;
                Reference._descriptor.scopeBias = scopeBias;
            }
        }
    }
}
