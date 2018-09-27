﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Core.Npm.Model
{
    public enum NpmDependecyType
    {
        Precise,
        Around,
        Lower,
        LowerOrEqual,
        Greater,
        GreaterOrEqual,
        Any,
        Forbidden
    }
}
