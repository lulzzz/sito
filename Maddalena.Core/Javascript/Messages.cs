﻿namespace Maddalena.Core.Javascript
{
    class Messages
    {
        public const string CannotAssignReadOnly = "Can not assign to readonly property {0}";
        public const string ConstructorCannotBeStatic = "Constructor cannot be static";
        public const string FunctionInLoop = "Do not define a function inside a loop";
        public const string IdentifierAlreadyDeclared = "Identifier {0} has already been declared";
        public const string IncrementPropertyWOSetter = "Can not increment property {0} without setter.";
        public const string IncrementReadonly = "Can not increment readonly {0}";
        public const string InvalidLefthandSideInAssignment = "Invalid left-hand side in assignment.";
        public const string InvalidPropertyName = "Invalid property name at {0}";
        public const string InvalidRegExp = "Unable to process regular expression {0}";
        public const string InvalidTryToCallWithNew = "Method {0} can not be called with new keyword";
        public const string InvalidTryToCallWithoutNew = "Method {0} can not be called without new keyword";
        public const string InvalidTryToCreateWithoutNew = "Type {0} can not be created with new keyword";

        public const string TooManyArgumentsForFunction = "Type {0} can not be created without new keyword";
        public const string TryingToGetProperty = "Too many arguments for function {0}";
        public const string TryingToSetProperty = "Can't get property {0} of {1}";
        public const string UnexpectedToken = "Can't set property {0} of {1}";
        public const string UnknowIdentifier = "Unknown identifier {0} at {1}";
        public const string VariableNotDefined = "Variable {0} is not defined";

    }
}
