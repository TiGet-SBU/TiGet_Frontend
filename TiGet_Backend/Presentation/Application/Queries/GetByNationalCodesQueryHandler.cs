using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetByNationalCodesQueryHandler : IRequestHandler<GetByNationalCodesQuery, List<UserPatientDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<ApplicationUser, UserPatientDTO> _mapper;

        public GetByNationalCodesQueryHandler(
            IUserRepository userRepository,
            IMapper<ApplicationUser, UserPatientDTO> mapper
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<List<UserPatientDTO>> Handle(GetByNationalCodesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByNationalCodes(request.NationalCodes);
            return users != null ? _mapper.ToDtos(users).ToList() : null;
        }
    }
}
