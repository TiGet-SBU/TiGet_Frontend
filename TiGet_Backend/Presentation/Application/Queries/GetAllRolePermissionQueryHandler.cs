using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.BuildingBlocks.Common.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllRolePermissionQueryHandler : IRequestHandler<GetAllRolePermissionQuery, List<RolePermissionDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;
        private readonly IMediator _mediator;

        public GetAllRolePermissionQueryHandler(IRoleRepository roleRepository, IIdentityService identityService, IPermissionService permissionService, IMediator mediator)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<RolePermissionDTO>> Handle(GetAllRolePermissionQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string> rolePermissions = new List<string>();
            var applicationPermissions = AssignableToRolePermissions.GetAsSelectListItems().ToList();

            if (!string.IsNullOrEmpty(request.roleId) && request.roleId != "undefined")
            {
                var role = await _roleRepository.GetByIdAsync(Guid.Parse(request.roleId));

                if (role.XmlPermission != null)
                    rolePermissions = _permissionService.GetUserPermissionsAsList(role.XmlPermission);
            }

            return applicationPermissions != null ? RolePermissionDTO.FromApplicationUser(rolePermissions, applicationPermissions) : null;
        }
    }


    public class RolePermissionDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Group { get; set; }
        public bool Disabled { get; set; }

        public bool Checked { get; set; }

        public static List<RolePermissionDTO> FromApplicationUser(IEnumerable<string> rolePermissions, List<SelectListItem> applicationPermissions)
        {
            List<RolePermissionDTO> roles = new List<RolePermissionDTO>();

            foreach (var item in applicationPermissions)
            {
                if (rolePermissions != null && rolePermissions.Contains(item.Id))
                {
                    roles.Add(new RolePermissionDTO()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Disabled = item.Disabled,
                        Group = item.Group,
                        Checked = true
                    });
                }
                else
                {
                    roles.Add(new RolePermissionDTO()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Disabled = item.Disabled,
                        Group = item.Group,
                        Checked = false
                    });
                }

            }
            return roles;
        }

    }


}

