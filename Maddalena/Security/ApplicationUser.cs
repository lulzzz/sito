using AspNetCore.Identity.Mongo;

namespace Maddalena.Security
{
    public class ApplicationUser : MongoIdentityUser
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }
    }
}
