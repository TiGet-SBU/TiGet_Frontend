using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.BuildingBlocks.Common.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Services;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleByIdDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IIdentityService _identityService;
        private readonly IPermissionService _permissionService;
        private readonly IMediator _mediator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository, IIdentityService identityService, IPermissionService permissionService, IMediator mediator)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<RoleByIdDTO> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _roleRepository.GetByIdAsync(Guid.Parse(request.roleId));

               

            if (result != null)
            {
                var role = RoleByIdDTO.FromApplicationRole(result);

                if (role.Permissions != null)
                    role.PermissionsList = _permissionService.GetDescriptions(role.XmlPermission);

                role.PermissionNames = _permissionService.GetUserPermissionsAsList(role.XmlPermission).ToArray();

                var permissions = AssignableToRolePermissions.GetAsSelectListItems();

                var selectListItems = permissions as IList<SelectListItem> ?? permissions.ToList();
                if (role.PermissionNames != null)
                {
                    selectListItems.ToList().ForEach(
                        a => a.Checked = role.PermissionNames.Any(s => s == a.Id));
                }

                role.PermissionListItems = selectListItems;

                return role;
            }
            else
                return null;
        }
    }


    public class RoleByIdDTO
    {

        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// دسترسی های گروه کاربری
        /// </summary>
        public string[] PermissionNames { get; set; }

        /// <summary>
        /// دسترسی ها
        /// </summary>
        public IEnumerable<string> PermissionsList { get; set; }

        /// <summary>
        /// لیست دسترسی های گروه کاربری
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// لیست دسترسی ها به صورت لیست آبشاری
        /// </summary>
        public IEnumerable<SelectListItem> PermissionListItems { get; set; }


        /// <summary>
        ///ساختار ایکس ام ال لیست دسترسی های گروه کاربری
        /// </summary>
        public XElement XmlPermission
        {
            get { return XElement.Parse(Permissions); }
        }

        public static RoleByIdDTO FromApplicationRole(ApplicationRole applicationRole)
        {
            var role = new RoleByIdDTO()
            {
                Id = applicationRole.Id.ToString(),
                Name = applicationRole.NormalizedName,
                Permissions = applicationRole.Permissions
            };
            return role;
        }

    }
}

