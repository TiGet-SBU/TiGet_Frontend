using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class ResetPasswordCommand : BaseCommand<bool>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }

        public ResetPasswordCommand(
            Guid userId,
            string token,
            string password
            )
        {
            UserId = userId;
            Token = token;
            Password = password;
        }
    }
}
