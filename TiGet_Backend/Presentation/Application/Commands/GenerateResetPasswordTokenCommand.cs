using MediatR;
using System.Runtime.Serialization;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class GenerateResetPasswordTokenCommand: IRequest<string>
    {
        public ApplicationUser User { get; set; }
        public GenerateResetPasswordTokenCommand(ApplicationUser user)
        {
            User = user;
        }
    }
}
