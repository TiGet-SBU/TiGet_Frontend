using Rhazes.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    public record CheckIrimcMemberInfoIntegrationEvent : IntegrationEvent
    {
        public string NationalCode { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }

        public CheckIrimcMemberInfoIntegrationEvent(
            string nationalCode,
            string medicalLicenseNumber,
            string firstName,
            string lastName,
            string phoneNumber,
            Guid userId
            )
        {
            NationalCode = nationalCode;
            MedicalLicenseNumber = medicalLicenseNumber;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            UserId = userId;
        }
    }
}
