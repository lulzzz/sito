using System;
using Microsoft.AspNetCore.Authorization;

namespace Maddalena.Security.Scope
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public Func<ApplicationUser,bool> Scope { get; }

        public HasScopeRequirement(Func<ApplicationUser, bool> scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}
