using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Infrastructure;
using System.Collections.Generic;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserByListIdQuery : BaseCommand<List<UserDTO>>
    {
        public List<string> listUserId { get; set; }
    }

}
