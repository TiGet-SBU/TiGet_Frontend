using System;
using Rhazes.BuildingBlocks.EventBus.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record ChangeStaffPhoneNumberIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }

        public ChangeStaffPhoneNumberIntegrationEvent(
            Guid userId,
            string phoneNumber
            )
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }
    }
}
