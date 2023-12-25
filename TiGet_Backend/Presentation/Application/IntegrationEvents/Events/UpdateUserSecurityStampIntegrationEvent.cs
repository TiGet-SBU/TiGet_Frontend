using Rhazes.BuildingBlocks.EventBus.Events;
using System;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record UpdateUserSecurityStampIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public UpdateUserSecurityStampIntegrationEvent (
            Guid userId
            )
        {
            UserId = userId;
        }
    }
}
