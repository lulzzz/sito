using AspNetCore.Identity.Mongo;

namespace Maddalena.Security
{
    public class ApplicationRole : MongoIdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
