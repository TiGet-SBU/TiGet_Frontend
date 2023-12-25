using System;
using System.Runtime.Serialization;
using Identity.API.Application.DTO;
using Identity.Infrastructure;

namespace Identity.API.Application.Commands
{
    [DataContract]
    public class AddOrUpdateUserCommand : BaseCommand<UserPatientDTO>
    {
        public AddOrUpdateUserCommand(
        Guid id,
        string email,
        string name,
        string lastName,
        string password
        )
        {
            ObjectDTO.Id = id;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.Email = email;
            ObjectDTO.Password = password;
        }
    }
}
