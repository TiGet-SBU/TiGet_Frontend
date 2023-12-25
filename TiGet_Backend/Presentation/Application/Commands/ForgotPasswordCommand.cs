using MediatR;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public ForgotPasswordCommand(string userName)
        {
            UserName = userName;
        }
    }
}
