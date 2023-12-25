using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetByNationalCodesQuery : BaseCommand<List<UserPatientDTO>>
    {
        public List<string> NationalCodes { get; set; }

        public GetByNationalCodesQuery(List<string> nationalCodes)
        {
            NationalCodes = nationalCodes;
        }
    }
}
