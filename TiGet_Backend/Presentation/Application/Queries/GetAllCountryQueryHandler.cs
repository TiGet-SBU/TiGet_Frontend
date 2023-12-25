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
    public class GetAllCountryQueryHandler : IRequestHandler<GetAllCountryQuery, List<CountryDTO>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMediator _mediator;

        public GetAllCountryQueryHandler(ICountryRepository countryRepository, IMediator mediator)
        {
            _countryRepository = MethodParameterChecker.CheckUp(countryRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<CountryDTO>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
        {
            var result = await _countryRepository.GetAllAsync();

            return result != null ? CountryDTO.FromCountry(result) : null;
        }
    }


    public class CountryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public static List<CountryDTO> FromCountry(IList<Country> countryList)
        {
            List<CountryDTO> countries = new List<CountryDTO>();
            foreach (var country in countryList)
            {
                countries.Add(new CountryDTO()
                {
                    Id = country.Id.ToString(),
                    Name = country.Name,
                    Code = country.Code,
                });
            }
            return countries;
        }

    }


}

