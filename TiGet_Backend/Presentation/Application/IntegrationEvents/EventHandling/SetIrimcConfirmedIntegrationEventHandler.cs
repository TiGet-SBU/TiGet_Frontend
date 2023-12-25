using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class SetIrimcConfirmedIntegrationEventHandler : IIntegrationEventHandler<SetIrimcConfirmedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public SetIrimcConfirmedIntegrationEventHandler(
             IMediator mediator
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }
        public async Task Handle(SetIrimcConfirmedIntegrationEvent @event)
        {
            SetIrimcConfirmedCommand setIrimcConfirmedCommand = new SetIrimcConfirmedCommand(@event.UserId, @event.IrimcConfirmed);
            await _mediator.Send(setIrimcConfirmedCommand);
        }
    }
}
