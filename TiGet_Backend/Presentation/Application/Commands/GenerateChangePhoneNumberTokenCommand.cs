using System;
using MediatR;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class GenerateChangePhoneNumberTokenCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }

        public GenerateChangePhoneNumberTokenCommand(Guid userId, string phoneNumber)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }
    }
}
