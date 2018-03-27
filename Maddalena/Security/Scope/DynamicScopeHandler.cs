using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Maddalena.Security.Scope
{
    public class DynamicScopeHandler : AuthorizationHandler<DymanicScopeRequirement>
    {
        readonly UserManager<ApplicationUser> _userManager;

        public DynamicScopeHandler(UserManager<ApplicationUser>
            userManager)
        {
            _userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DymanicScopeRequirement requirement)
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
