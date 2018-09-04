using System.Collections.Generic;
using MatteoFabbri.Roles.Dto;

namespace MatteoFabbri.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }

        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
