using System.Threading.Tasks;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Serilog.Context;
using Rhazes.Services.Identity.API.IntegrationEvents.Events;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class UserUpdateIntegrationEventHandler : IIntegrationEventHandler<UserUpdateIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public UserUpdateIntegrationEventHandler(
            IMediator mediator,
            IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
        }

        public async Task Handle(UserUpdateIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                var updateUserCommand = new UpdateUserCommand(
                    @event.UserId,
                    @event.UserName,
                    @event.Email,
                    @event.PhoneNumber,
                    @event.Name,
                    @event.LastName,
                    @event.Password,
                    @event.ConfirmPassword,
                    @event.UserType,
                    phoneNumberConfirmed: false,
                    irimcConfirmed: false,
                    finalConfirmed: false
                    );

                var result = await _mediator.Send(updateUserCommand);

                var sendTokenEvent = new SendChangePhoneNumberTokenIntegrationEvent(updateUserCommand.ObjectDTO.Id.Value, updateUserCommand.ObjectDTO.PhoneNumber);
                await _identityIntegrationEventService.AddAndSaveEventAsync(sendTokenEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(sendTokenEvent);

            }
        }
    }
}