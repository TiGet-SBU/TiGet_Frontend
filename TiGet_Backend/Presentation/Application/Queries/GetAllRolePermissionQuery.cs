using Rhazes.Services.Identity.Infrastructure;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllRolePermissionQuery : BaseCommand<List<RolePermissionDTO>>
    {
        public string roleId { get; set; }
    }

}
