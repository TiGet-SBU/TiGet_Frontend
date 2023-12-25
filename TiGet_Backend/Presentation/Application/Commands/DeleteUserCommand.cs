using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class DeleteLogicalUserCommand : BaseCommand<UserDeleteLogicalDTO>
    {
        public string Id { get; set; }

        public DeleteLogicalUserCommand(string id)
        {
            Id = id;
        }
    }
}
