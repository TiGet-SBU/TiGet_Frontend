using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateUserCommand : BaseCommand<UserPatientDTO>
    {

        public CreateUserCommand(
        string userName,
        string email,
        string phoneNumber,
        string password,
        string confirmPassword,
        string name,
        string lastName,
        string nationalCode,
        int userType,
        int gender,
        string medicalLicenseNumber

            )

        {
            ObjectDTO.UserName = userName;
            ObjectDTO.Email = email;
            ObjectDTO.PhoneNumber = phoneNumber;
            ObjectDTO.Password = password;
            ObjectDTO.ConfirmPassword = confirmPassword;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.NationalCode = nationalCode;
            ObjectDTO.UserType = userType;
            ObjectDTO.Gender = gender;
            ObjectDTO.MedicalLicenseNumber = medicalLicenseNumber;
        }
    }
}
