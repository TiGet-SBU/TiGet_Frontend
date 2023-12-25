using MediatR;
using System.Runtime.Serialization;

namespace Identity.API.Application.Commands
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
