using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.IntegrationEvents;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class CheckIrimcMemberInfoCommandHandler : IRequestHandler<CheckIrimcMemberInfoCommand>
    {
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;
        private readonly IUserRepository _userRepository;

        public CheckIrimcMemberInfoCommandHandler(
            IIdentityIntegrationEventService identityIntegrationEventService,
            IUserRepository userRepository
            )
        {
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<Unit> Handle(CheckIrimcMemberInfoCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);

            CheckIrimcMemberInfoIntegrationEvent checkIrimcMemberInfoIntegrationEvent = new CheckIrimcMemberInfoIntegrationEvent(user.UserName, command.MedicalLicenseNumber, user.Name, user.LastName, command.PhoneNumber, command.UserId);
            await _identityIntegrationEventService.AddAndSaveEventAsync(checkIrimcMemberInfoIntegrationEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(checkIrimcMemberInfoIntegrationEvent);

            return Unit.Value;
        }
    }
}
