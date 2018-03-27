using System;
using Microsoft.AspNetCore.Authorization;

namespace Maddalena.Security.Scope
{
    public class DymanicScopeRequirement : IAuthorizationRequirement
    {
        public Func<ApplicationUser,bool> Scope { get; }

        public DymanicScopeRequirement(Func<ApplicationUser, bool> scope)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}
