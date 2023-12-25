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
    public class GetAllCityQueryHandler : IRequestHandler<GetAllCityQuery, List<CityDTO>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMediator _mediator;

        public GetAllCityQueryHandler(ICityRepository cityRepository, IMediator mediator)
        {
            _cityRepository = MethodParameterChecker.CheckUp(cityRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<CityDTO>> Handle(GetAllCityQuery request, CancellationToken cancellationToken)
        {
            var result = await _cityRepository.GetAllAsync(Guid.Parse(request.StateId));

            return result != null ? CityDTO.FromCity(result) : null;
        }
    }


    public class CityDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string StateId { get; set; }


        public static CityDTO FromCity(City city)
        {
            return new CityDTO()
            {
                Id = city.Id.ToString(),
                Name = city.Name,
                Code = city.Code,
                State = city.State.Name,
                StateId = city.StateId.ToString(),
            };
        }


        public static List<CityDTO> FromCity(IList<City> cityList)
        {
            List<CityDTO> cities = new List<CityDTO>();
            foreach (var city in cityList)
            {
                cities.Add(new CityDTO()
                {
                    Id = city.Id.ToString(),
                    Name = city.Name,
                    Code = city.Code,
                    State = city.State.Name,
                    StateId = city.StateId.ToString(),
                });
            }
            return cities;
        }

    }


}

