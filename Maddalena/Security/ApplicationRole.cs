using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Security
{
    public class ApplicationRole : Microsoft.AspNetCore.Identity.MongoDB.IdentityRole
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
