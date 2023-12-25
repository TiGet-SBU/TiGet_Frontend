using MediatR;
using Rhazes.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllUserPagingQuery : DataSourceRequest, IRequest<DataSourceResult>
    {
    }

}
