using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class DeleteUserCommand : BaseCommand<UserDeleteDTO>
    {
        public DeleteUserCommand(string id)
        {
            ObjectDTO.Id = id;
        }
    }
}
