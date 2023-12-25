using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class VerifyChangePhoneNumberTokenCommand: IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }

        public VerifyChangePhoneNumberTokenCommand(
            Guid userId,
            string token,
            string phoneNumber)
        {
            UserId = userId;
            Token = token;
            PhoneNumber = phoneNumber;
        }


    }
}
