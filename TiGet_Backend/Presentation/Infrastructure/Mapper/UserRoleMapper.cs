using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;

namespace Rhazes.Services.Identity.API.Infrastructure.Mapper
{
    public class UserRoleMapper : BaseMapper<ApplicationUserRole, UserRoleDTO>
    {
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;

        public UserRoleMapper(
             IIdentityService identityService,
             IPermissionService permissionService
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
        }

        public override UserRoleDTO ToDto(ApplicationUserRole entity)
        {
            return new UserRoleDTO()
            {
                Name = entity.Role.Name,
                IsSystemic = entity.Role.IsSystemic,
                Checked = true
            };
        }

        public override ApplicationUserRole ToEntity(UserRoleDTO dto)
        {
            return null;
        }
    }
}
