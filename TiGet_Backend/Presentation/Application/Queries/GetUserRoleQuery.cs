using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserRoleQuery : BaseCommand<List<UserRoleDTO>>
    {
        public Guid userId { get; set; }
    }

}
