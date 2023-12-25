
using Rhazes.BuildingBlocks.EventBus.Events;

namespace Rhazes.Services.Identity.API.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record UserDeleteIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }

        public bool DeleteLogical { get; set; }


        public UserDeleteIntegrationEvent(string userId, bool deleteLogical)
        {
            UserId = userId;
            DeleteLogical = deleteLogical;
        }
    }
}
