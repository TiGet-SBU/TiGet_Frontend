using MediatR;
using System.Runtime.Serialization;
using Identity.Domain.AggregatesModel.UserAggregate;

namespace Identity.API.Application.Commands
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
