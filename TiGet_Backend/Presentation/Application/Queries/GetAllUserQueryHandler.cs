using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using System.Linq;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserPatientDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;

        public GetAllUserQueryHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);

        }

        public async Task<List<UserPatientDTO>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAllAsync();

            return _mapper.ToDtos(result).ToList();
        }
    }
}

