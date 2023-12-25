using System;
using System.Collections.Generic;
using Rhazes.BuildingBlocks.Common.Permissions;

namespace Rhazes.Services.Identity.API.Application.DTO
{
    public class UserRoleDTO
    {
        public string Name { get; set; }
        public bool IsSystemic { get; set; }
        public bool Checked { get; set; }

    }

}
