using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Serilog.Context;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class SendChangePhoneNumberTokenIntegrationEventHandler : IIntegrationEventHandler<SendChangePhoneNumberTokenIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SendChangePhoneNumberTokenIntegrationEventHandler> _logger;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public SendChangePhoneNumberTokenIntegrationEventHandler(
            IMediator mediator,
             ILogger<SendChangePhoneNumberTokenIntegrationEventHandler> logger,
             IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _logger = MethodParameterChecker.CheckUp(logger);
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
        }

        public async Task Handle(SendChangePhoneNumberTokenIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
                var command = new GenerateChangePhoneNumberTokenCommand(@event.UserId, @event.PhoneNumber);
                var token = await _mediator.Send(command);

                var verifyLookupIntegrationEvent = new VerifyLookupIntegrationEvent(@event.PhoneNumber, token);
                await _identityIntegrationEventService.AddAndSaveEventAsync(verifyLookupIntegrationEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(verifyLookupIntegrationEvent);
            }
        }
    }
}
