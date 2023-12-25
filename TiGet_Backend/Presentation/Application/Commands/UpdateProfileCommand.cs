using System;
using System.Runtime.Serialization;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class UpdateProfileCommand : BaseCommand<UserPatientDTO>
    {
        public UpdateProfileCommand(
        string email,
        string medicalLicenseNumber,
        int userType
        )
        {
            ObjectDTO.Email = email;
            ObjectDTO.MedicalLicenseNumber = medicalLicenseNumber;
            ObjectDTO.UserType = userType;
        }
    }
}
