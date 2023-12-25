using System;
using System.Runtime.Serialization;
using Identity.API.Application.DTO;
using Identity.Infrastructure;

namespace Identity.API.Application.Commands
{
    [DataContract]
    public class AddOrUpdateUserCommand : BaseCommand<UserPatientDTO>
    {
        public AddOrUpdateUserCommand(
        Guid id,
        string userName,
        string email,
        string phoneNumber,
        string name,
        string lastName,
        string password,
        string confirmPassword,
        int userType,
        bool phoneNumberConfirmed,
        bool irimcConfirmed,
        bool finalConfirmed
        )
        {
            ObjectDTO.Id = id;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.UserName = userName;
            ObjectDTO.Email = email;
            ObjectDTO.PhoneNumber = phoneNumber;
            ObjectDTO.PhoneNumberConfirmed = phoneNumberConfirmed;
            ObjectDTO.Password = password;
            ObjectDTO.ConfirmPassword = confirmPassword;
            ObjectDTO.UserType = userType;
            ObjectDTO.IrimcConfirmed = irimcConfirmed;
            ObjectDTO.FinalConfirmed = finalConfirmed;
        }
    }
}
