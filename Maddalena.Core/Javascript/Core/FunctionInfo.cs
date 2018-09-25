using System;
using System.Collections.Generic;
using Maddalena.Core.Javascript.Expressions;

namespace Maddalena.Core.Javascript.Core
{
    [Serializable]
    public sealed class FunctionInfo
    {
        public bool UseGetMember;
        public bool UseCall;
        public bool WithLexicalEnvironment;
        public bool ContainsArguments;
        public bool ContainsRestParameters;
        public bool ContainsEval;
        public bool ContainsWith;
        public bool NeedDecompose;
        public bool ContainsInnerEntities;
        public bool ContainsThis;
        public bool ContainsDebugger;
        public bool ContainsTry;
        public readonly List<Expression> Returns = new List<Expression>();
        public PredictedType ResultType;
    }
}