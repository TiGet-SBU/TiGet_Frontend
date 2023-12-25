using Rhazes.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record ChangePatientPhoneNumberIntegrationEvent : IntegrationEvent
    {
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ModifiedById { get; set; }

        public ChangePatientPhoneNumberIntegrationEvent(
            string nationalCode,
            string phoneNumber,
            Guid modifiedById
            )
        {
            NationalCode = nationalCode;
            PhoneNumber = phoneNumber;
            ModifiedById = modifiedById;
        }
    }
}
