using System.Threading.Tasks;
using MediatR;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.EventBus.Abstractions;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class ChangePatientPhoneNumberIntegrationEventHandler : IIntegrationEventHandler<ChangePatientPhoneNumberIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public ChangePatientPhoneNumberIntegrationEventHandler(
            IMediator mediator
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public Task Handle(ChangePatientPhoneNumberIntegrationEvent @event)
        {
            var command = new ChangePatientPhoneNumberCommand(@event.NationalCode, @event.PhoneNumber,@event.ModifiedById);
            return _mediator.Send(command);
        }
    }
}
