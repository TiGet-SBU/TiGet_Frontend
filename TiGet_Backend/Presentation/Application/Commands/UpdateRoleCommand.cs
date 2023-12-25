using Rhazes.BuildingBlocks.Common.Permissions;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class UpdateRoleCommand : BaseCommand<RoleDTO>
    {

        public UpdateRoleCommand(
         Guid id,
         string name,
         Guid tenant,
         List<SelectListItem> permissionsList
             )

        {
            ObjectDTO.Id = id;
            ObjectDTO.Name = name;
            ObjectDTO.TenantId = tenant;
            ObjectDTO.PermissionsList = permissionsList;
        }
    }
}
