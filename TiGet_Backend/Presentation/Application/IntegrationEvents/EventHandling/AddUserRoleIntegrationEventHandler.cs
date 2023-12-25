using System.Threading.Tasks;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Serilog.Context;
using Rhazes.Services.Identity.API.IntegrationEvents.Events;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using System.Collections.Generic;
using System;
using Rhazes.Services.Identity.Domain.Seedwork;
using System.Threading;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class AddUserRoleIntegrationEventHandler : IIntegrationEventHandler<AddUserRoleIntegrationEvent>
    {

        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AddUserRoleIntegrationEventHandler(
            ApplicationUserManager userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork
            )
        {

            _userManager = MethodParameterChecker.CheckUp(userManager);
            _roleManager = MethodParameterChecker.CheckUp(roleManager);
            _unitOfWork = MethodParameterChecker.CheckUp(unitOfWork);
        }

        public async Task Handle(AddUserRoleIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {

                var user = await _userManager.FindByIdAsync(@event.UserId.ToString());
                user.CurrentTenantId = @event.TenantId;

                var userRoles = await _userManager.GetRolesAsync(user, new List<Guid>() { @event.TenantId });
                if (!userRoles.Select(x => x.Name.ToUpper()).Contains("BASIC"))
                {
                    if (user.UserRoles == null)
                        user.UserRoles = new List<ApplicationUserRole>();
                    var roleBasic = await _roleManager.FindByNameAsync("BASIC");
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                }

                if (!userRoles.Select(x => x.Name.ToUpper()).Contains(@event.Role))
                {
                    if (user.UserRoles == null)
                        user.UserRoles = new List<ApplicationUserRole>();
                    var role = await _roleManager.FindByNameAsync(@event.Role);
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = role.Id });

                }
                await _userManager.UpdateAsync(user);

            }
        }
    }
}

