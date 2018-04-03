using Maddalena.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maddalena.Models.UserManageModels
{
    public class UserManageModel
    {
        public Dictionary<string,int> RoleCount { get; set; }

        public IQueryable<ApplicationUser> Users { get; set; }
    }
}
