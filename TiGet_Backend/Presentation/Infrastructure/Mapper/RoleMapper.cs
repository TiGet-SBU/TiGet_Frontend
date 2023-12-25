using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using System;
using System.Linq;

namespace Rhazes.Services.Identity.API.Infrastructure.Mapper
{
    public class RoleMapper : BaseMapper<ApplicationRole, RoleDTO>
    {
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;

        public RoleMapper(
             IIdentityService identityService,
             IPermissionService permissionService
            )
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
        }

        public override RoleDTO ToDto(ApplicationRole entity)
        {
            return new RoleDTO()
            {
                Id = entity.Id,
                Name = entity.NormalizedName,
                TenantId = entity.TenantId,
                Permissions = entity.Permissions,
                PermissionsNameList = entity.XmlPermission != null ? _permissionService.GetUserPermissionsAsList(entity.XmlPermission).ToList() : null
            };
        }

        public override ApplicationRole ToEntity(RoleDTO dto)
        {
            var now = DateTime.Now;
            ApplicationRole res;
            var id = (Guid.Empty.Equals(dto.Id) || dto.Id == null) ? Guid.NewGuid() : dto.Id.Value;
            var currentUserId = _identityService.GetUserId() == null ? id : _identityService.GetUserId().Value;
            
            res = new ApplicationRole()
            {
                Id = id,
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper(),
                TenantId = dto.TenantId,
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                XmlPermission = dto.PermissionsList != null || !dto.PermissionsList.Any() ? _permissionService.GetPermissionsAsXml(dto.PermissionsList.Where(c => c.Checked == true).Select(c => c.Id).ToArray()) : null,
                CreateById = currentUserId,
                CreateDate = now,
                ModifiedById = currentUserId,
                ModifiedDate = now,
                Deleted = false,
            };

            return res;
        }
    }
}
