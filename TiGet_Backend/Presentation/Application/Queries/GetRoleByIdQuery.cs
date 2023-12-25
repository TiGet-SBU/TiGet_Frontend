using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetRoleByIdQuery : BaseCommand<RoleByIdDTO>
    {
        public string roleId { get; set; }

    }

}
