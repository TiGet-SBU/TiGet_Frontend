using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.BuildingBlocks.Common.Permissions;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetRolesByTenantIdQueryHandler : IRequestHandler<GetRolesByTenantIdQuery, List<RoleDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper<ApplicationRole, RoleDTO> _mapper;

        public GetRolesByTenantIdQueryHandler(IRoleRepository roleRepository, IMapper<ApplicationRole, RoleDTO> mapper)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<List<RoleDTO>> Handle(GetRolesByTenantIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _roleRepository.GetAllAsync(request.TenantId);

            if (result != null)
            {
                var roles = _mapper.ToDtos(result);



                foreach (var role in roles)
                {

                    var collection = AssignableToRolePermissions.GetAsSelectListItems().ToList();
                    foreach (var item in collection)
                    {
                        item.Checked = (role.PermissionsNameList != null && role.PermissionsNameList.Count() > 0) ? role.PermissionsNameList.Contains(item.Id) : false;
                    }
                    role.PermissionsList = collection;
                }

                return roles.ToList();
            }
            else
                return null;
        }
    }


}

