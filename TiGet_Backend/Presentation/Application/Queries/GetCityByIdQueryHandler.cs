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
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDTO>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMediator _mediator;

        public GetCityByIdQueryHandler(ICityRepository cityRepository, IMediator mediator)
        {
            _cityRepository = MethodParameterChecker.CheckUp(cityRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<CityDTO> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _cityRepository.GetByIdAsync(request.Id);

            return result != null ? CityDTO.FromCity(result) : null;
        }
    }


}

