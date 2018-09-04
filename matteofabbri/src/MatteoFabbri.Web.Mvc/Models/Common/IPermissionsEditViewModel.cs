using System.Collections.Generic;
using MatteoFabbri.Roles.Dto;

namespace MatteoFabbri.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}