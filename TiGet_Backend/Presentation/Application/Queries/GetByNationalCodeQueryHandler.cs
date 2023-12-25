using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetByNationalCodeQueryHandler : IRequestHandler<GetByNationalCodeQuery, UserPatientDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;

        public GetByNationalCodeQueryHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<UserPatientDTO> Handle(GetByNationalCodeQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByNationalCode(request.NationalCode);

            return user != null ? _mapper.ToDto(user) : null;
        }
    }
}
