using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class VerifyChangePhoneNumber2TokenCommand : BaseCommand<bool>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }

        public VerifyChangePhoneNumber2TokenCommand(
            Guid userid,
            string token,
            string phoneNumber
            )
        {
            UserId = userid;
            Token = token;
            PhoneNumber = phoneNumber;
        }
    }
}
