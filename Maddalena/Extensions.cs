using System.Security.Claims;
using System.Threading.Tasks;
using Maddalena.Security;

namespace Maddalena
{
    public static class Extensions2
    {
        public static async Task<ApplicationUser> ToUser(this ClaimsPrincipal claim)
        {
            if (claim?.Identity?.Name == null) return null;

            return await ApplicationUser.FirstOrDefaultAsync(x => x.UserName == claim.Identity.Name);
        }
    }
}
