using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQuery, List<UserRoleDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public GetUserRoleQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository, IIdentityService identityService, IMediator mediator)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<UserRoleDTO>> Handle(GetUserRoleQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetByIdAsync(_identityService.GetUserId().Value);

            var applicationRoles = (await _roleRepository.GetQueryable())
                .Where(x => x.TenantId == currentUser.CurrentTenantId || x.IsSystemic).ToList();
            var user = await _userRepository.GetByIdAsync(request.userId);
            var result = await _userRepository.GetAllUserRolesAsync(user, currentUser.CurrentTenantId);

            return applicationRoles != null ? FromApplicationUserRole(result, applicationRoles) : null;
        }

        public static List<UserRoleDTO> FromApplicationUserRole(IList<string> roleList, List<ApplicationRole> applicationRoles)
        {
            List<UserRoleDTO> roles = new List<UserRoleDTO>();

            foreach (var item in applicationRoles)
            {
                if (item.Name == "Basic")
                    continue;

                if (roleList.Contains(item.Name))
                {
                    roles.Add(new UserRoleDTO()
                    {
                        Name = item.NormalizedName,
                        IsSystemic = item.IsSystemic,
                        Checked = true
                    });
                }
                else
                {
                    if (item.IsSystemic)
                        continue;

                    roles.Add(new UserRoleDTO()
                    {
                        Name = item.NormalizedName,
                        IsSystemic = item.IsSystemic,
                        Checked = false
                    });
                }

            }
            return roles;
        }
    }







}

