using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Mongo;

namespace Maddalena
{
    public static class Extensions
    {
        public static async Task<MongoIdentityUser> ToUser(this ClaimsPrincipal claim)
        {
            if (claim?.Identity?.Name == null) return null;

            return await MongoIdentityUser.FirstOrDefaultAsync(x => x.UserName == claim.Identity.Name);
        }
    }
}
