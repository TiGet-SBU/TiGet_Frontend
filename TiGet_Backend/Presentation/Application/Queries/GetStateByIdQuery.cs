using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetStateByIdQuery : BaseCommand<StateDTO>
    {
        public string Id { get; set; }
    }

}
