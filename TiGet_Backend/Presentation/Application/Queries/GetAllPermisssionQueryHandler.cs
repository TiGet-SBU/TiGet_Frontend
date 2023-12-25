using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.BuildingBlocks.Common.Permissions;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllPermisssionQueryHandler : IRequestHandler<GetAllPermissionQuery, List<PermissionDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;
        private readonly IMediator _mediator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public GetAllPermisssionQueryHandler(IRoleRepository roleRepository, IIdentityService identityService, IPermissionService permissionService, IMediator mediator)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<PermissionDTO>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            var result = AssignableToRolePermissions.GetAsSelectListItems().ToList();

            return result != null ? PermissionDTO.FromApplicationPermission(result) : null;
        }
    }


    public class PermissionDTO
    {
        public string Id { get; set; }
        public bool Disabled { get; set; }

        public string Group { get; set; }

        public bool Checked { get; set; }

        public string Name { get; set; }

        public static List<PermissionDTO> FromApplicationPermission(IList<SelectListItem> applicationPermissions)
        {
            List<PermissionDTO> permissions = new List<PermissionDTO>();
            foreach (var permission in applicationPermissions)
            {
                permissions.Add(new PermissionDTO()
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Group = permission.Group,
                    Checked = permission.Checked,
                    Disabled = permission.Disabled
                });
            }
            return permissions;
        }

    }
}

