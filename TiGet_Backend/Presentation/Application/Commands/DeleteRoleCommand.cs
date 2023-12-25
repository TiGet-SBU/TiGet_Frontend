using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class DeleteRoleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteRoleCommand(
        Guid id
        )

        {
            Id = id;
        }
    }
}
