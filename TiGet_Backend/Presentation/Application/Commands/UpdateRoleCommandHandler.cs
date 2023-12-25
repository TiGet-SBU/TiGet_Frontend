using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.Services.Identity.Domain.Validators;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDTO>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper<ApplicationRole, RoleDTO> _mapper;
        private readonly IValidator<ApplicationRole> _roleValidator;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper<ApplicationRole, RoleDTO> mapper, IValidator<ApplicationRole> roleValidator)
        {
            _roleRepository = MethodParameterChecker.CheckUp(roleRepository);
            _mapper = MethodParameterChecker.CheckUp(mapper);
            _roleValidator = MethodParameterChecker.CheckUp(roleValidator);
        }

        public async Task<RoleDTO> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var applicationRole = _mapper.ToEntity(command.ObjectDTO);

            await _roleValidator.Validate(applicationRole);

            var response = await _roleRepository.UpdateAsync(applicationRole);
            await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);



            return await Task.FromResult(_mapper.ToDto(response));

        }

    }


}

