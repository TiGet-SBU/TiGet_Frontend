using Rhazes.BuildingBlocks.Common.Permissions;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateRoleCommand : BaseCommand<RoleDTO>
    {

        public CreateRoleCommand(
        string name,
        Guid tenantId,
        List<SelectListItem> permissionsList
            )

        {
            ObjectDTO.Name = name;
            ObjectDTO.TenantId = tenantId;
            ObjectDTO.PermissionsList = permissionsList;
        }
    }
}
