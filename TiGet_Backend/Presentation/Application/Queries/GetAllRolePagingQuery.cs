using MediatR;
using Rhazes.BuildingBlocks.Common.Models;
using Rhazes.Services.Identity.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllRolePagingQuery : DataSourceRequest, IRequest<DataSourceResult>
    {
    }

}
