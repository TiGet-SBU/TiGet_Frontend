using System;
using System.Runtime.Serialization;
using MediatR;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class VerifyTwoFactorTokenCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }

        public VerifyTwoFactorTokenCommand(
            Guid userId,
            string token
            )
        {
            UserId = userId;
            Token = token;
        }
    }
}
