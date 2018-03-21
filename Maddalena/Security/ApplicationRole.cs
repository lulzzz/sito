namespace Maddalena.Security
{
    public class ApplicationRole : Microsoft.AspNetCore.Identity.MongoDB.IdentityRole
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
