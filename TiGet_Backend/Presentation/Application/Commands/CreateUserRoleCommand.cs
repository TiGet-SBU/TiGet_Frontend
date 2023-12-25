using MediatR;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateUserRoleCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; }

        public CreateUserRoleCommand(Guid userId, List<string> roles)
        {
            UserId = userId;
            Roles = roles;
        }
    }
}
