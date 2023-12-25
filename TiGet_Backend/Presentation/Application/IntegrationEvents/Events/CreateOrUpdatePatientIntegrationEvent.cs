
using Rhazes.BuildingBlocks.EventBus.Events;
using System;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record CreateOrUpdatePatientIntegrationEvent : IntegrationEvent
    {

        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public Guid UserId { get; set; }
        public Guid CreateById { get; set; }
        public Guid NationalityId { get; set; }

        public string FatherName { get; set; }
        public string Email { get; set; }
        public int? MaritalType { get; set; }
        public Guid? BirthStateId { get; set; }
        public Guid? BirthCityId { get; set; }
        public string Tel { get; set; }
        public Guid? StateId { get; set; }
        public Guid? CityId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int? BloodTypeId { get; set; }
        public int? SpouseBloodTypeId { get; set; }
        public Guid? EducationId { get; set; }
        public Guid? JobId { get; set; }

        public CreateOrUpdatePatientIntegrationEvent(
        string phoneNumber,
        string name,
        string lastName,
        string nationalCode,
        int gender,
        DateTime? birthdate,
        Guid userId,
        Guid createById,
        Guid nationalityId,
        string fatherName,
        string email,
        int? maritalType,
        Guid? birthStateId,
        Guid? birthCityId,
        string tel,
        Guid? stateId,
        Guid? cityId,
        string address,
        string zipCode,
        int? bloodTypeId,
        int? spouseBloodTypeId,
        Guid? educationId,
        Guid? jobId

            )

        {
            PhoneNumber = phoneNumber;
            Name = name;
            LastName = lastName;
            NationalCode = nationalCode;
            Gender = gender;
            Birthdate = birthdate;
            UserId = userId;
            CreateById = createById;
            NationalityId = nationalityId;
            FatherName = fatherName;
            Email = email;
            MaritalType = maritalType;
            BirthStateId = birthStateId;
            BirthCityId = birthCityId;
            Tel = tel;
            StateId = stateId;
            CityId = cityId;
            Address = address;
            ZipCode = zipCode;
            BloodTypeId = bloodTypeId;
            SpouseBloodTypeId = spouseBloodTypeId;
            EducationId = educationId;
            JobId = jobId;
        }
    }
}
