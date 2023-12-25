using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<UserPatientDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByIdAsync(request.userId);

            return result != null ? _mapper.ToDto(result) : null;
        }
    }
}

