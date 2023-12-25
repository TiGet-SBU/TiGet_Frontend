using Rhazes.Services.Identity.Infrastructure;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllCityQuery : BaseCommand<List<CityDTO>>
    {
        public string StateId { get; set; }
    }

}
