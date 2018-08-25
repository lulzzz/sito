using System.Security.Claims;

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
