
using Rhazes.BuildingBlocks.EventBus.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record VerifyLookupIntegrationEvent : IntegrationEvent
    {
        public string PhoneNumber { get; set; }
        public string Token { get; set; }


        public VerifyLookupIntegrationEvent(string phoneNumber, string token)
        {
            PhoneNumber = phoneNumber;
            Token = token;
        }
    }
}
