using Abp.Authorization;
using MatteoFabbri.Authorization.Roles;
using MatteoFabbri.Authorization.Users;

namespace MatteoFabbri.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
