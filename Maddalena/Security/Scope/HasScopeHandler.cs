using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Maddalena.Security.Scope
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        readonly UserManager<ApplicationUser> _userManager;

        public HasScopeHandler(UserManager<ApplicationUser>
            userManager)
        {
            _userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (context?.User?.Identity?.Name == null)
            {
                context.Fail();
                return;
            }

            var user = await _userManager.FindByNameAsync(context.User.Identity.Name);

            if (user == null)
            {
                context.Fail();
                return;
            }

            if(requirement.Scope(user)) context.Succeed(requirement);
        }
    }
}
