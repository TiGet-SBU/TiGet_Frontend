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
    public class GetAllStateQueryHandler : IRequestHandler<GetAllStateQuery, List<StateDTO>>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMediator _mediator;

        public GetAllStateQueryHandler(IStateRepository stateRepository, IMediator mediator)
        {
            _stateRepository = MethodParameterChecker.CheckUp(stateRepository);
            _mediator = MethodParameterChecker.CheckUp(mediator);
        }

        public async Task<List<StateDTO>> Handle(GetAllStateQuery request, CancellationToken cancellationToken)
        {
            var result = await _stateRepository.GetAllAsync(Guid.Parse(request.CountryId));

            return result != null ? StateDTO.FromState(result) : null;
        }
    }


    public class StateDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Coutry { get; set; }
        public string CoutryId { get; set; }

        public static StateDTO FromState(State stateObj)
        {

            return new StateDTO()
            {
                Id = stateObj.Id.ToString(),
                Name = stateObj.Name,
                Code = stateObj.Code,
            };

        }

        public static List<StateDTO> FromState(IList<State> stateList)
        {
            List<StateDTO> states = new List<StateDTO>();
            foreach (var state in stateList)
            {
                states.Add(new StateDTO()
                {
                    Id = state.Id.ToString(),
                    Name = state.Name,
                    Code = state.Code,
                    Coutry= state.Country.Name,
                    CoutryId = state.CountryId.ToString()
                    //StateId = Guid.Parse("afefce38-b4fc-44a7-bc63-fec6046c72ce"),
                    //CityId = Guid.Parse("86f1d963-c69f-4f7a-93da-341558900893"),
                    //NationalityId = Guid.Parse("5bf70adb-a059-4bbc-96d3-3bf54f19db7d"),
                });
            }
            return states;
        }

    }


}

