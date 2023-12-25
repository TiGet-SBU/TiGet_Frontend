using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.BuildingBlocks.Common.Exception;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateUserSecurityStampIntegrationEventHandler: IIntegrationEventHandler<UpdateUserSecurityStampIntegrationEvent>
    {
        private readonly IApplicationUserManager _userManager;

        public UpdateUserSecurityStampIntegrationEventHandler(
            IApplicationUserManager userManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        public async Task Handle(UpdateUserSecurityStampIntegrationEvent @event)
        {

            var user = await _userManager.FindByIdAsync(@event.UserId.ToString());
            var result = await _userManager.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
            {
                throw new DomainException("USER_SECURITY_STAMP_UPDATE_FAILD");
            }
        }
    }
}
