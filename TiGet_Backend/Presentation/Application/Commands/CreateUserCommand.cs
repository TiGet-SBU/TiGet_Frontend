using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateUserCommand : BaseCommand<UserPatientDTO>
    {

        public CreateUserCommand(
        string email,
        string password,
        string name,
        string lastName,

            )

        {
            ObjectDTO.Email = email;
            ObjectDTO.Password = password;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
        }
    }
}
