using System;
using Rhazes.BuildingBlocks.EventBus.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record SendChangePhoneNumberTokenIntegrationEvent : IntegrationEvent
    {
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }

        public SendChangePhoneNumberTokenIntegrationEvent(Guid userId, string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            UserId = userId;
        }
    }
}
