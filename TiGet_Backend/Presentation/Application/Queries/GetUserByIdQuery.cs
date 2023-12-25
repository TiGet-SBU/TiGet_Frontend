using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserByIdQuery : BaseCommand<UserPatientDTO>
    {
        public Guid userId { get; set; }
    }

}
