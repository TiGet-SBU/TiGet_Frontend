using Rhazes.BuildingBlocks.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents
{
    public interface IIdentityIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
