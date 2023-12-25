using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserByPhoneNumbersQuery: BaseCommand<List<UserPatientDTO>>
    {
        public IList<string> PhoneNumbers { get; set; }
      
    }
}
