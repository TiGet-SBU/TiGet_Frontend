using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using Rhazes.BuildingBlocks.Common.Models;
using Rhazes.BuildingBlocks.Common.Permissions;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllRolePagingQueryHandler : IRequestHandler<GetAllRolePagingQuery, DataSourceResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionService _permissionService;

        public GetAllRolePagingQueryHandler(IRoleRepository roleRepository, IPermissionService permissionService)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _permissionService = MethodParameterChecker.CheckUp(permissionService);

        }

        public async Task<DataSourceResult> Handle(GetAllRolePagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _roleRepository.GetAllPagingAsync(request);

            if (result != null)
            {
                var roles = RolePagingDTO.FromApplicationRole(result);
                foreach (RolePagingDTO role in roles.Data)
                {
                    var collection = AssignableToRolePermissions.GetAsSelectListItems().ToList();

                    IEnumerable<string> rolePermissions = new List<string>();
                    if (role.XmlPermission != null)
                        rolePermissions = _permissionService.GetUserPermissionsAsList(role.XmlPermission);

                    var permissionsList = new List<SelectListItem>();

                    foreach (var item in collection)
                    {
                        item.Checked = (rolePermissions != null && rolePermissions.Count() > 0) ? rolePermissions.Contains(item.Id) : false;
                    }

                    role.PermissionsList = collection;
                }

                return roles;
            }
            else
                return null;
        }
    }


    public class RolePagingDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// دسترسی ها
        /// </summary>
        public IEnumerable<SelectListItem> PermissionsList { get; set; }

        /// <summary>
        /// لیست دسترسی های گروه کاربری
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        ///ساختار ایکس ام ال لیست دسترسی های گروه کاربری
        /// </summary>
        public XElement XmlPermission
        {
            get { return Permissions != null ? XElement.Parse(Permissions) : null; }
        }

        public static DataSourceResult FromApplicationRole(DataSourceResult dataSource)
        {
            DataSourceResult result = new DataSourceResult()
            {
                Total = dataSource.Total,
                AggregateResults = dataSource.AggregateResults
            };

            List<RolePagingDTO> roles = new List<RolePagingDTO>();
            foreach (ApplicationRole applicationRole in dataSource.Data)
            {
                roles.Add(new RolePagingDTO()
                {
                    Id = applicationRole.Id.ToString(),
                    Name = applicationRole.Name,
                    Permissions = applicationRole.Permissions
                });
            }
            result.Data = roles;

            return result;
        }
    }
}

