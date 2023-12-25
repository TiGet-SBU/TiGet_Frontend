using System;
using System.Runtime.Serialization;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class UpdateUserCommand : BaseCommand<UserPatientDTO>
    {
        public UpdateUserCommand(
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
