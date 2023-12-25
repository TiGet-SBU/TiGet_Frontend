using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetCityByIdQuery : BaseCommand<CityDTO>
    {
        public Guid Id { get; set; }
    }

}
