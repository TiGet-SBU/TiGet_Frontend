using System;
using System.Linq;
using System.Runtime.Serialization;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class RegisterCommand : BaseCommand<UserPatientDTO>
    {
        public RegisterCommand(
            Guid? id,
            string name,
            string lastName,
            string nationalCode,
            string phoneNumber,
            string password,
            string confirmPassword,
            int userType,
            string medicalLicenseNumber,
            int gender
            )
        {
            ObjectDTO.Id = id;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.NationalCode = nationalCode;
            ObjectDTO.PhoneNumber = phoneNumber;
            ObjectDTO.Password = password;
            ObjectDTO.ConfirmPassword = confirmPassword;
            ObjectDTO.UserType = userType;
            ObjectDTO.UserName = nationalCode;
            ObjectDTO.MedicalLicenseNumber = medicalLicenseNumber;
            ObjectDTO.Gender = gender;
        }
    }
}
