using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetByNationalCodeQuery : BaseCommand<UserPatientDTO>
    {
        public string NationalCode { get; set; }

        public GetByNationalCodeQuery(string nationalCode)
        {
            NationalCode = nationalCode;
        }
    }
}
