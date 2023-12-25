using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllStateQuery : BaseCommand<List<StateDTO>>
    {
        public string CountryId { get; set; }
    }

}
