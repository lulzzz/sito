using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo;
using Maddalena.Modules.Geocoding;
using Mongolino;

namespace Maddalena.Security
{
    public class ApplicationUser : DBObject<ApplicationUser>, IMongoIdentityUser
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string FamilyName { get; set; }

        public Address Address { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string AuthenticatorKey { get; set; }
        public string PasswordHash { get; set; }

        public async Task<IEnumerable<string>> GetRoles()
        {
            return (await RoleMembership.WhereAsync(x => x.UserId == Id))
                .Select(m => ApplicationRole.First(x => x.Id == m.RoleId))
                .Select(x => x.Name)
                .ToArray();
        }
    }
}