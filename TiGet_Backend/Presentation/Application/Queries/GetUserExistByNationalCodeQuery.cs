using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserExistByNationalCodeQuery : BaseCommand<bool>
    {
        public string NationalCode { get; set; }

        public GetUserExistByNationalCodeQuery(string nationalCode)
        {
            NationalCode = nationalCode;
        }
    }
}
