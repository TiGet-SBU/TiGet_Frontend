using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserExistByNationalCodeQueryHandler : IRequestHandler<GetUserExistByNationalCodeQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public GetUserExistByNationalCodeQueryHandler(IUserRepository userRepository)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }
        public Task<bool> Handle(GetUserExistByNationalCodeQuery request, CancellationToken cancellationToken)
        {
            return _userRepository.GetExistByNationalCode(request.NationalCode);
        }
    }
}
