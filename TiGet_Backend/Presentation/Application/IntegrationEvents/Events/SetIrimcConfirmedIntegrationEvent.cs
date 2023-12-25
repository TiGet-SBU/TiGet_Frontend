using Rhazes.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record SetIrimcConfirmedIntegrationEvent: IntegrationEvent
    {
        public Guid UserId { get; set; }
        public bool IrimcConfirmed { get; set; }

        public SetIrimcConfirmedIntegrationEvent(
            Guid userId,
            bool irimcConfirmed
            )
        {
            UserId = userId;
            IrimcConfirmed = irimcConfirmed;
        }
    }
}
