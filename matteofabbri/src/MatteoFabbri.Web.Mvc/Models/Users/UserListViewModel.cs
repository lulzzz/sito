using System.Collections.Generic;
using MatteoFabbri.Roles.Dto;
using MatteoFabbri.Users.Dto;

namespace MatteoFabbri.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
