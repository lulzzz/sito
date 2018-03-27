using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Maddalena.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static bool HasScope(this ClaimsPrincipal claimPrincipal, string scope)
        {
            return claimPrincipal.IsInRole(scope);
        }
    }
}
