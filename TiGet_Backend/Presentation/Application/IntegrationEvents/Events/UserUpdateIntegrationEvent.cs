
using Rhazes.BuildingBlocks.EventBus.Events;
using System;

namespace Rhazes.Services.Identity.API.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record UserUpdateIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public int UserType { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string UserName { get; set; }
        public Guid ModifiedById { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserUpdateIntegrationEvent(
        Guid userId,
        int userType,
        string name,
        string lastName,
        string nationalCode,
        string userName,
        Guid modifiedById,
        string phoneNumber,
        bool phoneNumberConfirmed,
        string email,
        string password,
        string confirmPassword
            )

        {
            UserId = userId;
            UserType = userType;
            Name = name;
            LastName = lastName;
            NationalCode = nationalCode;
            UserName = userName;
            ModifiedById = modifiedById;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumberConfirmed;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
