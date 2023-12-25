
using Rhazes.BuildingBlocks.EventBus.Events;
using System;

namespace Rhazes.Services.Identity.API.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record AddUserPatientIntegrationEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public Guid CreateById { get; set; }
        public Guid NationalityId { get; set; }

      


        public AddUserPatientIntegrationEvent(
        string name,
        string lastName,
        string nationalCode,
        int gender,
        string phoneNumber,
        DateTime? birthdate,
        Guid createById,
        Guid nationalityId
            )

        {
            Name = name;
            LastName = lastName;
            NationalCode = nationalCode;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Birthdate = birthdate;
            CreateById = createById;
            NationalityId = nationalityId;
        }
    }
}
