using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetStateByIdQueryHandler : IRequestHandler<GetStateByIdQuery, StateDTO>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMediator _mediator;

        public GetStateByIdQueryHandler(IStateRepository stateRepository, IMediator mediator)
        {
            _stateRepository = MethodParameterChecker.CheckUp(stateRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<StateDTO> Handle(GetStateByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _stateRepository.GetByIdAsync(Guid.Parse(request.Id));

            return result != null ? StateDTO.FromState(result) : null;
        }
    }


    



}

