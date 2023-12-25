using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllRegisteriesQueryHandler : IRequestHandler<GetAllRegisteriesQuery, List<RegistryDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper<Registry, RegistryDTO> _mapper;

        public GetAllRegisteriesQueryHandler(IUserRepository userRepository, IMapper<Registry, RegistryDTO> mapper)
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
        }

        public async Task<List<RegistryDTO>> Handle(GetAllRegisteriesQuery request, CancellationToken cancellationToken)
        {
            var query = await _userRepository.GetAllRegisteryAsync();

            return new List<RegistryDTO>(_mapper.ToDtos(query));
        }



    }
    public class RegistryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

    }
}
