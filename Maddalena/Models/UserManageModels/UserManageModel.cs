using Maddalena.Security;
using System.Collections.Generic;
using System.Linq;

namespace Maddalena.Models.UserManageModels
{
    public class UserManageModel
    {
        public Dictionary<string,int> RoleCount { get; set; }

        public IQueryable<ApplicationUser> Users { get; set; }
    }
}
