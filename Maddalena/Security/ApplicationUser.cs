using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Modules.Geocoding;
using Microsoft.AspNetCore.Identity;
using Mongolino;

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
