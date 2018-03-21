using Maddalena.Modules.Geocoding;

namespace Maddalena.Security
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.MongoDB.IdentityUser
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }

        public Address Address { get; set; }
    }
}
