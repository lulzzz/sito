using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Maddalena
{
    public class Acl
    {
        public bool Public { get; set; }

        public List<string> AllowUsers { get; set; }

        public List<string> AllowRoles { get; set; }

        public List<string> DenyUsers { get; set; }

        public List<string> DenyRoles { get; set; }

        public bool IsAllowed(ClaimsPrincipal claim)
        {
            if (Public || claim.IsInRole("admin")) return true;

            if (DenyUsers.Contains(claim.Identity.Name)) return false;

            if (AllowUsers.Contains(claim.Identity.Name)) return true;

            return !DenyRoles.Any(claim.IsInRole) && AllowRoles.Any(claim.IsInRole);
        }
    }
}