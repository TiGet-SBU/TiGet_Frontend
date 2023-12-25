using System;
using System.Linq;
using System.Runtime.Serialization;
using Identity.API.Application.DTO;
using Identity.Domain.AggregatesModel.UserAggregate;
using Identity.Infrastructure;

namespace Identity.API.Application.Commands
{
    [DataContract]
    public class RegisterCommand : BaseCommand<UserPatientDTO>
    {
        public RegisterCommand(
            Guid? id,
            string name,
            string lastName,
            string Email,
            string password
            )
        {
            ObjectDTO.Id = id;
            ObjectDTO.Name = name;
            ObjectDTO.LastName = lastName;
            ObjectDTO.Email = Email;
            ObjectDTO.Password = password;
        }
    }
}
