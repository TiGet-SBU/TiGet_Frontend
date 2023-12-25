using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Services;
using Rhazes.Services.Identity.API.Application.Queries;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;

namespace Identity.API.Infrastructure.Mapper
{
    public class RegistryMapper : BaseMapper<Registry, RegistryDTO>
    {
        public RegistryMapper(IIdentityService identityService)
        {
            _identityService = MethodParameterChecker.CheckUp(identityService);
        }

        private readonly IIdentityService _identityService;

        public override RegistryDTO ToDto(Registry registry)
        {
            return new RegistryDTO()
            {
                Id = registry.Id,
                Name = registry.Title,
                Checked = registry.IsActive
            };
        }

        public override Registry ToEntity(RegistryDTO dto)
        {
            var res = new Registry
            {
                Id = string.IsNullOrWhiteSpace(dto.Id.ToString()) ? Guid.Empty :dto.Id,
                Title = dto.Name,
                IsActive = dto.Checked
            };

            return res;
        }
    }
}
