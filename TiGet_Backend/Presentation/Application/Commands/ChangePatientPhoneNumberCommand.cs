using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class ChangePatientPhoneNumberCommand : IRequest<bool>
    {
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid ModifiedById { get; set; }
        public ChangePatientPhoneNumberCommand(string nationalCode, string phoneNumber,Guid modifiedById)
        {
            NationalCode = nationalCode;
            PhoneNumber = phoneNumber;
            ModifiedById = modifiedById;
        }
    }
}
