using MediatR;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CheckIrimcMemberInfoCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string PhoneNumber { get; set; }


        public CheckIrimcMemberInfoCommand(
            Guid userId,
            string medicalLicenseNumber,
            string phoneNumber
            )
        {
            UserId = userId;
            MedicalLicenseNumber = medicalLicenseNumber;
            PhoneNumber = phoneNumber;
        }
    }
}
